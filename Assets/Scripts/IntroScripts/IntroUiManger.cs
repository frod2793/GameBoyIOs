using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroUiManger : MonoBehaviour
{
    [Header ("버튼 UI 목록")]
    [SerializeField]
    private Button startBtn;
    [SerializeField]
    private Button tutorialBtn;
    [SerializeField]
    private Button optionBtn;
    [Header("옵션 팝업")]
    public GameObject OptionPopUP;
    [Header("게임선태 팝업")]
    public GameObject CgamePopUp;


    private void Awake()
    {
        startBtn.onClick.AddListener(func_startBtn);
        tutorialBtn.onClick.AddListener(func_tutorialBtn);
        optionBtn.onClick.AddListener(func_optionBtn);
    }

    private void func_startBtn()
    {//시작 버튼 을 눌렀을떄 실행되야하는 함수
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


}
