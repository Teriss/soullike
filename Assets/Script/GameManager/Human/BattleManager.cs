using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class BattleManager : ActorManagerInterface
{
    private CapsuleCollider defCol;
    private void Start() {
        defCol = GetComponent<CapsuleCollider>();
        defCol.center = Vector3.up * 1.0f;
        defCol.height = 2.0f;
        defCol.radius = 0.5f;
        defCol.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other) {
        WeaponController attackerWC = other.GetComponentInParent<WeaponController>();
        if (attackerWC == null)
            return;

        GameObject attacker = attackerWC.wm.am.gameObject;
        GameObject attackee = am.gameObject;

        Vector3 attackDir = attackee.transform.position - attacker.transform.position;
        Vector3 counterDir = attacker.transform.position - attackee.transform.position;

        //float attackAngle = Vector3.Angle(attacker.transform.forward, attackDir);   //攻击角度
        //float counterAngle1 = Vector3.Angle(attackee.transform.forward, counterDir);    //弹反角度
        //float counterAngle2 = Vector3.Angle(attackee.transform.forward, attackee.transform.forward);    //弹反双方面向差

        //bool attackValid = (attackAngle < 100);
        bool attackValid = true;
        //bool counterValid = (counterAngle1 < 90 && Mathf.Abs(counterAngle2) < 90);
        bool counterValid = true;


        if (other.tag == "weapon") {
            am.TryDamage(attackerWC,attackValid,counterValid);
        }
    }

}
