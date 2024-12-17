
using System;
using UnityEngine;

namespace DogGuns_Games.vamsir
{
    public class Player_Base : MonoBehaviour
    {
        public float AttackPower { get; set; }
        public float CoolTime{ get; set; }
        public float AttackSpeed { get; set; }
        public float WeaponSize { get; set; }
        public float ProjectileCount{ get; set; }
        public float CriticalChance { get; set; }
        public float CriticalDamage { get; set; }
        public float Health { get; set; }
        public float HealthRegen { get; set; }
        public float Defense  { get; set; }
        public float MoveSpeed { get; set; }
        public float ExpGain { get; set; }
        public float GoldGain { get; set; }
        public float ItemGainRange{ get; set; }
        public float Reroll { get; set; }

        public Vector3 AttackAngle { get; set; }
        
        public int CharacterIndex { get; set; } //현재 캐릭터 인덱스
        
        public Weaphon_base WeaphonBase { get; set; }


        public virtual void OnEnable()
        {
            WeaphonBase = FindFirstObjectByType<Weaphon_base>();
            WeaphonBase.transform.SetParent(transform);
            WeaphonBase.transform.localPosition = Vector3.zero;
        }

        public virtual void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Mob"))
            {
                Player_Hit();
            }
            
            if (other.gameObject.CompareTag("Exp"))
            {
               Debug.Log("Exp");
               
               EXP_Obj expObj = other.gameObject.GetComponent<EXP_Obj>();
               
               expObj.objectPool_Spawner.ExpObjectPool.Release(expObj);
            }
            
        }


        public virtual void Player_attack( Vector3 attackAngle)
        {
            attackAngle = this.AttackAngle;
        }
        public virtual void Player_Die()
        {
        }

        public virtual void Player_Hit()
        {
        }

        public virtual void Player_Idle()
        {
        }
    }
}