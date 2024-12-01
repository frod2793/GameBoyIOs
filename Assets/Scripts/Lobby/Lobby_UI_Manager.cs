using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Lobby_UI_Manager : MonoBehaviour
{
    [Header("<color=green>플레이 및 설정 버튼 UI 목록</color>")] [SerializeField]
    private Button startBtn;

    [SerializeField] private Button tutorialBtn;
    [SerializeField] private Button optionBtn;
    [Header("옵션 팝업")] public GameObject OptionPopUP;
    [Header("게임선택 팝업")] public GameObject CgamePopUp;

    [Header("<color=green>캐릭터 선택 매니져</color>")] [SerializeField]
    private SelectCharacter_UI_Manager selectCharacterUIManager;

    [Header("캐릭터 선택창 열기 버튼")] [SerializeField]
    private Button openCharacterSelectButton;

    [Header("캐릭터 리스트 열기 버튼")] [SerializeField]
    private Button openCharacterListPanel;

    [Header("캐릭터 스킬뷰 열기 버튼")] [SerializeField]
    private Button openCharacterSkillViewPanel;

    [Header("캐릭터 선택창 닫기 버튼")] [SerializeField]
    private Button closeCharacterSelectButton;

    [FormerlySerializedAs("messingerManager")] [Header("<color=green>우편함 매니져</color>")] [SerializeField]
    private Post_Manager postManager;

    [Header("우편함 열기 버튼")] [SerializeField] private Button openMessingerButton;
    [Header("우편함 닫기 버튼")] [SerializeField] private Button closeMessingerButton;

    [Header("<color=green>재화 표시</color>")] [SerializeField]
    private TMP_Text Gold;

    [SerializeField] private TMP_Text dia;

    [FormerlySerializedAs("playerDataDontdesytoy")] [Header("플레이어 정보")] [SerializeField] private Player_Data_Manager_Dontdesytoy playerDataManagerDontdesytoy;

    private void Awake()
    {
        playerDataManagerDontdesytoy = FindAnyObjectByType<Player_Data_Manager_Dontdesytoy>();
        playButton_Init();
        CharacterSelct_Init();
    }

    private void Start()
    {
        Gold.text = playerDataManagerDontdesytoy.scritpableobj_playerData.currency1.ToString();
        dia.text = playerDataManagerDontdesytoy.scritpableobj_playerData.currency2.ToString();
    }

    /// <summary>
    /// 버튼 초기화
    /// </summary>
    private void playButton_Init()
    {
        startBtn.onClick.AddListener(func_startBtn);
        tutorialBtn.onClick.AddListener(func_tutorialBtn);
        optionBtn.onClick.AddListener(func_optionBtn);
    }

    /// <summary>
    ///  캐릭터 선택창 버튼 초기화
    /// </summary>
    private void CharacterSelct_Init()
    {
        openCharacterSelectButton.onClick.AddListener(() => selectCharacterUIManager.OpenCharacterSelectPanel());
        openCharacterListPanel.onClick.AddListener(() => { selectCharacterUIManager.OpenCharacterListPanel(); });
        openCharacterSkillViewPanel.onClick.AddListener(() =>
        {
            selectCharacterUIManager.OpenCharacterSkillViewPanel();
        });
        closeCharacterSelectButton.onClick.AddListener(() => { selectCharacterUIManager.CloseCharacterSelectPanel(); });
    }

    private void func_startBtn()
    {
        //시작 버튼 을 눌렀을떄 실행되야하는 함수
        //임시 기능
        //  SceneLoader.Instace.LoadScene("SampleScene");
        Debug.Log("게임 선택 팝업");

        CgamePopUp.SetActive(true);
    }


    private void func_tutorialBtn()
    {
    }

    private void func_optionBtn()
    {
        Instantiate(OptionPopUP);
        OptionPopUP.SetActive(true);
    }

    private void runGame()
    {
        SceneLoader.Instace.LoadScene("RunGame");
        Debug.Log("런게임");
    }
}