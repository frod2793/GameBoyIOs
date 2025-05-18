using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DogGuns_Games.Lobby
{
    /// <summary>
    /// 로비 UI를 관리하는 클래스
    /// </summary>
    public class LobbyUIManager : MonoBehaviour
    {
        #region 변수 및 필드

        [Header("<color=green>플레이 및 설정 버튼 UI 목록</color>")] [SerializeField]
        private Button startBtn;

        [SerializeField] private Button tutorialBtn;
        [SerializeField] private Button optionBtn;

        [Header("<color=green>팝업 UI</color>")] [SerializeField] private GameObject optionPopUp;
        [SerializeField] private GameObject cgamePopUp;

        [Header("<color=green>캐릭터 시스템</color>")] [SerializeField]
        private CharacterSelectUIManager characterSelectUIManager;

        [SerializeField] private Button openCharacterSelectButton;
        [SerializeField] private Button openCharacterListPanel;
        [SerializeField] private Button openCharacterSkillViewPanel;
        [SerializeField] private Button closeCharacterSelectButton;

        [Header("<color=green>우편 시스템</color>")] [SerializeField]
        private PostManager postManager;

        [SerializeField] private Button openMessingerButton;
        [SerializeField] private Button getPostReiwordButton;
        [SerializeField] private Button closeMessingerButton;
        [SerializeField] private Button getPostExpensionReiwordButton;
        [SerializeField] private Button closeMessingerExpensionButton;

        [Header("<color=green>퀘스트 시스템</color>")] [SerializeField]
        private QuestPanelManager questPanelManager;

        [SerializeField] private Button openQuestPanelButton;
        [SerializeField] private Button closeQuestPanelButton;
        [SerializeField] private Button closeQuestExpensionButton;

        [Header("<color=green>재화 시스템</color>")] [SerializeField]
        private TMP_Text gold;

        [SerializeField] private TMP_Text dia;


        [Header("상점 시스템")] [SerializeField] private StoreManager storeManager;
        [SerializeField] private Button openStoreButton;
        [SerializeField] private Button closeStoreButton;


        [Header("플레이어 정보")] [SerializeField] private PlayerDataManagerDontdesytoy playerDataManagerDontdesytoy;

        public static Action closePopUpAction;

        private static List<Action> closePopUpActionList = new List<Action>();

        // 상수
        private const string ErrorNullReference = "참조가 없습니다: {0}";

        #endregion

        #region Unity 라이프사이클

        /// <summary>
        /// 초기화 작업 수행
        /// </summary>
        private void Awake()
        {
            // 플레이어 데이터 매니저 찾기
            playerDataManagerDontdesytoy = FindAnyObjectByType<PlayerDataManagerDontdesytoy>();

            // 버튼 초기화
            InitializeButtons();
        }

        /// <summary>
        /// 화면에 재화 정보 표시
        /// </summary>
        private void Start()
        {
            UpdateCurrencyDisplay();
        }

        #endregion

        #region 초기화 메서드

        /// <summary>
        /// 모든 버튼 초기화
        /// </summary>
        private void InitializeButtons()
        {
            playButton_Init();
            CharacterSelct_Init();
            InitOtherSystems();
            InitQuestManager();
            InitPostManager();
            InitStoreManager();
        }

        /// <summary>
        /// 게임 관련 버튼 이벤트 등록
        /// </summary>
        private void playButton_Init()
        {
            if (startBtn != null)
                startBtn.onClick.AddListener(func_startBtn);
            else
                Debug.LogError(string.Format(ErrorNullReference, "시작 버튼"));

            if (tutorialBtn != null)
                tutorialBtn.onClick.AddListener(func_tutorialBtn);
            else
                Debug.LogError(string.Format(ErrorNullReference, "튜토리얼 버튼"));

            if (optionBtn != null)
                optionBtn.onClick.AddListener(func_optionBtn);
            else
                Debug.LogError(string.Format(ErrorNullReference, "옵션 버튼"));
        }

        /// <summary>
        /// 캐릭터 선택창 버튼 이벤트 등록
        /// </summary>
        private void CharacterSelct_Init()
        {
            if (characterSelectUIManager == null)
            {
                Debug.LogError(string.Format(ErrorNullReference, "캐릭터 선택 매니저"));
                return;
            }

            if (openCharacterSelectButton != null)
                openCharacterSelectButton.onClick.AddListener(characterSelectUIManager.OpenCharacterSelectPanel);
            else
                Debug.LogError(string.Format(ErrorNullReference, "캐릭터 선택창 열기 버튼"));

            if (openCharacterListPanel != null)
                openCharacterListPanel.onClick.AddListener(characterSelectUIManager.OpenCharacterListPanel);
            else
                Debug.LogError(string.Format(ErrorNullReference, "캐릭터 리스트 열기 버튼"));

            if (openCharacterSkillViewPanel != null)
                openCharacterSkillViewPanel.onClick.AddListener(characterSelectUIManager.OpenCharacterSkillViewPanel);
            else
                Debug.LogError(string.Format(ErrorNullReference, "캐릭터 스킬뷰 열기 버튼"));

            if (closeCharacterSelectButton != null)
                closeCharacterSelectButton.onClick.AddListener(characterSelectUIManager.CloseCharacterSelectPanel);
            else
                Debug.LogError(string.Format(ErrorNullReference, "캐릭터 선택창 닫기 버튼"));
        }

        /// <summary>
        /// 기타 시스템 초기화 (우편함, 퀘스트 등)
        /// </summary>
        private void InitOtherSystems()
        {
            if (openMessingerButton != null && postManager != null)
                openMessingerButton.onClick.AddListener(postManager.OpenPostBoxPanel);
            else
                Debug.LogError(string.Format(ErrorNullReference, "우편함 버튼 또는 매니저"));

            if (openQuestPanelButton != null && questPanelManager != null)
                openQuestPanelButton.onClick.AddListener(questPanelManager.OpenQuestPanel);
            else
                Debug.LogError(string.Format(ErrorNullReference, "퀘스트 버튼 또는 매니저"));
        }

        /// <summary>
        /// 상점 시스템 초기화
        /// </summary>
        private void InitStoreManager()
        {
            if (openStoreButton != null && storeManager != null)
                openStoreButton.onClick.AddListener(() => storeManager.OpenStorePanel());
            else
                Debug.LogError(string.Format(ErrorNullReference, "상점 버튼 또는 매니저"));

            if (closeStoreButton != null && storeManager != null)
                closeStoreButton.onClick.AddListener(() => storeManager.CloseStoreItemPopUp());
            else
                Debug.LogError(string.Format(ErrorNullReference, "상점 닫기 버튼 또는 매니저"));
        }


        private void InitQuestManager()
        {
            if (closeQuestPanelButton != null && questPanelManager != null)
                closeQuestPanelButton.onClick.AddListener(CloseButtonClick);
            else
                Debug.LogError(string.Format(ErrorNullReference, "퀘스트 닫기 버튼 또는 매니저"));

            if (closeQuestExpensionButton != null && questPanelManager != null)
                closeQuestExpensionButton.onClick.AddListener(CloseButtonClick);
            else
                Debug.LogError(string.Format(ErrorNullReference, "퀘스트 확장 닫기 버튼 또는 매니저"));
        }

        /// <summary>
        /// 우편 시스템 관련 버튼 이벤트 초기화
        /// </summary>
        private void InitPostManager()
        {
            // 우편함 닫기 버튼 초기화
            if (closeMessingerButton != null && postManager != null)
            {
                closeMessingerButton.onClick.AddListener(CloseButtonClick);
                Debug.Log("우편함 닫기 버튼 이벤트 등록 완료");
            }
            else
            {
                Debug.LogError(string.Format(ErrorNullReference, "우편함 닫기 버튼 또는 매니저"));
            }

            // 우편함 확장 패널 닫기 버튼 초기화
            if (closeMessingerExpensionButton != null && postManager != null)
            {
                closeMessingerExpensionButton.onClick.AddListener(CloseButtonClick);
                Debug.Log("우편함 확장 닫기 버튼 이벤트 등록 완료");
            }
            else
            {
                Debug.LogError(string.Format(ErrorNullReference, "우편함 확장 닫기 버튼 또는 매니저"));
            }

            // 우편 보상 수령 버튼 초기화
            if (getPostExpensionReiwordButton != null && postManager != null)
            {
                getPostExpensionReiwordButton.onClick.AddListener(() =>
                {
                    postManager.Getreward();
                    CloseButtonClick();
                });

              //  getPostReiwordButton.onClick.AddListener(() => { postManager.Getreward(); });

                Debug.Log("우편 보상 수령 버튼 이벤트 등록 완료");
            }
            else
            {
                Debug.LogError(string.Format(ErrorNullReference, "우편 보상 수령 버튼 또는 매니저"));
            }
        }


        private void ClickmobileBackButton()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //todo: closePopUpActionList 에 등록되어 있는 액션중 최근 에 등록된 액션을 실행
                CloseButtonClick();
            }
        }

        public static void AddClosePopUpAction(Action action)
        {
            if (action != null)
            {
                closePopUpActionList.Add(action);
            }
            else
            {
                Debug.LogError(string.Format(ErrorNullReference, "액션"));
            }
        }

        private void CloseButtonClick()
        {
            if (closePopUpActionList.Count > 0)
            {
                closePopUpActionList[closePopUpActionList.Count - 1].Invoke();
                closePopUpActionList.RemoveAt(closePopUpActionList.Count - 1);
            }
            else
            {
                Debug.Log("팝업이 없습니다.");
            }
        }

        #endregion

        #region UI 업데이트 메서드

        /// <summary>
        /// 화면에 재화 정보 업데이트
        /// </summary>
        private void UpdateCurrencyDisplay()
        {
            if (playerDataManagerDontdesytoy?.scritpableobjPlayerData == null)
            {
                Debug.LogError(string.Format(ErrorNullReference, "플레이어 데이터"));
                return;
            }

            if (gold != null)
                gold.text = playerDataManagerDontdesytoy.scritpableobjPlayerData.currency1.ToString();

            if (dia != null)
                dia.text = playerDataManagerDontdesytoy.scritpableobjPlayerData.currency2.ToString();
        }

        #endregion

        #region 버튼 콜백 함수

        /// <summary>
        /// 시작 버튼 콜백 - 게임 선택 팝업 표시
        /// </summary>
        private void func_startBtn()
        {
            Debug.Log("게임 선택 팝업");

            if (cgamePopUp != null)
                cgamePopUp.SetActive(true);
            else
                Debug.LogError(string.Format(ErrorNullReference, "게임 선택 팝업"));
        }

        /// <summary>
        /// 튜토리얼 버튼 콜백 - 미구현
        /// </summary>
        private void func_tutorialBtn()
        {
            // 튜토리얼 기능 구현 예정
            Debug.Log("튜토리얼 기능 호출됨 (미구현)");
        }

        /// <summary>
        /// 옵션 버튼 콜백 - 옵션 팝업 표시
        /// </summary>
        private void func_optionBtn()
        {
            if (optionPopUp != null)
            {
                // 새로운 인스턴스 생성 대신 활성화
                optionPopUp.SetActive(true);
            }
            else
                Debug.LogError(string.Format(ErrorNullReference, "옵션 팝업"));
        }

        #endregion

        #region 게임 진행 함수

        /// <summary>
        /// 게임 실행 - 씬 전환
        /// </summary>
        private void runGame()
        {
            if (SceneLoader.Instace != null)
            {
                SceneLoader.Instace.LoadScene("RunGame");
                Debug.Log("런게임 씬으로 전환");
            }
            else
            {
                Debug.LogError(string.Format(ErrorNullReference, "씬 로더"));
            }
        }

        #endregion
    }
}