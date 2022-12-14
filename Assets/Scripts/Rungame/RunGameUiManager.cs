using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunGameUiManager : MonoBehaviour
{
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

 

    public Button PlayBtn;
    public Text Scoretext;
    public bool isPlay = false;
    
    public delegate void Onplay(bool isPlay);
    public Onplay onPlay;
    [Header("힐 아이템 목표 점")]
    public int score = 0;
    public int NextHealScore;//목표점수 
    public int HealScore;//더할점수

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
    // Start is called before the first frame update
    void Start()
    {
        PlayBtn.onClick.AddListener(Func_playBtn);
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
        PlayBtn.gameObject.SetActive(true);
        isPlay = false;
        IsDead = true;
        onPlay.Invoke(isPlay);
        StopCoroutine(addScore());
    }

   
}
