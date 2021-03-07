using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateManager : ActorManagerInterface
{

    public float maxHP = 50.0f;
    public float HP = 50.0f;

    protected bool isGround;
    protected bool isJump;
    protected bool isFall;
    protected bool isRoll;
    protected bool isJab;
    protected bool isAttack;
    protected bool isHit;
    protected bool isDie;
    protected bool isBlock;
    protected bool isDefense;
    protected bool isCounterBack;
    protected bool isCounterBackEnable;
    protected bool isStunned;

    protected bool isCanDefense;
    protected bool isImmortal;


    public bool IsGround { get => isGround;}
    public bool IsJump { get => isJump; }
    public bool IsFall { get => isFall; }
    public bool IsRoll { get => isRoll;}
    public bool IsJab { get => isJab; }
    public bool IsAttack { get => isAttack; }
    public bool IsHit { get => isHit; }
    public bool IsDie { get => isDie; }
    public bool IsBlock { get => isBlock;}
    public bool IsDefense { get => isDefense; }
    public bool IsCanDefense { get => isCanDefense;}
    public bool IsImmortal { get => isImmortal;}
    public bool IsCounterBackEnable { get => isCounterBackEnable; set => isCounterBackEnable = value; }
    public bool IsCounterBack { get => isCounterBack; set => isCounterBack = value; }
    public bool IsStunned { get => isStunned;}

    //private void Start() {
    //    AddHP(0);
    //}

    //private void Update() {
    //    isGround = am.pc.CheckState("ground");
    //    isJump = am.pc.CheckState("jump");
    //    isFall = am.pc.CheckState("fall");
    //    isRoll = am.pc.CheckState("roll");
    //    isJab = am.pc.CheckState("jab");
    //    isAttack = am.pc.CheckStateByTag("attackR") || am.pc.CheckStateByTag("attackL");
    //    isHit = am.pc.CheckState("hited");
    //    isDie = am.pc.CheckState("death");
    //    isBlock = am.pc.CheckState("block");
    //    isCounterBack = am.pc.CheckState("counterBack");
    //    isStunned = am.pc.CheckState("stunned");
    //    //isDefense = am.ac.CheckState("defense1h", "Defence Layer");

    //    isCanDefense = isGround || isBlock;
    //    isDefense = isCanDefense && am.pc.CheckState("defense1h", "Defence Layer");
    //    isImmortal = isRoll || isJab;
    //}


    public void AddHP(float value) {
        HP += value;
        HP = Mathf.Clamp(HP, 0, maxHP);

    }

}
