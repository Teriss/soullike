using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : ActorManagerInterface
{

    public float maxHP = 50.0f;
    public float HP = 50.0f;

    private bool isGround;
    private bool isJump;
    private bool isFall;
    private bool isRoll;
    private bool isJab;
    private bool isAttack;
    private bool isHit;
    private bool isDie;
    private bool isBlock;
    private bool isDefense;
    private bool isCounterBack;
    private bool isCounterBackEnable;
    private bool isStunned;

    private bool isCanDefense;
    private bool isImmortal;


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

    private void Start() {
        AddHP(0);
    }

    private void Update() {
        isGround = am.ac.CheckState("ground");
        isJump = am.ac.CheckState("jump");
        isFall = am.ac.CheckState("fall");
        isRoll = am.ac.CheckState("roll");
        isJab = am.ac.CheckState("jab");
        isAttack = am.ac.CheckStateByTag("attackR") || am.ac.CheckStateByTag("attackL");
        isHit = am.ac.CheckState("hited");
        isDie = am.ac.CheckState("death");
        isBlock = am.ac.CheckState("block");
        isCounterBack = am.ac.CheckState("counterBack");
        isStunned = am.ac.CheckState("stunned");
        //isDefense = am.ac.CheckState("defense1h", "Defence Layer");

        isCanDefense = isGround || isBlock;
        isDefense = isCanDefense && am.ac.CheckState("defense1h", "Defence Layer");
        isImmortal = isRoll || isJab;
    }
    

    public void AddHP(float value) {
        HP += value;
        HP = Mathf.Clamp(HP, 0, maxHP);

    }

}
