using System;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace DogGuns_Games.vamsir
{
    public class ObjectPool_Spawner : MonoBehaviour
    {
        [SerializeField] public IObjectPool<Vamser_Mob_Base> MobObjectPool;

        [FormerlySerializedAs("poolSize_MobCount")] [Header("<color=green>몹 오브젝트")] [SerializeField]
        private int poolSizeMobCount = 20;

        [Header("<color=green>몹 프리팹")] [SerializeField]
        private Vamser_Mob_Base mobPrefab;

        [FormerlySerializedAs("Mob_Parent")] [Header("<color=green>몹 오브젝트 스폰 위치")] [SerializeField]
        private Transform mobParent;

        private int _mobCount = 0;

        private int _mobSpawnWave = 0;


        [SerializeField] public IObjectPool<EXP_Obj> ExpObjectPool;

        [Header("<color=green>경험치 오브젝트")] [SerializeField]
        private EXP_Obj expPrefab;

        [SerializeField] private EXP_Obj bigExpPrefab;

        Camera mainCamera;

        private void Awake()
        {
            MobObjectPool = new ObjectPool<Vamser_Mob_Base>(Create_Mob,
                OnGet, OnRelease, OnDestory, maxSize: poolSizeMobCount);

            ExpObjectPool = new ObjectPool<EXP_Obj>(Create_EXP,
                OnGet_EXP, OnRelease_EXP, OnDestory_EXP, maxSize: poolSizeMobCount);


            mainCamera = Camera.main;
        }

        private void Start()
        {
            GameStart();
        }

        private void GameStart()
        {
            for (int i = 0; i < poolSizeMobCount; i++)
            {
                MobObjectPool.Get();
                _mobCount++;
            }

            _mobSpawnWave = 1;
        }


        private void CheckMob()
        {
            if (_mobCount <= 0)
            {
                Invoke(nameof(ReSpawn), 3);
            }
        }

        private void ReSpawn()
        {
            _mobSpawnWave++;
            poolSizeMobCount += 5;
            Debug.Log("Wave : " + _mobSpawnWave);
            for (int i = 0; i < poolSizeMobCount; i++)
            {
                MobObjectPool.Get();
                _mobCount++;
            }
        }


        #region Mob

        private Vamser_Mob_Base Create_Mob()
        {
            Vamser_Mob_Base mob = Instantiate(mobPrefab.gameObject, mobParent).GetComponent<Vamser_Mob_Base>();
            mob.objectPool_Spawner = this;
            return mob;
        }

        private void OnGet(Vamser_Mob_Base obj)
        {
            MoveObjectOffScreen(obj);
        }

        private void OnRelease(Vamser_Mob_Base obj)
        {
            obj.gameObject.SetActive(false);
            _mobCount--;
            CheckMob();
            // 몹이 죽었을때 경험치 생성
            EXP_Obj exp = ExpObjectPool.Get();
            exp.transform.position = obj.transform.position;
            exp.gameObject.SetActive(true);
        }

        private void OnDestory(Vamser_Mob_Base obj)
        {
            Destroy(obj.gameObject);
        }

        #endregion

        private EXP_Obj Create_EXP()
        {
            // 큰 경험치 작은경험치 를 랜덤으로 생성 하는대 큰 경험치는 30%확률로 생성 
            EXP_Obj exp = Random.Range(0, 100) < 30
                ? Instantiate(bigExpPrefab.gameObject, mobParent).GetComponent<EXP_Obj>()
                : Instantiate(expPrefab.gameObject, mobParent).GetComponent<EXP_Obj>();
            exp.objectPool_Spawner = this;
            return exp;
        }

        private void OnGet_EXP(EXP_Obj obj)
        {
            obj.gameObject.SetActive(true);
        }

        private void OnRelease_EXP(EXP_Obj obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void OnDestory_EXP(EXP_Obj obj)
        {
            Destroy(obj.gameObject);
        }


        private void MoveObjectOffScreen(Vamser_Mob_Base obj)
        {
            if (mainCamera != null)
            {
                // 뷰포트 밖의 랜덤 위치 생성
                float x = Random.Range(-2.0f, 2.0f);
                float y = Random.Range(-2.0f, 2.0f);

                // 위치가 뷰포트 밖에 있는지 확인 및 수정
                if (x > 0 && x < 1) x = x < 0.5f ? -0.1f : 1.1f;
                if (y > 0 && y < 1) y = y < 0.5f ? -0.1f : 1.1f;

                // 뷰포트 위치를 월드 위치로 변환
                Vector3 offScreenPosition =
                    mainCamera.ViewportToWorldPoint(new Vector3(x, y, mainCamera.nearClipPlane));

                // 객체의 위치 설정
                obj.transform.position = offScreenPosition;

                // 객체 활성화
                obj.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Main Camera가 설정되지 않았습니다.");
            }
        }
    }
}