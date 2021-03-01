using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFactory
{
    private GameDatabsase weaponDB;

    public WeaponFactory(GameDatabsase DB) {
        weaponDB = DB;
    }

    public GameObject CreateWeapon(string weaponID,Vector3 pos,Quaternion rot) {
        GameObject prefab = Resources.Load("Weapon/"+weaponID+"/" + weaponID) as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, pos, rot);

        WeaponData data = obj.AddComponent<WeaponData>();
        data.ATK = weaponDB.DB[weaponID]["ATK"].f;

        return obj;
    }

    public GameObject CreateWeapon(string weaponID,string side, WeaponManager wm) {
        WeaponController wc;
        if (side == "R") {
            wc = wm.wcR;
            if (wc.transform != null)
                wm.UnloadWeapon(side);
        }
        else if (side == "L") {
            wc = wm.wcL;
            if (wc.transform != null)
                wm.UnloadWeapon(side);
        }
        else
            return null;

        

        GameObject prefab = Resources.Load("weapon/" + weaponID) as GameObject;
        GameObject obj = GameObject.Instantiate(prefab);

        obj.transform.parent = wc.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = new Vector3(1, 1, 1);
        wm.UpdateWeapon(side, obj.GetComponent<Collider>());

        WeaponData data = obj.AddComponent<WeaponData>();
        data.ATK = weaponDB.DB[weaponID]["ATK"].f;
        

        return obj;
    }
}
