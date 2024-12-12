using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DogGuns_Games.Run
{
    public class ReSpawnManager : MonoBehaviour
    {
        [SerializeField] private RunGameUiManager gameUiManager;
        bool isHeal;

        private void Start()
        {
            for (int i = 0; i < gameUiManager.Mobs.Length; i++)
            {
                for (int j = 0; j < gameUiManager.Objcount; j++)
                {
                    gameUiManager.MobPool.Add(Createobj(gameUiManager.Mobs[i], transform));
                }
            }

            gameUiManager.Healpool.Add(Createobj(gameUiManager.HealObj, transform));
            gameUiManager.Biggerpool.Add(Createobj(gameUiManager.Biggerobj, transform));
            for (int i = 0; i < gameUiManager.Coins.Length; i++)
            {
                for (int j = 0; j < gameUiManager.Coin_Objcount; j++)
                {
                    gameUiManager.CoinPool.Add(Createobj(gameUiManager.Coins[i], transform));
                }
            }

            gameUiManager.OnPlay += playgame;
        }

        private void FixedUpdate()
        {
            if (gameUiManager.isPlay)
            {
                if (gameUiManager.Hpbar.value < gameUiManager.PointHp)
                {
                    StartCoroutine(CreateHealObj());
                }
            }
        }

        private void playgame(bool isPlay)
        {
            if (isPlay)
            {
                //  gameUiManager.Healpool[0].SetActive(true);
                StartCoroutine(createMob());
                StartCoroutine(CreateCoin());
                StartCoroutine(CreateHealObj());
                StartCoroutine(CreateBiggerObj());
            }
            else
            {
                StopAllCoroutines();
            }
        }

        IEnumerator createMob()
        {
            yield return new WaitForSeconds(0.5f);
            while (true)
            {
                if (!gameUiManager.isPlay)
                {
                    yield return new WaitForSeconds(0.1f);
                }
                else
                {
                    gameUiManager.MobPool[Deactivemob()].SetActive(true);
                    yield return new WaitForSeconds(Random.Range(2f, 4f));
                }
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


        IEnumerator CreateCoin()
        {
            yield return new WaitForSeconds(0.1f);

            while (true)
            {
                if (!gameUiManager.isPlay)
                {
                    // 게임이 멈춘 상태라면 0.1초 대기
                    yield return new WaitForSeconds(0.1f);
                    continue;
                }

                // 게임이 진행 중일 경우
                for (int i = 0; i < gameUiManager.CoinPool.Count; i++)
                {
                    // 비활성화된 코인만 활성화
                    if (!gameUiManager.CoinPool[i].activeSelf)
                    {
                        gameUiManager.CoinPool[i].SetActive(true);
                        yield return new WaitForSeconds(0.1f); // 활성화 후 대기
                    }
                }

                yield return null; // 다음 프레임까지 대기
            }
        }


        IEnumerator CreateHealObj()
        {
            while (gameUiManager.isPlay)
            {
                if (gameUiManager.currenthp < gameUiManager.PointHp && !isHeal &&
                    gameUiManager.score > gameUiManager.nextHealScore)
                {
                    gameUiManager.Healpool[0].SetActive(true);
                    gameUiManager.nextHealScore = gameUiManager.score;
                    gameUiManager.nextHealScore += gameUiManager.healScore;
                    isHeal = true;
                    yield return new WaitForSeconds(0.1f);
                }
                else
                {
                    yield return new WaitForSeconds(0.1f);
                }
            }

            if (gameUiManager.currenthp > gameUiManager.PointHp)
            {
                isHeal = false;
            }
        }


        IEnumerator CreateBiggerObj()
        {
            while (gameUiManager.isPlay)
            {
                if (gameUiManager.score < gameUiManager.nextBiggerScore)
                {
                    yield return new WaitForSeconds(0.1f);
                }
                else
                {
                    gameUiManager.Biggerpool[0].SetActive(true);
                    print("Bigger");
                    gameUiManager.nextBiggerScore = gameUiManager.score;
                    gameUiManager.nextBiggerScore += gameUiManager.biggerScore;

                    yield return new WaitForSeconds(0.1f);
                }
            }
        }

        GameObject Createobj(GameObject obj, Transform parent)
        {
            GameObject copy = Instantiate(obj, parent);
            copy.SetActive(false);
            return copy;
        }
    }
}