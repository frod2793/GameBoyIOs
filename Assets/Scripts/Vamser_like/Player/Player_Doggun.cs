using DG.Tweening;
using UnityEngine;

namespace DogGuns_Games.vamsir
{


    public class Player_Doggun : Player_Base
    {
        
        SpriteRenderer playerSpriteRenderer;
        
        void Init()
        {
            // 부모 클래스의 변수를 오버라이드하여 초기화
            AttackPower = 15f;
            CoolTime = 0.5f;
            AttackSpeed = 1.5f;
            WeaponSize = 1.2f;
            ProjectileCount = 3f;
            CriticalChance = 0.2f;
            CriticalDamage = 2f;
            Health = 150f;
            HealthRegen = 1.5f;
            Defense = 5f;
            MoveSpeed = 7f;
            ExpGain = 1.2f;
            GoldGain = 1.3f;
            ItemGainRange = 1.5f;
            Reroll = 2f;
        }

        public override void OnEnable()
        {
            base.OnEnable();
            Init();
        }

        public override void OnCollisionStay2D(Collision2D other)
        {
            base.OnCollisionStay2D(other);
        }

        public override void Player_attack(Vector3 attackAngle)
        {
            base.Player_attack( attackAngle);
            
            WeaphonBase.Weaphon_Attack(attackAngle);
         //   Debug.Log("Player_attack : " + AttackAngle);
        }

        public override void Player_Die()
        {
            base.Player_Die();
        }

        public override void Player_Hit()
        {
            base.Player_Hit();
            HitEffect();
        }

        public override void Player_Idle()
        {
            base.Player_Idle();
        }
        
        private void HitEffect()
        {
            if (playerSpriteRenderer == null)
            {
                playerSpriteRenderer = GetComponent<SpriteRenderer>();
            }

            Color originalColor = Color.white;
            playerSpriteRenderer.DOColor(Color.red, 0.1f).OnComplete(() =>
            {
                playerSpriteRenderer.DOColor(originalColor, 0.1f);
            });
        }
        
    }
}