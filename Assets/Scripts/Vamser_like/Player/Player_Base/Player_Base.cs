
using System;
using UnityEngine;

namespace DogGuns_Games.vamsir
{
    public class Player_Base : MonoBehaviour
    {
        public float attackPower { get; set; }
        public float coolTime{ get; set; }
        public float attackSpeed { get; set; }
        public float weaponSize { get; set; }
        public float projectileCount{ get; set; }
        public float criticalChance { get; set; }
        public float criticalDamage { get; set; }
        public float health { get; set; }
        public float healthRegen { get; set; }
        public float defense  { get; set; }
        public float moveSpeed { get; set; }
        public float expGain { get; set; }
        public float goldGain { get; set; }
        public float itemGainRange{ get; set; }
        public float reroll { get; set; }

        public Vector3 attackAngle { get; set; }
        
        public int CharacterIndex { get; set; } //현재 캐릭터 인덱스
        
        public Weaphon_base weaphonBase { get; set; }


        public virtual void OnEnable()
        {
            weaphonBase = FindFirstObjectByType<Weaphon_base>();
            weaphonBase.transform.SetParent(transform);
            weaphonBase.transform.localPosition = Vector3.zero;
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
               
               expObj.objectPool_Spawner.EXP_objectPool.Release(expObj);
            }
            
        }


        public virtual void Player_attack( Vector3 attackAngle)
        {
            attackAngle = this.attackAngle;
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