using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharactorManager
{
    void Start() {
        inputs.OnAction += DoAction;
    }

    private void DoAction() {
        if(im.overlapCasterEMs.Count != 0) {
            foreach(CasterEvent item in im.overlapCasterEMs) {
                if (item.eventName.Equals("frontStab"))
                    dm.FrontStun(this, item.cm);
                else if (item.eventName.Equals("open box")) {
                    if (!item.active) {
                        dm.OpenBox(this, item.transform.parent.gameObject);
                        item.active = true;
                    }
                }
            }
        }
    }


    protected override void GetDamage(CharactorManager attacker) {
        if (sm.HP > 0) {
            if (sm.IsStunned) {
                //attacker.dm.Stun(attacker, this);
                //sm.AddHP(-10);
                if (sm.HP <= 0)
                    Die();
            }
            else {
                sm.AddHP(-5);
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
