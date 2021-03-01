using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour
{
    public bool isItem;

    public ActorController ac;

    public BattleManager bm;
    public WeaponManager wm;
    public StateManager sm;
    public DiretorManager dm;
    public InteractionManager im;

    // Start is called before the first frame update
    void Awake()
    {
        ac = GetComponent<ActorController>();
        bm = Bind<BattleManager>(transform.Find("sensor").gameObject);
        wm = Bind<WeaponManager>(ac.model);
        sm = Bind<StateManager>(gameObject);
        dm = Bind<DiretorManager>(gameObject);
        im = Bind<InteractionManager>(transform.Find("sensor").gameObject);
        ac.OnAction += DoAction;
    }

    private void DoAction() {
        if(im.overlapCasterEMs.Count != 0) {
            foreach(CasterEventManager item in im.overlapCasterEMs) {
                if (item.eventName.Equals("frontStab"))
                    dm.FrontStun(this, item.am);
                else if (item.eventName.Equals("open box")) {
                    if (!item.active) {
                        dm.OpenBox(this, item.itemm);
                        item.active = true;
                    }
                }
            }
        }
    }

    private T Bind<T>(GameObject obj) where T: ActorManagerInterface{
        T temp;
        temp = obj.GetComponent<T>();
        if (temp == null)
            temp = obj.AddComponent<T>();
        temp.am = this;
        return temp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TryDamage(WeaponController attacker,bool attackValid,bool counterValid) {

        if (sm.IsImmortal) {
            //do nothing
        }
        else if (sm.IsCounterBack) {
            if (sm.IsCounterBackEnable) {
                if (counterValid)
                    attacker.wm.am.Stunned();
            }
            else {
                if (attackValid)
                    GetDamage(attacker.wm.am);
            }
        }
        else if (sm.IsDefense && attackValid) {
            Block();
        }
        else {
            if(attackValid && !ac.CheckState("hited"))
                GetDamage(attacker.wm.am);
        }
    }

    private void GetDamage(ActorManager attacker) {
        if (sm.HP > 0) {
            if (sm.IsStunned) {
                //attacker.dm.Stun(attacker, this);
                //sm.AddHP(-10);
                if (sm.HP <= 0)
                    Die();
            }
            else {
                sm.AddHP(0);
                if (sm.HP > 0)
                    Hit();
                else
                    Die();
            }

        }
    }

    private void Stunned() {
        ac.SetTrigger("stunned");
    }

    public void Block() {
        ac.SetTrigger("block");
    }

    public void Hit() {
        ac.SetTrigger("hit");

    }

    public void Die() {
        ac.SetTrigger("die");
        //ac.pi.moveEnable = false;
        if (ac.camcon.lockState) {
            ac.camcon.Lockon();
        }
    }

    public void LockActorController(bool value) {
        ac.SetBool("lock", value);
        ac.pi.moveEnable = !value;
        ac.Stop();
        ac.model.SendMessage("AttackDisable");
    }

}
