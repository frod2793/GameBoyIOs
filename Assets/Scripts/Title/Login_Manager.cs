using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DogGuns_Games
{
    public class Login_Manager : MonoBehaviour
    {
        [Header("회원 가입")] [SerializeField] private GameObject SignUp_PopUp;

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

        [Header("로그인")] [SerializeField] private GameObject Login_PopUp;
        [SerializeField] private TMP_InputField Login_ID_InputField;
        [SerializeField] private TMP_InputField Login_PW_InputField;
        [SerializeField] private Button LoginBtn;
        [SerializeField] private Button OpenSingUpPopUp_Btn;

        [Header("시작버튼")] [SerializeField] private Button StartBtn;

        [Header("게스트 로그인")] [SerializeField] private Button GuestLoginBtn;

        [Header("일반 로그인")] [SerializeField] private Button OpenLoginPopUpBtn;

        //서버 매니져 
        private Server_Manager serverManager;
        private Player_Data_Manager_Dontdesytoy _playerDataManagerDontdesytoy;
        private string savePath;

        private void Awake()
        {
            SignUpBtn.onClick.AddListener(Func_SignUpBtn);
            LoginBtn.onClick.AddListener(Func_LoginBtn);
        }

        void Start()
        {
            serverManager = FindAnyObjectByType<Server_Manager>();
            savePath = Path.Combine(Application.persistentDataPath, "playerData.json");
            _playerDataManagerDontdesytoy = FindAnyObjectByType<Player_Data_Manager_Dontdesytoy>();

            StartBtn.onClick.AddListener(Func_StartBtn);
            OpenSingUpPopUp_Btn.onClick.AddListener(Func_OpenSingUpPopUp_Btn);
            OpenLoginPopUpBtn.onClick.AddListener(Func_OpenLoginPopUp_Btn);
            GuestLoginBtn.onClick.AddListener(() => { Func_GuestLoginBtn(); });

            LoginButtonGroupACtive(false);
        }

        /// <summary>
        ///   토큰 로그인
        /// </summary>
        private void TokenLogin()
        {
            serverManager.TokenLogin(
                onSuccess: () => { SceneLoader.Instace.LoadScene("LobbyScene"); },
                onFailure: () => LoginButtonGroupACtive(true)
            );
        }

        /// <summary>
        /// 로그인 버튼 그룹 활성화, 비활성화
        /// </summary>
        /// <param name="active"></param>
        private void LoginButtonGroupACtive(bool active)
        {
            GuestLoginBtn.gameObject.SetActive(active);
            OpenSingUpPopUp_Btn.gameObject.SetActive(active);
            OpenLoginPopUpBtn.gameObject.SetActive(active);
        }

        /// <summary>
        /// 게스트 로그인 버튼 함수 
        /// </summary>
        private void Func_GuestLoginBtn()
        {
            serverManager.GuestLogin(() =>
            {
                StartBtn.interactable = true;
                StartBtn.gameObject.SetActive(true);
                FindPlayerdata(() => { CreateNewPlayerData(serverManager.NickName, serverManager.UUid); });
                SceneLoader.Instace.LoadScene("LobbyScene");
                LoginButtonGroupACtive(false);
            });
        }

        /// <summary>
        /// 회원 가입 버튼 함수
        /// </summary>
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
                        SignUp_NickName_InputField.text, () =>
                        {
                            SignUp_PopUp.SetActive(false);
                            Login_PopUp.SetActive(true);
                            CreateNewPlayerData(serverManager.NickName, serverManager.UUid);
                            SceneLoader.Instace.LoadScene("LobbyScene");
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

        /// <summary>
        /// 로그인 프로세스 코루틴
        /// </summary>
        /// <returns></returns>
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
                    FindPlayerdata(() => { CreateNewPlayerData(serverManager.NickName, serverManager.UUid); });
                });
                yield return new WaitForSeconds(1f);
                StartBtn.interactable = true;
                StartBtn.gameObject.SetActive(true);
                SceneLoader.Instace.LoadScene("LobbyScene");
            }
            else
            {
                Debug.Log("빈칸을 채워주세요");
            }
        }

        /// <summary>
        /// 로그인 버튼 함수
        /// </summary>
        private void Func_LoginBtn()
        {
            StartCoroutine(CO_Login_Process());
        }

        /// <summary>
        /// 회원 가입 팝업 열기 함수 
        /// </summary>
        private void Func_OpenSingUpPopUp_Btn()
        {
            SignUp_PopUp.SetActive(true);
            Login_PopUp.SetActive(false);
        }

        /// <summary>
        /// 로그인 팝업 열기 함수 
        /// </summary>
        private void Func_OpenLoginPopUp_Btn()
        {
            Login_PopUp.SetActive(true);
            SignUp_PopUp.SetActive(false);
        }

        /// <summary>
        /// 새로운 플레이어 데이터 생성
        /// </summary>
        /// <param name="playerName">  </param>
        /// <param name="uid"></param>
        private void CreateNewPlayerData(string playerName, string uid)
        {
            _playerDataManagerDontdesytoy.scritpableobj_playerData.InitializePlayerData(playerName, uid);
            StartBtn.interactable = true;
            InsertPlayerData(); // 새로 생성한 데이터를 저장
        }

        /// <summary>
        ///     플레이어 데이터 저장
        /// </summary>
        public void InsertPlayerData()
        {
            // ScriptableObject를 JSON으로 직렬화
            string jsonData = JsonUtility.ToJson(_playerDataManagerDontdesytoy.scritpableobj_playerData, true);

            // 파일에 저장
            File.WriteAllText(savePath, jsonData);
            Debug.Log("PlayerData saved to: " + savePath);

            //저장된 파일을 클라우드에 저장
            Debug.Log(_playerDataManagerDontdesytoy.scritpableobj_playerData.nickname);
            serverManager.GameDataInsert(_playerDataManagerDontdesytoy.scritpableobj_playerData);
        }


        private void LoadPlayerData()
        {
            if (File.Exists(savePath))
            {
                // JSON 파일을 읽어와 ScriptableObject에 덮어씌움
                string jsonData = File.ReadAllText(savePath);
                if (_playerDataManagerDontdesytoy == null)
                {
                    _playerDataManagerDontdesytoy = FindFirstObjectByType<Player_Data_Manager_Dontdesytoy>();
                }

                if (_playerDataManagerDontdesytoy.scritpableobj_playerData == null)
                {
                    _playerDataManagerDontdesytoy.scritpableobj_playerData =
                        ScriptableObject.CreateInstance<PlayerData>();
                }

                JsonUtility.FromJsonOverwrite(jsonData, _playerDataManagerDontdesytoy.scritpableobj_playerData);
                Debug.Log("PlayerData loaded from: " + savePath);
            }
            else
            {
                Debug.LogWarning("No PlayerData file found at: " + savePath);
            }
        }

        /// <summary>
        /// 플레이어 데이터 찾기
        /// </summary>
        /// <param name="action">게임 데이터가 존재하지않을떄 실행할 액션</param>
        private void FindPlayerdata(Action action)
        {
            serverManager.GameDataGet(action);
        }

        /// <summary>
        /// 시작 버튼 함수
        /// </summary>
        private void Func_StartBtn()
        {
            TokenLogin();
        }

        /// <summary>
        ///     플레이어 데이터 삭제
        /// </summary>
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
}