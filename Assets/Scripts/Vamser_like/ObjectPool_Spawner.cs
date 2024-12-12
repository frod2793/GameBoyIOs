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
                // 화면 밖 위치 계산
                Vector3 offScreenPosition =
                    mainCamera.ViewportToWorldPoint(new Vector3(1.2f, 1.2f, mainCamera.nearClipPlane));
                obj.transform.position =
                    new Vector3(offScreenPosition.x, offScreenPosition.y, obj.transform.position.z);
            }
        }
    }
}