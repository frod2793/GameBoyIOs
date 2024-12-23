using UnityEngine;
using UnityEngine.Serialization;

namespace DogGuns_Games.vamsir
{
    public class Weaphon_base : MonoBehaviour
    {
        
        
        public float AttackPower { get; set; }
        public float CoolTime { get; set; }
        public float AttackSpeed { get; set; }
        public float AttackRange { get; set; }

        public int weaphonIndex;//현재 무기 인덱스 
        
        public enum WeaphonState
        {
            Idle,
            Attack,
            Reload
        }

        [SerializeField] private WeaphonState weaphonState;

        public virtual void OnEnable()
        {
            SetWeaphonState(WeaphonState.Idle);
        }

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                // 플레이 모드에서만 SetWeaphonState 호출
                SetWeaphonState(weaphonState);
            }
        }

        public void SetWeaphonState(WeaphonState state)
        {
            switch (state)
            {
                case WeaphonState.Idle:
                    Weaphon_Idle();
                    break;
                case WeaphonState.Attack:
                    Weaphon_Attack(Vector3.zero);
                    break;
                case WeaphonState.Reload:
                    Weaphon_Reload();
                    break;
            }
        }

        public virtual void Weaphon_Idle()
        {
            // 무기가 대기 상태일 때 처리
        }

        public virtual void Weaphon_Attack(Vector3 attackAngle)
        {
            // 무기가 공격 상태일 때 처리
        }

        public virtual void Weaphon_Reload()
        {
            // 무기가 재장전 상태일 때 처리
        }
    }
}