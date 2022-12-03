using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunGameUiManager : MonoBehaviour
{
    [Header("플레이어")]
    public Button JumpBtn;
    public float JumpHight;
    public float JumpSpeed;


    [Header("점수 오브젝트 ")]
    public float CoinSpeed;
    public List<GameObject> CoinPool = new List<GameObject>();
    public GameObject[] Coins;
    public int Coin_Objcount;
    public Vector2 C_startpoint;

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

    public int score = 0;


    public bool IsDead;
    private void Func_playBtn()
    {
        JumpBtn.gameObject.SetActive(true);
        PlayBtn.gameObject.SetActive(false);
        isPlay = true;
        IsDead = false;
        onPlay.Invoke(isPlay);
        score = 0;
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
