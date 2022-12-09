using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSpawnManager : MonoBehaviour
{
    [SerializeField]   
   private RunGameUiManager gameUiManager;



    private void Start()
    {
        for (int i = 0; i < gameUiManager.Mobs.Length; i++)
        {
            for (int j = 0; j < gameUiManager.Objcount; j++)
            {
                gameUiManager.MobPool.Add(Createobj(gameUiManager.Mobs[i], transform));
              
            }
        }


        for (int i = 0; i < gameUiManager.Coins.Length; i++)
        {
            for (int j = 0; j < gameUiManager.Coin_Objcount; j++)
            {
                gameUiManager.CoinPool.Add(Createobj(gameUiManager.Coins[i], transform));
            }
        }

        gameUiManager.onPlay += playgame;

    }



    //private void Start()
    //{
    //    
    //    
    //}

    private void playgame(bool isPlay)
    {
        if (isPlay)
        {
            for (int i = 0; i < gameUiManager.MobPool.Count; i++)
            {
                if (gameUiManager.MobPool[i].activeSelf)
                {
                    gameUiManager.MobPool[i].SetActive(false);
               

                }
               
            }
        StartCoroutine(createMob());
        StartCoroutine(createCoin());
        }
        else
        {
            StopAllCoroutines();
        }
    }


    IEnumerator createMob()
    {
        yield return new WaitForSeconds(0.5f);
        while (gameUiManager.isPlay)
        {
            gameUiManager.MobPool[Deactivemob()].SetActive(true);
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }

    int Deactivemob()
    {
        List<int> num = new List<int>();
        for (int i = 0; i < gameUiManager.MobPool.Count; i++)
        {
            if (!gameUiManager.MobPool[i].activeSelf)
            {
                num.Add(i);
            }
        }
        int x = 0;
        if (num.Count > 0)
        {
            x = num[Random.Range(0, num.Count)];
        }
        return x;

    }



    IEnumerator createCoin()
    {
        yield return new WaitForSeconds(0.1f);


        List<int> num = new List<int>();
     //   int x = 0;
        for (int i = 0; i < gameUiManager.CoinPool.Count; i++)
        {
            if (!gameUiManager.CoinPool[i].activeSelf)
            {
                num.Add(i);
            }
        }
        while (gameUiManager.isPlay)
        {
            if (gameUiManager.isPlay)
            {
                for (int i = 0; i < num.Count - 1; i++)
                {
                    gameUiManager.CoinPool[i].SetActive(true);
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }

       
    }


    GameObject Createobj(GameObject obj,Transform parent)
    {
        GameObject copy = Instantiate(obj);
        copy.transform.SetParent(parent);
        copy.SetActive(false);
        return copy;
    }

}
