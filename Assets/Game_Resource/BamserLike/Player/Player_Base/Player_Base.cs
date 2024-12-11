using UnityEngine;

public class Player_Base : MonoBehaviour
{
    public float attackPower = 10f;
    public float coolTime = 1f;
    public float attackSpeed = 1f;
    public float weaponSize = 1f;
    public float projectileCount = 1f;
    public float criticalChance = 0f;
    public float criticalDamage = 1f;
    public float health = 100f;
    public float healthRegen = 1f;
    public float defense = 0f;
    public float moveSpeed = 5f;
    public float expGain = 1f;
    public float goldGain = 1f;
    public float itemGainRange = 1f;
    public float reroll = 1f;
    
    public virtual void Player_attack()
    {
        
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
