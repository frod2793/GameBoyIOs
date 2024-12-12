using UnityEngine;

namespace DogGuns_Games.vamsir
{


    public class Player_Doggun : Player_Base
    {
        void Start()
        {
            // 부모 클래스의 변수를 오버라이드하여 초기화
            attackPower = 15f;
            coolTime = 0.5f;
            attackSpeed = 1.5f;
            weaponSize = 1.2f;
            projectileCount = 3f;
            criticalChance = 0.2f;
            criticalDamage = 2f;
            health = 150f;
            healthRegen = 1.5f;
            defense = 5f;
            moveSpeed = 7f;
            expGain = 1.2f;
            goldGain = 1.3f;
            itemGainRange = 1.5f;
            reroll = 2f;
        }
        
    }
}