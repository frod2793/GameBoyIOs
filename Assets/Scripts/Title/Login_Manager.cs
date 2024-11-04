using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Login_Manager : MonoBehaviour
{
    
    [Header("회원 가입")]
    [SerializeField]
    private GameObject SignUp_PopUp;

    [SerializeField] 
    private TMP_InputField SignUp_NickName_InputField;
    [SerializeField]
    private TMP_InputField SignUp_ID_InputField;
    [SerializeField]
    private TMP_InputField SignUp_PW_InputField;
    [SerializeField]
    private TMP_InputField SignUp_PW_Check_InputField;
    [SerializeField]
    private Button SignUpBtn;

    
    private PlayerProfil_Manager playerProfilManager;
    public string NickName
    {
        get => SignUp_NickName_InputField.text;
        set => SignUp_NickName_InputField.text = value; 
    }

    [Header("로그인")]
    [SerializeField]
    private GameObject Login_PopUp;
    [SerializeField]
    private TMP_InputField Login_ID_InputField;
    [SerializeField]
    private TMP_InputField Login_PW_InputField;
    [SerializeField]
    private Button LoginBtn;
    
    //서버 매니져 
    private Server_Manager serverManager;
    
       void Start()
    {
        serverManager = FindAnyObjectByType<Server_Manager>();
        playerProfilManager = FindAnyObjectByType<PlayerProfil_Manager>();
    }

    private void Awake()
    {
        SignUpBtn.onClick.AddListener(Func_SignUpBtn);
        LoginBtn.onClick.AddListener(Func_LoginBtn);
    }
    
  private void Func_SignUpBtn()
    {
        if (SignUp_NickName_InputField.text != "" && SignUp_ID_InputField.text != "" && SignUp_PW_InputField.text != "" && SignUp_PW_Check_InputField.text != "")
        {
            if (SignUp_PW_InputField.text == SignUp_PW_Check_InputField.text)
            {
                //회원가입
                //서버에 회원가입 요청
                //성공시
                serverManager.SignUp(SignUp_ID_InputField.text, SignUp_PW_InputField.text, SignUp_NickName_InputField.text);
                
                
                
                SignUp_PopUp.SetActive(false);
                Login_PopUp.SetActive(true);
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
            serverManager.Login(Login_ID_InputField.text, Login_PW_InputField.text);
            yield return new WaitForSeconds(1f);
            playerProfilManager.Func_CreateBtn();
            Login_PopUp.SetActive(false);
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
}
