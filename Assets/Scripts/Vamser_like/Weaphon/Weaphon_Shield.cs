using DG.Tweening;
using UnityEngine;

namespace DogGuns_Games.vamsir
{
    public class Weaphon_Shield : Weaphon_base
    {
        
        [Header("<color=green> 방패 아이템 관련 변수")]
        [SerializeField]private GameObject shield;
        [SerializeField] private Collider2D shieldCollider;
        
        bool _isAnimShield; // 중복 호출 방지 플래그
        
        public override void OnEnable()
        {
            base.OnEnable();
            shieldCollider.enabled = false;
        }

        public override void Weaphon_Idle()
        {
            base.Weaphon_Idle();
        }
        public override void Weaphon_Reload()
        {
            base.Weaphon_Reload();
        }

        public override void Weaphon_Attack(Vector3 attackAngle)
        {
            base.Weaphon_Attack(attackAngle);
            Shieldmove_anime();
        }

        private void Shieldmove_anime()
        {
            if (_isAnimShield)
            {
                return;
            }
            _isAnimShield = true;
            shield.transform.DOLocalMove(new Vector3(0, 0, 0), 0.5f)
                .SetEase(Ease.OutBounce)
                .From(new Vector3(0, 1, 0))
                .OnUpdate(() =>
                {
                    shieldCollider.enabled = shield.transform.localPosition == Vector3.zero;
                })
                .OnComplete(() => 
                {
                    _isAnimShield = false;
                });
        }
    }
}