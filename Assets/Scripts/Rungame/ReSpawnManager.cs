using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSpawnManager : MonoBehaviour
{
    [SerializeField]   
   private RunGameUiManager gameUiManager;



    private void Awake()
    {
        for (int i = 0; i < gameUiManager.Mobs.Length; i++)
        {
            for (int j = 0; j < gameUiManager.Objcount; j++)
            {
                gameUiManager.MobPool.Add(Createobj(gameUiManager.Mobs[i], transform));
                gameUiManager.MobPool[i].transform.position = gameUiManager.Mobs[i].transform.position;
            }
        }
    }



    private void Start()
    {
        gameUiManager.onPlay += playgame;
        
    }

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
    GameObject Createobj(GameObject obj,Transform parent)
    {
        GameObject copy = Instantiate(obj);
        copy.transform.SetParent(parent);
        copy.SetActive(false);
        return copy;
    }

}
