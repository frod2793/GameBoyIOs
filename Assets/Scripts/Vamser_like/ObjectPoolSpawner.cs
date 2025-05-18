using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace DogGuns_Games.vamsir
{
    /// <summary>
    /// 뱀서라이크 게임의 오브젝트 풀 관리 및 스폰 시스템
    /// </summary>
    public class ObjectPoolSpawner : MonoBehaviour
    {
        #region 필드 및 속성

        // 오브젝트 풀 참조
        public IObjectPool<Vamser_Mob_Base> MobObjectPool;
        public IObjectPool<EXP_Obj> ExpObjectPool;
        public IObjectPool<Coin_Obj> CoinObjectPool;
        
        [Header("<color=green>몹 오브젝트</color>")]
        [SerializeField] private int poolSizeMobCount = 20;
        
        [Header("<color=green>몹 프리팹</color>")]
        [SerializeField] private Vamser_Mob_Base mobPrefab;

        [Header("<color=green>몹 오브젝트 스폰 위치</color>")]
        [SerializeField] private Transform mobParent;
        
        // 몹 카운트 관련
        private int _mobCount;
        public int MobCount => _mobCount;
        
        private int _mobSpawnWave;
        public int MobSpawnWave => _mobSpawnWave;
        
        [Header("<color=green>경험치 오브젝트</color>")]
        [SerializeField] private EXP_Obj expPrefab;
        [SerializeField] private EXP_Obj bigExpPrefab;
        
        [Header("<color=green>코인 오브젝트</color>")]
        [SerializeField] private Coin_Obj coinPrefab;
        [SerializeField] private float coinSpawnPercent = 25;
        
        // 스폰 제어 변수
        private bool _isSpawningAllowed = true;
        private Coroutine _spawnCoroutine;
        
        // 기타 참조
        private Camera _mainCamera;
        
        #endregion

        #region 초기화 및 라이프사이클

        /// <summary>
        /// 컴포넌트 초기화 및 오브젝트 풀 생성
        /// </summary>
        private void Awake()
        {
            InitializePools();
            _mainCamera = Camera.main;
        }

        /// <summary>
        /// 오브젝트 풀 초기화
        /// </summary>
        private void InitializePools()
        {
            // 몹 오브젝트 풀 초기화
            MobObjectPool = new ObjectPool<Vamser_Mob_Base>(
                Create_Mob,
                OnGet,
                OnRelease,
                OnDestory,
                maxSize: poolSizeMobCount
            );

            // 경험치 오브젝트 풀 초기화
            ExpObjectPool = new ObjectPool<EXP_Obj>(
                Create_EXP,
                OnGet_EXP,
                OnRelease_EXP,
                OnDestory_EXP,
                maxSize: poolSizeMobCount
            );

            // 코인 오브젝트 풀 초기화
            CoinObjectPool = new LinkedPool<Coin_Obj>(
                CreateCoin,
                OnGet_Coin,
                Onrelease_Coin,
                OnDestory_Coin,
                maxSize: poolSizeMobCount
            );
        }

        /// <summary>
        /// 이벤트 구독 설정
        /// </summary>
        private void Start()
        {
            SubscribeToEvents();
        }

        /// <summary>
        /// 게임 상태 이벤트 구독
        /// </summary>
        private void SubscribeToEvents()
        {
            PlayStateManager.OnGameStart += GameStart;
            PlayStateManager.OnGamePause += Pause;
            PlayStateManager.OnGameResume += Resume;
            PlayStateManager.OnGameOver += GameEnd;
        }

        /// <summary>
        /// 이벤트 구독 해제
        /// </summary>
        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        /// <summary>
        /// 게임 상태 이벤트 구독 해제
        /// </summary>
        private void UnsubscribeFromEvents()
        {
            PlayStateManager.OnGameStart -= GameStart;
            PlayStateManager.OnGamePause -= Pause;
            PlayStateManager.OnGameResume -= Resume;
            PlayStateManager.OnGameOver -= GameEnd;
        }

        #endregion

        #region 게임 상태 관리

        /// <summary>
        /// 게임 시작 시 몹 스폰 초기화
        /// </summary>
        private void GameStart()
        {
            if (PlayStateManager.instance.isPlay)
            {
                // 초기 몹 스폰
                SpawnInitialMobs();
                _mobSpawnWave = 1;
                
                // 스폰 코루틴 시작 (필요시 활성화)
                // _spawnCoroutine = StartCoroutine(SpawnRoutine());
            }
        }

        /// <summary>
        /// 게임 일시정지 처리
        /// </summary>
        private void Pause()
        {
            // 스폰 일시 중지
            _isSpawningAllowed = false;
    
            // 진행 중인 스폰 코루틴 중지
            if (_spawnCoroutine != null)
            {
                StopCoroutine(_spawnCoroutine);
                _spawnCoroutine = null;
            }
    
            Debug.Log("오브젝트 스폰 일시 중지됨");
        }

        /// <summary>
        /// 게임 재개 처리
        /// </summary>
        private void Resume()
        {
            // 스폰 재개
            _isSpawningAllowed = true;
    
            // 필요시 스폰 코루틴 재시작
            if (_spawnCoroutine == null && PlayStateManager.instance.isPlay)
            {
                // _spawnCoroutine = StartCoroutine(SpawnRoutine());
            }
    
            Debug.Log("오브젝트 스폰 재개됨");
        }

        /// <summary>
        /// 게임 종료 시 모든 오브젝트 풀 정리
        /// </summary>
        private void GameEnd()
        {
            // 모든 코루틴 종료
            if (_spawnCoroutine != null)
            {
                StopCoroutine(_spawnCoroutine);
                _spawnCoroutine = null;
            }
            
            // 모든 오브젝트 풀 정리
            MobObjectPool.Clear();
            ExpObjectPool.Clear();
            CoinObjectPool.Clear();
            
            Debug.Log("게임 종료: 모든 오브젝트 풀 정리됨");
        }

        #endregion

        #region 몹 스폰 및 관리

        /// <summary>
        /// 초기 몹 스폰
        /// </summary>
        private void SpawnInitialMobs()
        {
            for (int i = 0; i < poolSizeMobCount; i++)
            {
                if (_isSpawningAllowed)
                {
                    MobObjectPool.Get();
                    _mobCount++;
                }
            }
        }

        /// <summary>
        /// 남은 몹이 있는지 체크하고 없으면 리스폰 예약
        /// </summary>
        private void CheckMob()
        {
            if (_mobCount <= 0)
            {   
                // 몹이 모두 죽었을때 3초후 리스폰
                Invoke(nameof(ReSpawn), 3);
            }
        }

        /// <summary>
        /// 다음 웨이브 몹 리스폰
        /// </summary>
        private void ReSpawn()
        {
            if (!_isSpawningAllowed) return;
            
            _mobSpawnWave++;
            poolSizeMobCount += 5;
            
            Debug.Log($"Wave: {_mobSpawnWave}, 몹 스폰 수: {poolSizeMobCount}");
            
            for (int i = 0; i < poolSizeMobCount; i++)
            {
                if (_isSpawningAllowed)
                {
                    MobObjectPool.Get();
                    _mobCount++;
                }
            }
        }

        /// <summary>
        /// 지정된 몹 위치에 경험치 오브젝트 스폰
        /// </summary>
        private void SpawnExp(Vamser_Mob_Base obj)
        {
            if (!_isSpawningAllowed) return;
            
            EXP_Obj exp = ExpObjectPool.Get();
            exp.transform.position = obj.transform.position;
            exp.gameObject.SetActive(true);
        }

        /// <summary>
        /// 확률에 따라 지정된 몹 위치에 코인 오브젝트 스폰
        /// </summary>
        private void SpawnCoin(Vamser_Mob_Base obj)
        {
            if (!_isSpawningAllowed) return;
            
            if (SpawnRandom(coinSpawnPercent))
            {
                Coin_Obj coin = CoinObjectPool.Get();
                coin.transform.position = obj.transform.position;
                coin.gameObject.SetActive(true);
            }
        }

        #endregion

        #region 오브젝트 풀 - 몹

        /// <summary>
        /// 몹 오브젝트 생성
        /// </summary>
        private Vamser_Mob_Base Create_Mob()
        {
            Vamser_Mob_Base mob = Instantiate(mobPrefab.gameObject, mobParent).GetComponent<Vamser_Mob_Base>();
            mob.objectPool_Spawner = this;
            return mob;
        }

        /// <summary>
        /// 몹 오브젝트 풀에서 가져올 때 처리
        /// </summary>
        private void OnGet(Vamser_Mob_Base obj)
        {
            MoveObjectOffScreen(obj);
        }

        /// <summary>
        /// 몹 오브젝트를 풀에 반환할 때 처리
        /// </summary>
        private void OnRelease(Vamser_Mob_Base obj)
        {
            obj.gameObject.SetActive(false);
            _mobCount--;
            
            // 남은 몹 수 체크
            CheckMob();
            
            // 몹이 죽었을 때 아이템 생성
            SpawnExp(obj);
            SpawnCoin(obj);
        }

        /// <summary>
        /// 몹 오브젝트 파괴 시 처리
        /// </summary>
        private void OnDestory(Vamser_Mob_Base obj)
        {
            Destroy(obj.gameObject);
        }

        #endregion

        #region 오브젝트 풀 - 경험치

        /// <summary>
        /// 경험치 오브젝트 생성
        /// </summary>
        private EXP_Obj Create_EXP()
        {
            // TODO: 큰 경험치는 일정 웨이브 이후 또는 특정 조건에서 생성
            if (_mobSpawnWave >= 5 && Random.value > 0.9f && bigExpPrefab != null)
            {
                return CreateObject(bigExpPrefab);
            }
            
            return CreateObject(expPrefab);
        }

        /// <summary>
        /// 경험치 오브젝트 풀에서 가져올 때 처리
        /// </summary>
        private void OnGet_EXP(EXP_Obj obj)
        {
            OnGetObject(obj);
        }

        /// <summary>
        /// 경험치 오브젝트를 풀에 반환할 때 처리
        /// </summary>
        private void OnRelease_EXP(EXP_Obj obj)
        {
            OnReleaseObject(obj);
        }

        /// <summary>
        /// 경험치 오브젝트 파괴 시 처리
        /// </summary>
        private void OnDestory_EXP(EXP_Obj obj)
        {
            OnDestroyObject(obj);
        }

        #endregion

        #region 오브젝트 풀 - 코인

        /// <summary>
        /// 코인 오브젝트 생성
        /// </summary>
        private Coin_Obj CreateCoin()
        {
            return CreateObject(coinPrefab);
        }

        /// <summary>
        /// 코인 오브젝트 풀에서 가져올 때 처리
        /// </summary>
        private void OnGet_Coin(Coin_Obj obj)
        {
            OnGetObject(obj);
        }

        /// <summary>
        /// 코인 오브젝트를 풀에 반환할 때 처리
        /// </summary>
        private void Onrelease_Coin(Coin_Obj obj)
        {
            OnReleaseObject(obj);
        }

        /// <summary>
        /// 코인 오브젝트 파괴 시 처리
        /// </summary>
        private void OnDestory_Coin(Coin_Obj obj)
        {
            OnDestroyObject(obj);
        }

        #endregion

        #region 공통 유틸리티

        /// <summary>
        /// 제네릭 오브젝트 생성 메서드
        /// </summary>
        private T CreateObject<T>(T prefab) where T : MonoBehaviour
        {
            T obj = Instantiate(prefab.gameObject, mobParent).GetComponent<T>();
            
            // 오브젝트 타입에 따라 추가 설정
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

        /// <summary>
        /// 오브젝트 활성화 공통 처리
        /// </summary>
        private void OnGetObject<T>(T obj) where T : MonoBehaviour
        {
            obj.gameObject.SetActive(true);
        }

        /// <summary>
        /// 오브젝트 비활성화 공통 처리
        /// </summary>
        private void OnReleaseObject<T>(T obj) where T : MonoBehaviour
        {
            obj.gameObject.SetActive(false);
        }

        /// <summary>
        /// 오브젝트 파괴 공통 처리
        /// </summary>
        private void OnDestroyObject<T>(T obj) where T : MonoBehaviour
        {
            Destroy(obj.gameObject);
        }

        /// <summary>
        /// 오브젝트를 화면 밖에 위치시키는 메서드
        /// </summary>
        private void MoveObjectOffScreen(Vamser_Mob_Base obj)
        {
            if (_mainCamera == null)
            {
                Debug.LogWarning("Main Camera가 설정되지 않았습니다.");
                return;
            }
            
            // 뷰포트 밖의 랜덤 위치 생성
            float x = Random.Range(-2.0f, 2.0f);
            float y = Random.Range(-2.0f, 2.0f);

            // 위치가 뷰포트 내부인 경우 강제로 외부로 이동
            if (x > 0 && x < 1) x = x < 0.5f ? -0.1f : 1.1f;
            if (y > 0 && y < 1) y = y < 0.5f ? -0.1f : 1.1f;

            // 뷰포트 위치를 월드 위치로 변환
            Vector3 offScreenPosition = _mainCamera.ViewportToWorldPoint(
                new Vector3(x, y, _mainCamera.nearClipPlane)
            );

            // 객체 위치 설정 및 활성화
            obj.transform.position = offScreenPosition;
            obj.gameObject.SetActive(true);
        }

        /// <summary>
        /// 확률에 따른 스폰 여부 결정
        /// </summary>
        private bool SpawnRandom(float percent)
        {
            return Random.Range(0, 100) < percent;
        }
        
        
        #endregion
    }
}