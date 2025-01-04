
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace DogGuns_Games.vamsir
{
    public class Normal_Mob : Vamser_Mob_Base
    {
        [Header("<color=green>플레이여")] [SerializeField]
        private Player_Base player;

        [Header("<color=green>플레이어 무기")] [SerializeField]
        private Weaphon_base player_Weaphon;

        //피격 물체가 발사체인지 구분
        private bool _isHitByShoot;

        private void Awake()
        {
            DOTween.SetTweensCapacity(500, 50);
        }

        private void Init()
        {
            player = FindFirstObjectByType<Player_Base>();
            player_Weaphon = FindFirstObjectByType<Weaphon_base>();
            Mob_Speed = 0.5f;
            Mob_Hp = 100f;
            Mob_AttackDamage = 10f;
            Mob_AttackSpeed = 1f;
            Mob_AttackRange = 1f;
            Mob_IsDie = false;
            Mob_IsHit = false;
            _isHitByShoot = player_Weaphon.isShooting;
        }


        public override void OnEnable()
        {
            base.OnEnable();

            Init();

            SetMobState(MobState.Move);
        }


        private void FixedUpdate()
        {
            if (!ismove)
            {
                transform.DOKill();
                return;
            }

            // 플레이어가 없으면 찾아서 대입 
            player ??= FindFirstObjectByType<Player_Base>();
            
            if (player == null)
            {
                return;
            }

            // 플레이어 방향으로 이동 dotween
            // 플레이어 위치에 도달하면 멈춤
            Vector3 direction = (player.transform.position - transform.position).normalized;
            float distance = Vector3.Distance(player.transform.position, transform.position);

        
            //플레이어와의 거리가 5이하면 이동하지 않음
            if (distance < 0.3f)
            {
                transform.DOKill();
                return;
            }

            transform.DOMove(transform.position + direction * distance, distance / Mob_Speed);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (_isHitByShoot && !Mob_IsHit && other.gameObject.CompareTag("Player_Attack"))
            {
                HitCooltime(other).Forget();
            }
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (!_isHitByShoot && !Mob_IsHit && other.gameObject.CompareTag("Player_Attack"))
            {
                HitCooltime(other).Forget();
            }
        }

        private async UniTask HitCooltime(Collision2D other)
        {
            Mob_IsHit = true;
            if (player_Weaphon == null)
            {
                player_Weaphon = FindFirstObjectByType<Weaphon_base>();
            }

            Debug.Log("<color=green>Hit: " + player_Weaphon.attackPower);
            Mob_Hp -= player_Weaphon.attackPower;
            Mob_StunTime = player_Weaphon.mobStunTime;
            if (Mob_Hp <= 0)
            {
                SetMobState(MobState.Die);
            }
            else
            {
                SetMobState(MobState.Stun);
            }

            await UniTask.NextFrame();
            Mob_IsHit = false;
        }


        protected override void Mob_Idle()
        {
            Debug.Log("Idle");
        }

        protected override void Mob_Move()
        {
            ismove = true;
        }

        protected override void Mob_Stun()
        {
            Debug.Log("Stun");
            //3초간 스턴

            ismove = false;

            DOVirtual.DelayedCall(Mob_StunTime, () => { SetMobState(MobState.Move); });
        }

        protected override void Mob_hit()
        {
            base.Mob_hit();
        }

        protected override void Mob_Attack()
        {
            Debug.Log("Attack");
        }

        protected override void Mob_Die()
        {
            base.Mob_Die();
            transform.DOKill();
            Debug.Log("Die");
        }
    }
}