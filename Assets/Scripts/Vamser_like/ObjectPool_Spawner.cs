using UnityEngine;
using UnityEngine.Pool;

namespace DogGuns_Games.vamsir
{
    public class ObjectPool_Spawner : MonoBehaviour
    {
        [SerializeField] public IObjectPool<Vamser_Mob_Base> objectPool;
        [SerializeField] private int poolSize_MobCount = 10;
        [SerializeField] private Vamser_Mob_Base Mob_prefab;
        [SerializeField] private Transform Mob_Parent;
        Camera mainCamera;

        private void Awake()
        {
            objectPool = new ObjectPool<Vamser_Mob_Base>(Create_Mob,
                OnGet, OnRelease, OnDestory, maxSize: poolSize_MobCount);

            mainCamera = Camera.main;
        }

        private void Start()
        {
            for (int i = 0; i < poolSize_MobCount; i++)
            {
                objectPool.Get();
            }
        }

        private Vamser_Mob_Base Create_Mob()
        {
            Vamser_Mob_Base mob = Instantiate(Mob_prefab.gameObject, Mob_Parent).GetComponent<Vamser_Mob_Base>();
            mob.objectPool_Spawner = this;
            return mob;
        }

        private void OnGet(Vamser_Mob_Base obj)
        {
            obj.gameObject.SetActive(true);
            MoveObjectOffScreen(obj);
            //todo : mob 스폰을 카메라 밖 랜덤으로
        }

        private void OnRelease(Vamser_Mob_Base obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void OnDestory(Vamser_Mob_Base obj)
        {
            Destroy(obj.gameObject);
        }

        private void MoveObjectOffScreen(Vamser_Mob_Base obj)
        {
            if (mainCamera != null)
            {
                // // 뷰포트 크기 가져오기
                // float viewportWidth = mainCamera.pixelWidth;
                // float viewportHeight = mainCamera.pixelHeight;

                // 뷰포트 밖의 랜덤 위치 생성
                float x = Random.Range(-1.0f, 2.0f);
                float y = Random.Range(-1.0f, 2.0f);

                // 위치가 뷰포트 밖에 있는지 확인
                if (x > 0 && x < 1) x = x < 0.5f ? -0.1f : 1.1f;
                if (y > 0 && y < 1) y = y < 0.5f ? -0.1f : 1.1f;

                // 뷰포트 위치를 월드 위치로 변환
                Vector3 offScreenPosition =
                    mainCamera.ViewportToWorldPoint(new Vector3(x, y, mainCamera.nearClipPlane));

                // 객체의 위치 설정
                obj.transform.position = offScreenPosition;
            }
        }
    }
}