using System;
using DG.Tweening;
using DogGuns_Games.Run;
using UnityEngine;

namespace DogGuns_Games.vamsir
{ 
    public class Normal_Mob : Vamser_Mob_Base
    {
     
        [SerializeField]
        private Player_Base player;

        private bool ismove;

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
            SetMobState(MobState.Move);
            
        }


        private void FixedUpdate()
        {
            if (ismove)
            {
                // 플레이어 방향으로 이동 dotween
                if (!Equals(player,null))
                { 
                   // Debug.Log("Move");
                    Vector3 direction = (player.transform.position - transform.position).normalized;
                    float distance = Vector3.Distance(player.transform.position, transform.position);
                    transform.DOMove(transform.position + direction * distance, distance / Mob_Speed);
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
        }

        protected override void Mob_Attack()
        {
            Debug.Log("Attack");
        }

        protected override void Mob_Die()
        {
            base.Mob_Die();
            Debug.Log("Die");
        }
        
    }
}
