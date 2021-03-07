using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : ActorManagerInterface
{
    public Collider weaponL;
    public Collider weaponR;

    public GameObject whL;
    public GameObject whR;

    public WeaponController wcL;
    public WeaponController wcR;


    private void Start() {
        whL = transform.DFSforWeapon("LeftWeaponHandel").gameObject;
        whR =  transform.DFSforWeapon("RightWeaponHandel").gameObject;

        wcL = BindWeaponController(whL);
        wcR = BindWeaponController(whR);

        weaponL = whL.GetComponentInChildren<Collider>();
        weaponR = whR.GetComponentInChildren<Collider>();
    }

    private WeaponController BindWeaponController(GameObject obj) {
        WeaponController temp = obj.GetComponent<WeaponController>();
        if (temp == null)
            temp = obj.AddComponent<WeaponController>();
        temp.wm = this;
        return temp;

    }

    public void UpdateWeapon(string side,Collider col) {
        if (side == "R")
            weaponR = col;
        else if (side == "L")
            weaponL = col;
    }

    public void UnloadWeapon(string side) {
        if (side == "R") {
            foreach (Transform weapon in whR.transform)
                Destroy(weapon.gameObject);
        }
            
        else if (side == "L") {
            foreach (Transform weapon in whL.transform)
                Destroy(weapon.gameObject);
        }
    }

    public void AttackEnable() {
        if (cm.inputs.CheckStateByTag("attackR"))
            weaponR.enabled = true;
        else if (cm.inputs.CheckStateByTag("attackL"))
            weaponL.enabled = true;
    }

    public void AttackDisable() {
        weaponL.enabled = false;
        weaponR.enabled = false;
    }

    public void CounterBackEnable() {
        cm.sm.IsCounterBackEnable = true;
        
    }

    public void CounterBackDisable() {
        cm.sm.IsCounterBackEnable = false;
    }

}
