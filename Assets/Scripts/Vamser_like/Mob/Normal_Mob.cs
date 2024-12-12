using DogGuns_Games.Run;
using UnityEngine;

namespace DogGuns_Games.vamsir
{ 
    public class Normal_Mob : Vamser_Mob_Base
    {
        protected override void Mob_Idle()
        {
            Debug.Log("Idle");
        }

        protected override void Mob_Move()
        {
            Debug.Log("Move");
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
