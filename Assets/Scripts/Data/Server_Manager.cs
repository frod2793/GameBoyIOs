using BackEnd;
using UnityEngine;
// using UnityGoogleDrive;
public class Server_Manager : MonoBehaviour
{
    
    public string UUid;
    public string NickName;
    
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
    
    public void SignUp(string id, string pw, string nickname)
    {
        BackendReturnObject bro = Backend.BMember.CustomSignUp(id, pw);
        if (bro.IsSuccess())
        {
            Debug.Log("회원가입 성공");
            
        }
        else
        {
            Debug.Log("회원가입 실패");
        }
        bro = Backend.BMember.UpdateNickname(nickname);
        if (bro.IsSuccess())
        {
            Debug.Log("닉네임 변경 성공");
        }
        else
        {
            
        }
       
    }
    
    public void Login(string id, string pw)
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
            
        }
        else
        {
            Debug.Log("로그인 실패");
        }
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
            Debug.Log("게임 데이터 저장 성공");
        }
        else
        {
            Debug.Log("게임 데이터 저장 실패");
        }
    }
 }
 

