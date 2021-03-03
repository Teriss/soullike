using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour
{
    public bool isItem;

    public PlayerControl pc;

    public BattleManager bm;
    public WeaponManager wm;
    public StateManager sm;
    public DiretorManager dm;
    public InteractionManager im;

    // Start is called before the first frame update
    void Awake()
    {
        pc = GetComponent<PlayerControl>();
        bm = Bind<BattleManager>(transform.Find("sensor").gameObject);
        wm = Bind<WeaponManager>(pc.model);
        sm = Bind<StateManager>(gameObject);
        dm = Bind<DiretorManager>(gameObject);
        im = Bind<InteractionManager>(transform.Find("sensor").gameObject);
        pc.OnAction += DoAction;
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
            if(attackValid && !pc.CheckState("hited"))
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
        pc.SetTrigger("stunned");
    }

    public void Block() {
        pc.SetTrigger("block");
    }

    public void Hit() {
        pc.SetTrigger("hit");

    }

    public void Die() {
        pc.SetTrigger("die");
        //ac.pi.moveEnable = false;
        //if (pc.camcon.lockState) {
        //    pc.camcon.Lockon();
        //}
    }

    public void LockActorController(bool value) {
        pc.SetBool("lock", value);
        pc.LockMovement();
        pc.Stop();
        pc.model.SendMessage("AttackDisable");
    }

}
