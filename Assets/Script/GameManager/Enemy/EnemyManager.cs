using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : CharactorManager
{
    protected override void GetDamage(CharactorManager attacker) {
        if (sm.HP > 0) {
            if (sm.IsStunned) {
                attacker.dm.FrontStun(attacker, this);
                //sm.AddHP(-10);
                if (sm.HP <= 0)
                    Die();
            }
            else {
                sm.AddHP(-10);
                if (sm.HP > 0)
                    Hit();
                else
                    Die();
            }
        }
    }

    public override void LockActorController(bool value) {
        inputs.moveable = !value;
        inputs.SetBool("lock", value);
    }

}
