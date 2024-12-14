using System;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace DogGuns_Games.vamsir
{
    public class ObjectPool_Spawner : MonoBehaviour
    {
        [SerializeField] public IObjectPool<Vamser_Mob_Base> MOB_objectPool;

        [Header("<color=green>몹 오브젝트")] [SerializeField]
        private int poolSize_MobCount = 20;

        [Header("<color=green>몹 프리팹")] [SerializeField]
        private Vamser_Mob_Base Mob_prefab;

        [Header("<color=green>몹 오브젝트 스폰 위치")] [SerializeField]
        private Transform Mob_Parent;

        private int MobCount = 0;

        private int MobSpawnWave = 0;


        [SerializeField] public IObjectPool<EXP_Obj> EXP_objectPool;

        [Header("<color=green>경험치 오브젝트")] [SerializeField]
        private EXP_Obj Exp_Prefab;

        [SerializeField] private EXP_Obj BigExp_Prefab;

        Camera mainCamera;

        private void Awake()
        {
            MOB_objectPool = new ObjectPool<Vamser_Mob_Base>(Create_Mob,
                OnGet, OnRelease, OnDestory, maxSize: poolSize_MobCount);

            EXP_objectPool = new ObjectPool<EXP_Obj>(Create_EXP,
                OnGet_EXP, OnRelease_EXP, OnDestory_EXP, maxSize: poolSize_MobCount);


            mainCamera = Camera.main;
        }

        private void Start()
        {
            GameStart();
        }

        private void GameStart()
        {
            for (int i = 0; i < poolSize_MobCount; i++)
            {
                MOB_objectPool.Get();
                MobCount++;
            }

            MobSpawnWave = 1;
        }


        private void CheckMob()
        {
            if (MobCount <= 0)
            {
                Invoke(nameof(ReSpawn), 3);
            }
        }

        private void ReSpawn()
        {
            MobSpawnWave++;
            poolSize_MobCount += 5;
            Debug.Log("Wave : " + MobSpawnWave);
            for (int i = 0; i < poolSize_MobCount; i++)
            {
                MOB_objectPool.Get();
                MobCount++;
            }
        }


        #region Mob

        private Vamser_Mob_Base Create_Mob()
        {
            Vamser_Mob_Base mob = Instantiate(Mob_prefab.gameObject, Mob_Parent).GetComponent<Vamser_Mob_Base>();
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
            MobCount--;
            CheckMob();
            // 몹이 죽었을때 경험치 생성
            EXP_Obj exp = EXP_objectPool.Get();
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
                ? Instantiate(BigExp_Prefab.gameObject, Mob_Parent).GetComponent<EXP_Obj>()
                : Instantiate(Exp_Prefab.gameObject, Mob_Parent).GetComponent<EXP_Obj>();
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