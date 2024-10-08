using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RunGameUiManager : MonoBehaviour
{
    public SettingsData_oBJ settingsData; // ScriptableObject 참조
    
    [Header("플레이어")]
    public Button JumpBtn;
    public Slider Hpbar;
    public float JumpHight;
    public float JumpSpeed;
    
    
    [Header("초당 깎이는 Hp")]
    public float discountHp;

    public float maxHp;
 
    public float currenthp;

    [Header("점수 오브젝트 ")]
    public float CoinSpeed;
    public List<GameObject> CoinPool = new List<GameObject>();
    public GameObject[] Coins;
    public int Coin_Objcount;
    public Vector2 C_startpoint;

    [Header("체력 회복 오브젝트")]
    public GameObject HealObj;
    public float PointHp;//체력 회복 아이템이 스폰되는 기준 hp
    public List<GameObject> Healpool;

    [Header("거인화 오브젝트 ")]
    public GameObject Biggerobj;
  
    public List<GameObject> Biggerpool;


    [Header("장애물")]
    public List<GameObject> MobPool = new List<GameObject>();
    public GameObject[] Mobs;
    public int Objcount;
    public float MobSpeed;
    public Vector2 startpoint;

    [Header("배경 땅")]
    public SpriteRenderer[] tiles;
    public Sprite[] GroundImage;
    public float Groundspeed;

    [Header("UI 버튼 & PopUP")]
    public Button PuseBtn;
    public Button PlayBtn;
    public Text Scoretext;
    public bool isPlay = false;
    
    [Header("Puse 팝업")]
    public Image PusePopup;
    public Button PopUp_playBtn;
    public Button PopUp_ResetBtn;
    public Button PopUp_OPtionBtn;
    [Header("옵션 팝업")]
    public OptionPopupManager OptionPopUP;
    
   // public bool isPuse = false;

    public delegate void Onplay(bool isPlay);
    public Onplay onPlay;
    [Header("힐 아이템 목표 점")]
    public int score = 0;
    public int NextHealScore;//목표점수 
    public int HealScore;//더할점수

    [Header("거대화 아이템 목표 점수")]
    public int NextBiggerScore;
    public int BiggerScore;
    
    
    public bool IsDead;
    private void Func_playBtn()
    {
       
        JumpBtn.gameObject.SetActive(true);
        PlayBtn.gameObject.SetActive(false);
        isPlay = true;
        IsDead = false;
        onPlay.Invoke(isPlay);
        score = 0;
        NextHealScore = 0;
        Scoretext.text = score.ToString();

        StartCoroutine(addScore());
    }

    private void Puse()
    {
        isPlay = false;
        PusePopup.gameObject.SetActive(true);
    }

    private void Option()
    {
        Instantiate(OptionPopUP);
        OptionPopUP.gameObject.SetActive(true);
    }
    
    private void PopUp_Reset()
    {
        LoadSetting();
        Gameover();
        PusePopup.gameObject.SetActive(false);
    }
    private void Popup_Play()
    {
        LoadSetting();
        isPlay = true;
        PusePopup.gameObject.SetActive(false);
        //추후 카운트다운 기능 넣을것

    }
    // Start is called before the first frame update
    void Awake()
    {
        PlayBtn.onClick.AddListener(Func_playBtn);
        PuseBtn.onClick.AddListener(Puse);
        PopUp_playBtn.onClick.AddListener(Popup_Play);
        PopUp_ResetBtn.onClick.AddListener(PopUp_Reset);
        PopUp_OPtionBtn.onClick.AddListener(Option);
        PusePopup.gameObject.SetActive(false);
        LoadSetting();
    }

    private void LoadSetting()
    {
        if (settingsData.ConSmall_toggle)
        {
            JumpBtn.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        }
        else if (settingsData.ConNormal_Toggle)
        {
            JumpBtn.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (settingsData.ConBigBtn_Toggle)
        {
            JumpBtn.gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1);
        }

    }

    IEnumerator addScore()
    {
        while (isPlay)
        {
           // score++;
            Scoretext.text = score.ToString();
            yield return new WaitForSeconds(0.1f);
        }
         
    }

    // Update is called once per frame
   public void Gameover()
    {
        JumpBtn.gameObject.SetActive(false);
        PlayBtn.gameObject.SetActive(true);
        isPlay = false;
        IsDead = true;
        onPlay.Invoke(isPlay);
        StopCoroutine(addScore());
    }

   
}
