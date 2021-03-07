using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharactorManager : MonoBehaviour{
    public CharactorInput inputs;

    public BattleManager bm;
    public WeaponManager wm;
    public StateManager sm;
    public DiretorManager dm;
    public InteractionManager im;

    void Awake() {
        inputs = GetComponent<CharactorInput>();
        bm = Bind<BattleManager>(gameObject);
        wm = Bind<WeaponManager>(inputs.model);
        sm = Bind<StateManager>(gameObject);
        dm = Bind<DiretorManager>(gameObject);
        im = Bind<InteractionManager>(transform.Find("sensor").gameObject);
    }

    private T Bind<T>(GameObject obj) where T : ActorManagerInterface {
        T temp;
        temp = obj.GetComponent<T>();
        if (temp == null)
            temp = obj.AddComponent<T>();
        temp.cm = this;
        return temp;
    }

    public void TryDamage(CharactorManager attacker, bool attackValid, bool counterValid) {

        if (sm.IsImmortal) {
            //do nothing
        }
        else if (sm.IsCounterBack) {
            if (sm.IsCounterBackEnable) {
                if (counterValid)
                    attacker.Stunned();
            }
            else {
                if (attackValid)
                    GetDamage(attacker);
            }
        }
        else if (sm.IsDefense && attackValid) {
            Block();
        }
        else {
            if (attackValid && !inputs.CheckState("hited"))
                GetDamage(attacker);
        }
    }
    protected abstract void GetDamage(CharactorManager attacker);
    public abstract void LockActorController(bool value);

    public void Stunned() {
        inputs.SetTrigger("stunned");
    }

    public void Block() {
        inputs.SetTrigger("block");
    }

    public void Hit() {
        inputs.SetTrigger("hit");

    }

    public void Die() {
        inputs.SetTrigger("die");
        //ac.pi.moveEnable = false;
        //if (pc.camcon.lockState) {
        //    pc.camcon.Lockon();
        //}
    }

    //public void LockActorController(bool value) {
    //    //pc.SetBool("lock", value);
    //    //pc.LockMovement();
    //    //pc.Stop();
    //    //pc.model.SendMessage("AttackDisable");
    //}
}
