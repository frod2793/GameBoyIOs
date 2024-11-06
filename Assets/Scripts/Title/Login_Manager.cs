using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Login_Manager : MonoBehaviour
{
    [Header("회원 가입")] 
    [SerializeField] private GameObject SignUp_PopUp;

    [SerializeField] private TMP_InputField SignUp_NickName_InputField;
    [SerializeField] private TMP_InputField SignUp_ID_InputField;
    [SerializeField] private TMP_InputField SignUp_PW_InputField;
    [SerializeField] private TMP_InputField SignUp_PW_Check_InputField;
    [SerializeField] private Button SignUpBtn;

    public string NickName
    {
        get => SignUp_NickName_InputField.text;
        set => SignUp_NickName_InputField.text = value;
    }

    [Header("로그인")] 
    [SerializeField] private GameObject Login_PopUp;
    [SerializeField] private TMP_InputField Login_ID_InputField;
    [SerializeField] private TMP_InputField Login_PW_InputField;
    [SerializeField] private Button LoginBtn;
    [SerializeField] private Button OpenSingUpPopUp_Btn;
    
    [Header("시작버튼")] 
    [SerializeField] private Button StartBtn;

    [Header("게스트 로그인")]
    [SerializeField]
    private Button GuestLoginBtn;
    [Header("일반 로그인")]
    [SerializeField]
    private Button OpenLoginPopUpBtn;

    //서버 매니져 
    private Server_Manager serverManager;
    private Player_Data_Dontdesytoy playerDataDontdesytoy;
    private string savePath;
    
    void Start()
    {
        serverManager = FindAnyObjectByType<Server_Manager>();
        savePath = Path.Combine(Application.persistentDataPath, "playerData.json");
        playerDataDontdesytoy = FindAnyObjectByType<Player_Data_Dontdesytoy>();

        StartBtn.onClick.AddListener(Func_StartBtn);
        OpenSingUpPopUp_Btn.onClick.AddListener(Func_OpenSingUpPopUp_Btn);
        OpenLoginPopUpBtn.onClick.AddListener(Func_OpenLoginPopUp_Btn);
        GuestLoginBtn.onClick.AddListener(() =>
        {
            Func_GuestLoginBtn();
        });
    }

    private void LoginButtonGroupACtive(bool active)
    {
        GuestLoginBtn.gameObject.SetActive(active);
        OpenSingUpPopUp_Btn.gameObject.SetActive(active);
        OpenLoginPopUpBtn.gameObject.SetActive(active);
    }
    
    private void Awake()
    {
        SignUpBtn.onClick.AddListener(Func_SignUpBtn);
        LoginBtn.onClick.AddListener(Func_LoginBtn);
    }

    private void Func_GuestLoginBtn()
    {
        serverManager.GuestLogin(() =>
        {
            StartBtn.interactable = true;
            StartBtn.gameObject.SetActive(true);
            FindPlayerdata(() =>
            { 
                CreateNewPlayerData(serverManager.NickName, serverManager.UUid);
            });
             LoginButtonGroupACtive(false);
        });
    }
    
    private void Func_SignUpBtn()
    {
        if (SignUp_NickName_InputField.text != "" && SignUp_ID_InputField.text != "" &&
            SignUp_PW_InputField.text != "" && SignUp_PW_Check_InputField.text != "")
        {
            if (SignUp_PW_InputField.text == SignUp_PW_Check_InputField.text)
            {
                //회원가입
                //서버에 회원가입 요청
                //성공시
                serverManager.SignUp(SignUp_ID_InputField.text, SignUp_PW_InputField.text,
                    SignUp_NickName_InputField.text, () => {  
                        SignUp_PopUp.SetActive(false);
                        Login_PopUp.SetActive(true);
                        CreateNewPlayerData(serverManager.NickName, serverManager.UUid);
                        
                    });
              
            }
            else
            {
                Debug.Log("비밀번호가 일치하지 않습니다.");
            }
        }
        else
        {
            Debug.Log("빈칸을 채워주세요");
        }
    }

    IEnumerator CO_Login_Process()
    {
        if (Login_ID_InputField.text != "" && Login_PW_InputField.text != "")
        {
            //로그인
            //서버에 로그인 요청
            //성공시
            serverManager.Login(Login_ID_InputField.text, Login_PW_InputField.text, () =>
            { 
                Login_PopUp.SetActive(false);
                FindPlayerdata(() =>
                { 
                    CreateNewPlayerData(serverManager.NickName, serverManager.UUid);
                });
                
            });
            yield return new WaitForSeconds(1f);
            StartBtn.interactable = true;
            StartBtn.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("빈칸을 채워주세요");
        }
    }

    private void Func_LoginBtn()
    {
        StartCoroutine(CO_Login_Process());
    }
    
    private void Func_OpenSingUpPopUp_Btn()
    {
        SignUp_PopUp.SetActive(true);
        Login_PopUp.SetActive(false);
    }

    private void Func_OpenLoginPopUp_Btn()
    {
        Login_PopUp.SetActive(true);
        SignUp_PopUp.SetActive(false);
    }
    
    private void SearchPlayerData()
    {
        if (File.Exists(savePath))
        {
            LoadPlayerData();
        }
        else
        {
            // PlayerProfil_Create_PopUp.SetActive(true);
            StartBtn.interactable = false;
        }
    }

    private void CreateNewPlayerData(string playerName, string uid)
    {
        playerDataDontdesytoy.scritpableobj_playerData.InitializePlayerData(playerName, uid);
        StartBtn.interactable = true;
        InsertPlayerData(); // 새로 생성한 데이터를 저장
    }

    public void InsertPlayerData()
    {
        // ScriptableObject를 JSON으로 직렬화
        string jsonData = JsonUtility.ToJson(playerDataDontdesytoy.scritpableobj_playerData, true);

        // 파일에 저장
        File.WriteAllText(savePath, jsonData);
        Debug.Log("PlayerData saved to: " + savePath);

        //저장된 파일을 클라우드에 저장
        Debug.Log(playerDataDontdesytoy.scritpableobj_playerData.nickname);
        serverManager.GameDataInsert(playerDataDontdesytoy.scritpableobj_playerData);

    }


    private void LoadPlayerData()
    {
        if (File.Exists(savePath))
        {
            // JSON 파일을 읽어와 ScriptableObject에 덮어씌움
            string jsonData = File.ReadAllText(savePath);
            if (playerDataDontdesytoy == null)
            {
                playerDataDontdesytoy = FindFirstObjectByType<Player_Data_Dontdesytoy>();
            }

            if (playerDataDontdesytoy.scritpableobj_playerData == null)
            {
                playerDataDontdesytoy.scritpableobj_playerData = ScriptableObject.CreateInstance<PlayerData>();
            }

            JsonUtility.FromJsonOverwrite(jsonData, playerDataDontdesytoy.scritpableobj_playerData);
            Debug.Log("PlayerData loaded from: " + savePath);
        }
        else
        {
            Debug.LogWarning("No PlayerData file found at: " + savePath);
        }
    }

    private void FindPlayerdata(Action action)
    {
        serverManager.GameDataGet(action);
    }
    

    private void Func_StartBtn()
    {
        SceneLoader.Instace.LoadScene("LobbyScene");
    }


    private void DeletePlayerData()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("PlayerData deleted from: " + savePath);
        }
        else
        {
            Debug.LogWarning("No PlayerData file found at: " + savePath);
        }
    }
}