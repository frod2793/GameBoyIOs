using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

namespace DogGuns_Games.vamsir
{
    public class Weaphon_Bone : Weaphon_base
    {
        public IObjectPool<Bone_Bullet> objectPool;
        [SerializeField] private int poolSize_BulletCount = 10;

        [SerializeField] private GameObject bonePrefab;
        [SerializeField] private GameObject Bullet_Parent;

        bool isAttacking; // 중복 호출 방지 플래그
        
        public override void OnEnable()
        {
            base.OnEnable();
            //발사체 오브젝트 풀 설정 
            objectPool = new ObjectPool<Bone_Bullet>(Create_Bullet,
                OnGet, OnRelease, OnDestory, maxSize: poolSize_BulletCount);

            Bullet_Parent = GameObject.FindWithTag("WeaponPool");

        }


        private Bone_Bullet Create_Bullet()
        {
            Bone_Bullet bullet = Instantiate(bonePrefab.gameObject, Bullet_Parent.transform)
                .GetComponent<Bone_Bullet>();
            bullet.objectPool_Spawner = this;
            return bullet;
        }

        private void OnGet(Bone_Bullet obj)
        {
            obj.gameObject.SetActive(true);
        }

        private void OnRelease(Bone_Bullet obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void OnDestory(Bone_Bullet obj)
        {
            Destroy(obj.gameObject);
        }


        public override void Weaphon_Idle()
        {
            base.Weaphon_Idle();
        }

        public override void Weaphon_Attack(Vector3 attackAngle)
        {
            base.Weaphon_Attack(attackAngle);
            if (!isAttacking)
            {
                Thorw_Bone(attackAngle).Forget();
                
            }
        }

        public override void Weaphon_Reload()
        {
            base.Weaphon_Reload();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        private async UniTask Thorw_Bone(Vector3 attackAngle)
        {
            isAttacking = true;
            Bone_Bullet bullet = objectPool.Get();
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.Euler(attackAngle);
            bullet.bulletSpeed = attackSpeed;
            bullet.Thow_Bullet(attackAngle);
            await UniTask.Delay(TimeSpan.FromSeconds(coolTime));
            isAttacking = false;
        }
    }
}