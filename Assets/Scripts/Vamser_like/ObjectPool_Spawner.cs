using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace DogGuns_Games.vamsir
{
    public class ObjectPool_Spawner : MonoBehaviour
    {
        public IObjectPool<Vamser_Mob_Base> MobObjectPool;

        [Header("<color=green>몹 오브젝트")] [SerializeField]
        private int poolSizeMobCount = 20;

        [Header("<color=green>몹 프리팹")] [SerializeField]
        private Vamser_Mob_Base mobPrefab;

        [Header("<color=green>몹 오브젝트 스폰 위치")] [SerializeField]
        private Transform mobParent;

        private int _mobCount;
        public int MobCount => _mobCount;

        private int _mobSpawnWave;
        public int MobSpawnWave => _mobSpawnWave;

        public IObjectPool<EXP_Obj> ExpObjectPool;
        public IObjectPool<Coin_Obj> CoinObjectPool;

        [Header("<color=green>경험치 오브젝트")] [SerializeField]
        private EXP_Obj expPrefab;

        [SerializeField] private EXP_Obj bigExpPrefab;

        [Header("<color=green>코인 오브잭트")] [SerializeField]
        private Coin_Obj coinPrefab;

        Camera _mainCamera;

        private void Awake()
        {
            MobObjectPool = new ObjectPool<Vamser_Mob_Base>(Create_Mob,
                OnGet, OnRelease, OnDestory, maxSize: poolSizeMobCount);

            ExpObjectPool = new ObjectPool<EXP_Obj>(Create_EXP,
                OnGet_EXP, OnRelease_EXP, OnDestory_EXP, maxSize: poolSizeMobCount);

            CoinObjectPool = new LinkedPool<Coin_Obj>(CreateCoin, OnGet_Coin, Onrelease_Coin, OnDestory_Coin,
                maxSize: poolSizeMobCount);
            _mainCamera = Camera.main;
        }


        private void Start()
        {
            Play_State.OnGameStart += GameStart;
        }

        private void GameStart()
        {
            if (Play_State.instance.isPlay)
            {
                for (int i = 0; i < poolSizeMobCount; i++)
                {
                    MobObjectPool.Get();
                    _mobCount++;
                }

                _mobSpawnWave = 1;
            }
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

            // 몹이 죽었을때 코인 생성
            Coin_Obj coin = CoinObjectPool.Get();
            coin.transform.position = obj.transform.position;
            coin.gameObject.SetActive(true);
        }

        private void OnDestory(Vamser_Mob_Base obj)
        {
            Destroy(obj.gameObject);
        }

        #endregion

        #region EXP

        private EXP_Obj Create_EXP()
        {
            return CreateObject(Random.Range(0, 100) < 30 ? bigExpPrefab : expPrefab);
        }

        private void OnGet_EXP(EXP_Obj obj)
        {
            OnGetObject(obj);
        }

        private void OnRelease_EXP(EXP_Obj obj)
        {
            OnReleaseObject(obj);
        }

        private void OnDestory_EXP(EXP_Obj obj)
        {
            OnDestroyObject(obj);
        }

        #endregion

        #region Coin

        private Coin_Obj CreateCoin()
        {
            return CreateObject(coinPrefab);
        }

        private void OnGet_Coin(Coin_Obj obj)
        {
            OnGetObject(obj);
        }

        private void Onrelease_Coin(Coin_Obj obj)
        {
            OnReleaseObject(obj);
        }

        private void OnDestory_Coin(Coin_Obj obj)
        {
            OnDestroyObject(obj);
        }

        #endregion

        private T CreateObject<T>(T prefab) where T : MonoBehaviour
        {
            T obj = Instantiate(prefab.gameObject, mobParent).GetComponent<T>();
            if (obj is EXP_Obj expObj)
            {
                expObj.objectPoolSpawner = this;
            }
            else if (obj is Coin_Obj coinObj)
            {
                coinObj.objectPoolSpawner = this;
            }

            return obj;
        }

        private void OnGetObject<T>(T obj) where T : MonoBehaviour
        {
            obj.gameObject.SetActive(true);
        }

        private void OnReleaseObject<T>(T obj) where T : MonoBehaviour
        {
            obj.gameObject.SetActive(false);
        }

        private void OnDestroyObject<T>(T obj) where T : MonoBehaviour
        {
            Destroy(obj.gameObject);
        }

        private void MoveObjectOffScreen(Vamser_Mob_Base obj)
        {
            if (_mainCamera != null)
            {
                // 뷰포트 밖의 랜덤 위치 생성
                float x = Random.Range(-2.0f, 2.0f);
                float y = Random.Range(-2.0f, 2.0f);

                // 위치가 뷰포트 밖에 있는지 확인 및 수정
                if (x > 0 && x < 1) x = x < 0.5f ? -0.1f : 1.1f;
                if (y > 0 && y < 1) y = y < 0.5f ? -0.1f : 1.1f;

                // 뷰포트 위치를 월드 위치로 변환
                Vector3 offScreenPosition =
                    _mainCamera.ViewportToWorldPoint(new Vector3(x, y, _mainCamera.nearClipPlane));

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