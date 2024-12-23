using System;
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


        private void Awake()
        {
            DOTween.SetTweensCapacity(500, 50);
        }

        private void Start()
        {
            Mob_Speed = 0.5f;
            Mob_Hp = 100f;
            Mob_AttackDamage = 10f;
            Mob_AttackSpeed = 1f;
            Mob_AttackRange = 1f;
            Mob_StunTime = 1f;
            Mob_IsDie = false;
        }


        public override void OnEnable()
        {
            base.OnEnable();
            player = FindFirstObjectByType<Player_Base>();
            player_Weaphon = FindFirstObjectByType<Weaphon_base>();

            SetMobState(MobState.Move);
        }


        private void FixedUpdate()
        {
            if (ismove)
            {
                // 플레이어 방향으로 이동 dotween
                // 플레이어 위치에 도달하면 멈춤
                if (player.transform.position == transform.position)
                { 
                    transform.DOKill();
                    return;
                }
                
                if (!Equals(player, null))
                {
                    Vector3 direction = (player.transform.position - transform.position).normalized;
                    float distance = Vector3.Distance(player.transform.position, transform.position);
                    transform.DOMove(transform.position + direction * distance, distance / Mob_Speed);
                }
            }
            else
            {
                transform.DOKill();
            }
        }


        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player_Attack"))
            {
                Debug.Log("Hit");
                //  SetMobState(MobState.Stun);
                Mob_Hp -= player_Weaphon.AttackPower;
                if (Mob_Hp <= 0)
                {
                    SetMobState(MobState.Die);
                }
                else
                {
                    SetMobState(MobState.Stun);
                }
            }
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