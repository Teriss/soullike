using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : StateManager
{
    private void Start() {
        AddHP(0);
    }
    private void Update() {
        isGround = cm.inputs.CheckState("ground");
        isJump = cm.inputs.CheckState("jump");
        isFall = cm.inputs.CheckState("fall");
        isRoll = cm.inputs.CheckState("roll");
        isJab = cm.inputs.CheckState("jab");
        isAttack = cm.inputs.CheckStateByTag("attackR") || cm.inputs.CheckStateByTag("attackL");
        isHit = cm.inputs.CheckState("hited");
        isDie = cm.inputs.CheckState("death");
        isBlock = cm.inputs.CheckState("block");
        isCounterBack = cm.inputs.CheckState("counterBack");
        isStunned = cm.inputs.CheckState("stunned");
        //isDefense = am.ac.CheckState("defense1h", "Defence Layer");

        isCanDefense = isGround || isBlock;
        isDefense = isCanDefense && cm.inputs.CheckState("defense1h", "Defence Layer");
        isImmortal = isRoll || isJab;
    }
}
