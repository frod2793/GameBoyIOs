using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DogGuns_Games.vamsir
{
    public class Weaphon_StrongBlackWater : Weaphon_base
    {
        private bool isAttacking; // 중복 호출 방지 플래그

        public override void OnEnable()
        {
            base.OnEnable();
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
                isAttacking = true;
                ActiveBlackWater().Forget();
            }
        }
        
        private async UniTask ActiveBlackWater()
        {
            // 플레이어 공격 함수가 호출되는 동안 실행
            float originalAttackPower = AttackPower;
            
            for (int i = 0; i < 10; i++)
            {
                AttackPower = 0;
                await UniTask.Delay(500);
                AttackPower = originalAttackPower;
                await UniTask.Delay(500);
            }

            isAttacking = false; // 공격 완료 후 플래그 리셋
        }
        
        public override void Weaphon_Reload()
        {
            base.Weaphon_Reload();
        }
    }
}