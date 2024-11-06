using System;
using System.Text;
using BackEnd;
using UnityEngine;
// using UnityGoogleDrive;
public class Server_Manager : MonoBehaviour
{
    
    public string UUid;
    public string NickName;
    private string gameDataRowInDate = string.Empty;
    PlayerData playerData;
    //파괴되지않는 오브젝트
    private static Server_Manager instance;
    public static Server_Manager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<Server_Manager>();
                if (instance == null)
                {
                    GameObject container = new GameObject("Server_Manager");
                    instance = container.AddComponent<Server_Manager>();
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
        BackEndInIt();
    }
    void BackEndInIt() 
    {
        var bro = Backend.Initialize(); // 뒤끝 초기화

        // 뒤끝 초기화에 대한 응답값
        if(bro.IsSuccess()) {
            Debug.Log("초기화 성공 : " + bro); // 성공일 경우 statusCode 204 Success
        } else {
            Debug.LogError("초기화 실패 : " + bro); // 실패일 경우 statusCode 400대 에러 발생
        }
    }
    
    public void SignUp(string id, string pw, string nickname,Action ac)
    {
        BackendReturnObject bro = Backend.BMember.CustomSignUp(id, pw);
        if (bro.IsSuccess())
        {
            Debug.Log("회원가입 성공: "+bro);
            bro = Backend.BMember.UpdateNickname(nickname);
            if (bro.IsSuccess())
            {
                Debug.Log("닉네임 변경 성공: "+bro);
                ac.Invoke();
                
            }
            else
            {
                Debug.Log("닉네임 변경 실패: "+bro);
                ErroDebug(bro);
            }
        }
        else
        {
            Debug.Log("회원가입 실패: "+bro);
            ErroDebug(bro);
        }
     
       
    }
    
    public void Login(string id, string pw,Action ac)
    {
        BackendReturnObject bro = Backend.BMember.CustomLogin(id, pw);
        if (bro.IsSuccess())
        {
            Debug.Log("로그인 성공");
            Debug.Log(bro);

            UUid = Backend.UID;
            NickName = Backend.UserNickName;
            Debug.Log("UUid: "+UUid);
            Debug.Log("NickName: "+NickName);
       
            
            ac.Invoke();
            
        }
        else
        {
            Debug.Log("로그인 실패: "+bro);
            ErroDebug(bro);
        }
    }

    public void GuestLogin(Action action)
    {
        BackendReturnObject bro = Backend.BMember.GuestLogin("게스트 로그인으로 로그인함");
        if(bro.IsSuccess())
        {
            Debug.Log("게스트 로그인에 성공했습니다: "+bro);
            UUid = Backend.UID;
            NickName = Backend.UserNickName;
            Debug.Log("UUid: "+UUid);
            Debug.Log("NickName: "+NickName);
            action.Invoke();
        }
        else
        {
            Debug.Log("게스트 로그인에 실패했습니다: "+bro);
            ErroDebug(bro);
        }
       
    }
    
    private void ErroDebug(BackendReturnObject bro)
    {
       // bro = Backend.BMember.CustomLogin;
        Debug.Log(bro.GetStatusCode());
        Debug.Log(bro.GetErrorCode());
        Debug.Log(bro.GetMessage());
    }
    
    public void GameDataInsert(PlayerData playerData)
    {
        Param param = new Param();
        param.Add("nickname", playerData.nickname);
        param.Add("uid", playerData.UID);
        param.Add("Money1", playerData.currency1);
        param.Add("Money2", playerData.currency2);
        param.Add("experience", playerData.experience);
        param.Add("level", playerData.level);
        
        
        BackendReturnObject bro = Backend.GameData.Insert("User_Data", param);
        if (bro.IsSuccess())
        {
            gameDataRowInDate = bro.GetInDate();
            Debug.Log("게임 데이터 저장 성공");
        }
        else
        {
            Debug.Log("게임 데이터 저장 실패");
        }
    }
    
    public void GameDataGet(Action action)
    {
        Debug.Log("게임 정보 조회 함수를 호출합니다.");

        var bro = Backend.GameData.GetMyData("User_Data", new Where());

        if (bro.IsSuccess())
        {
            Debug.Log("게임 정보 조회에 성공했습니다. : " + bro);


            LitJson.JsonData gameDataJson = bro.FlattenRows(); // Json으로 리턴된 데이터를 받아옵니다.  

            // 받아온 데이터의 갯수가 0이라면 데이터가 존재하지 않는 것입니다.  
            if (gameDataJson.Count <= 0)
            {
                Debug.LogWarning("데이터가 존재하지 않습니다.: " + bro);
                action.Invoke();
            }
            else
            {
                gameDataRowInDate = gameDataJson[0]["inDate"].ToString(); //불러온 게임 정보의 고유값입니다.  

                gameDataRowInDate = gameDataJson[0]["inDate"].ToString(); //불러온 게임 정보의 고유값입니다.

                playerData = ScriptableObject.CreateInstance<PlayerData>();

                playerData.level = int.Parse(gameDataJson[0]["level"].ToString());
                playerData.currency1 = int.Parse(gameDataJson[0]["Money1"].ToString());
                playerData.currency2 = int.Parse(gameDataJson[0]["Money2"].ToString());
                playerData.experience = float.Parse(gameDataJson[0]["experience"].ToString());
                playerData.UID = gameDataJson[0]["uid"].ToString();
                // playerData.nickname = gameDataJson[0]["nickname"].ToString();

                Player_Data_Dontdesytoy.Instance.UpdatePlayerData(playerData);

                Debug.Log(playerData.ToString());


                // foreach (string itemKey in gameDataJson[0]["inventory"].Keys)
                // {
                //     userData.inventory.Add(itemKey, int.Parse(gameDataJson[0]["inventory"][itemKey].ToString()));
                // }
                //
                // foreach (LitJson.JsonData equip in gameDataJson[0]["equipment"])
                // {
                //     userData.equipment.Add(equip.ToString());
                // }

                Debug.Log(playerData.ToString());
            }
        }
        else
        {
            Debug.LogError("게임 정보 조회에 실패했습니다. : " + bro);
            ErroDebug(bro);

            if (bro.GetStatusCode() == "404")
            {
                action.Invoke();
            }
        }
    }
    
    
    public void GameDataUpdate(PlayerData playerData)
    {
        Param param = new Param();
        param.Add("nickname", playerData.nickname);
        param.Add("uid", playerData.UID);
        param.Add("Money1", playerData.currency1);
        param.Add("Money2", playerData.currency2);
        param.Add("experience", playerData.experience);
        param.Add("level", playerData.level);
        
        BackendReturnObject bro = null;
        
        
        if (string.IsNullOrEmpty(gameDataRowInDate))
        {
            Debug.Log("내 제일 최신 게임 정보 데이터 수정을 요청합니다.");

            bro = Backend.GameData.Update("User_Data", new Where(), param);
        }
        else
        {
            Debug.Log($"{gameDataRowInDate}의 게임 정보 데이터 수정을 요청합니다.");

            bro = Backend.GameData.UpdateV2("User_Data", gameDataRowInDate, Backend.UserInDate, param);
        }

        if (bro.IsSuccess())
        {
            Debug.Log("게임 정보 데이터 수정에 성공했습니다. : " + bro);
        }
        else
        {
            Debug.LogError("게임 정보 데이터 수정에 실패했습니다. : " + bro);
        }
    }
    
 }
 

