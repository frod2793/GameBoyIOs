using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DogGuns_Games.vamsir
{
    public class Weaphon_StrongBlackWater : Weaphon_base
    {
        private bool _isAttacking; // 중복 호출 방지 플래그
        private Collider2D _collider2D; 

        public override void OnEnable()
        {
            base.OnEnable();
            mobStunTime = 0.5f;
        }

        public override void Weaphon_Idle()
        {
            base.Weaphon_Idle();
        }

        public override void Weaphon_Attack(Vector3 attackAngle)
        {
            if (_collider2D == null)
            {
                _collider2D = GetComponent<Collider2D>();
            }
               
            base.Weaphon_Attack(attackAngle);

            if (!_isAttacking)
            {
                _isAttacking = true;
                ActiveBlackWater().Forget();
            }
        }

        private async UniTask ActiveBlackWater()
        {
            // 플레이어 공격 함수가 호출되는 동안 실행
            float originalAttackPower = attackPower; // 원래 공격력을 저장
  
            await UniTask.Delay(TimeSpan.FromSeconds(coolTime)); // coolTime 동안 대기
            _collider2D.enabled = true;   
            attackPower = originalAttackPower; // 원래 공격력으로 복원
            await UniTask.Delay(TimeSpan.FromSeconds(coolTime)); // coolTime 동안 대기
            _collider2D.enabled = false;
            Debug.Log("BlackWater Attack: " + attackPower);
            _isAttacking = false; // 공격 완료 후 플래그 리셋
        }

        public override void Weaphon_Reload()
        {
            base.Weaphon_Reload();
        }
    }
}