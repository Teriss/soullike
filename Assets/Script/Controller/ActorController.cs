using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour {
    [Header("Speed setting")]
    public float walkSpeed = 1.4f;
    public float runSpeed = 2.4f;
    public float jumpSpeed = 4.0f;
    public float rollSpeed = 2.5f;
    public float jabSpeed = 3.0f;

    [Header("Friction setting")]
    public PhysicMaterial frictionOne;
    public PhysicMaterial frictionZero;

    public GameObject model;
    public CameraController camcon;
    public PlayerInput pi;
    private PlayerInput[] pis;
    private Animator anim;
    private Rigidbody rigid;
    private CapsuleCollider col;

    private Vector3 movingVec;
    private Vector3 elterVec;
    private Vector3 deltaPos;
    private bool moveable = true;
    private bool attackabel = true;
    private bool trackDiretion = false;

    public bool leftIsShield = false;

    public delegate void OnActionDelegate();
    public event OnActionDelegate OnAction;


    private void Awake() {
        pis = GetComponents<PlayerInput>();
        foreach (var item in pis) {
            if (item.enabled == true) {
                pi = item;
                break;
            }
        }
        anim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update() {
        //if (pi.lockon)
        //    camcon.Lockon();

        float runAnimSpeed = (pi.run ? 2.0f : 1.0f);

        //******************
        //move
        if (camcon.lockState) {
            //locked
            Vector3 localDvec = transform.InverseTransformVector(pi.Dforword);
            anim.SetFloat("forward", localDvec.z * runAnimSpeed);
            anim.SetFloat("right", localDvec.x * runAnimSpeed);

            if (trackDiretion)
                model.transform.forward = movingVec.normalized;
            else
                model.transform.forward = transform.forward;

            if (moveable)
                movingVec = pi.Dforword * walkSpeed * (pi.run ? runSpeed : 1.0f);
        }
        else {
            //unlocked
            anim.SetFloat("forward", pi.Danima * Mathf.Lerp(anim.GetFloat("forward"), runAnimSpeed, 0.4f));
            anim.SetFloat("right", 0);


            if (pi.Danima > 0.1f) {
                model.transform.forward = Vector3.Slerp(model.transform.forward, pi.Dforword, 0.3f);
            }
            if (moveable)
                movingVec = pi.Danima * model.transform.forward * walkSpeed * (pi.run ? runSpeed : 1.0f);
        }

        //***************
        //other sports
        if (rigid.velocity.magnitude > 10)
            anim.SetTrigger("roll");

        if (pi.jump) {
            anim.SetTrigger("jump");
            attackabel = false;
            if (Mathf.Abs(pi.Danima) < 0.01f)
                anim.SetTrigger("jab");
        }

        //attack
        if ((pi.leftMouse || pi.rightMouse) && (CheckState("ground") || CheckStateByTag("attackR") || CheckStateByTag("attackL")) && attackabel) {
            if (pi.rightMouse) {                    //left hand attack
                if (leftIsShield) {
                    if (pi.heavyAttack)
                        //counter back
                        SetTrigger("counterBack");
                    else {

                    }
                }
                else {
                    if (pi.heavyAttack) {
                        // left hand heavy attack
                    }
                    else {
                        //left hand soft attack
                        anim.SetBool("subHand", true);
                        anim.SetTrigger("attack");
                    }
                }
            }
            else if (pi.leftMouse) {                //right hand attack
                anim.SetBool("subHand", false);
                anim.SetTrigger("attack");
            }
        }
        //defense
        if ((CheckState("ground") || CheckState("block")) && leftIsShield && !pi.run)
            anim.SetBool("defense", pi.defense);
        else
            anim.SetBool("defense", false);

        if (pi.action) {
            OnAction.Invoke();
        }
    }

    private void FixedUpdate() {
        rigid.position += deltaPos;
        rigid.velocity = new Vector3(movingVec.x, rigid.velocity.y, movingVec.z) + elterVec;
        elterVec = deltaPos = Vector3.zero;
    }

    public bool CheckState(string stateName, string layoutName = "Base Layers") {
        int layoutIndex = anim.GetLayerIndex(layoutName);
        return anim.GetCurrentAnimatorStateInfo(layoutIndex).IsName(stateName);
    }
    public bool CheckStateByTag(string tagName, string layoutName = "Base Layers") {
        int layoutIndex = anim.GetLayerIndex(layoutName);
        return anim.GetCurrentAnimatorStateInfo(layoutIndex).IsTag(tagName);
    }

    public void SetTrigger(string triggerName) {
        anim.SetTrigger(triggerName);
    }

    public void SetBool(string boolName, bool value) {
        anim.SetBool(boolName, value);
    }

    public Animator GetAnimator() {
        return anim;
    }

    public void Stop() {
        movingVec = Vector3.zero;
        elterVec = movingVec;
        deltaPos = movingVec;
    }



    //
    //Animator State Event received by messages

    public void onJumpEnter() {
        pi.moveEnable = false;
        moveable = false;
        elterVec = new Vector3(0, jumpSpeed, 0);
        trackDiretion = true;
    }

    public void onGroundEnter() {
        pi.moveEnable = true;
        moveable = true;
        attackabel = true;
        col.material = frictionOne;
        trackDiretion = false;
    }

    public void onGroundExit() {
        col.material = frictionZero;
    }

    public void onTheGround() {
        anim.SetBool("stillFall", false);
    }

    public void inTheAir() {
        anim.SetBool("stillFall", true);
    }
    public void onRollEnter() {
        pi.moveEnable = false;
        moveable = false;
        elterVec = new Vector3(0, rollSpeed, 0);
        trackDiretion = true;
    }

    public void onRoll_JumpUpdate() {
        Mathf.Lerp(movingVec.x, 0, 0.5f);
        Mathf.Lerp(movingVec.z, 0, 0.5f);
    }

    public void onJabEnter() {
        pi.moveEnable = false;
        moveable = false;
    }

    public void onJabUpdate() {
        elterVec = model.transform.forward * jabSpeed * anim.GetFloat("jabVelocity");
    }

    public void onAttack1hA() {
        pi.moveEnable = false;
    }

    public void onAttackExit() {
        model.SendMessage("AttackDisable");
    }

    public void onDefenseEnter() {
        anim.SetLayerWeight(anim.GetLayerIndex("Defence Layer"), 1);
    }

    public void onDefenseExit() {
        anim.SetLayerWeight(anim.GetLayerIndex("Defence Layer"), 0);
    }

    public void onHitEnter() {
        pi.moveEnable = false;
        movingVec = Vector3.zero;
        model.SendMessage("AttackDisable");
    }

    public void onDeathEnter() {
        pi.moveEnable = false;
        movingVec = Vector3.zero;
        model.SendMessage("AttackDisable");
    }

    public void onBlockEnter() {
        pi.moveEnable = false;
    }

    public void onStunnedEnter() {
        pi.moveEnable = false;
        movingVec = Vector3.zero;
    }

    public void onCounterBackEnter() {
        pi.moveEnable = false;
        movingVec = Vector3.zero;
    }

    public void onCounterBackExit() {
        model.SendMessage("CounterBackDisable");
    }


    public void onRMUpdate(object deltaPosition) {
        if (CheckStateByTag("attackL") || CheckStateByTag("attackR"))
            deltaPos += (Vector3)deltaPosition;

        if (CheckState("jump") || CheckState("roll"))
            deltaPos += (Vector3)deltaPosition * 0.5f;
    }

}


