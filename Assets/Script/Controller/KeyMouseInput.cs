using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyMouseInput : PlayerInput
{

    [Header("Input Key Setting")]
    public string KeyUp;
    public string KeyDown;
    public string KeyLeft;
    public string KeyRight;

    public string KeyAction;
    public string KeyRun;
    public string KeyJump;
    public string KeyLeftMouse;
    public string KeyRightMouse;
    public string KeyHeavyAttack;
    public string KeyLock;

    public string KeyArrUp;
    public string KeyArrDown;
    public string KeyArrLeft;
    public string KeyArrRight;

    public float mouseSpeedX;
    public float mouseSpeedY;

    //**********
    //button
    private MyButton butForward;
    private MyButton butBack;
    private MyButton butLeft;
    private MyButton butRight;
    
    private MyButton butArrUp;
    private MyButton butArrDown;
    private MyButton butArrLeft;
    private MyButton butArrUpRight;

    private MyButton butAction;
    private MyButton butRun;
    private MyButton butJump;
    private MyButton butLeftMouse;
    private MyButton butRightMouse;
    private MyButton butDefense;
    private MyButton butHeavyAttack;
    private MyButton butLock;

    private void Awake()
    {
        butForward = new MyButton(KeyUp);
        butBack = new MyButton(KeyDown);
        butLeft = new MyButton(KeyLeft);
        butRight = new MyButton(KeyRight);

        butArrUp = new MyButton(KeyArrUp);
        butArrDown = new MyButton(KeyArrDown);
        butArrLeft = new MyButton(KeyArrLeft);
        butArrUpRight = new MyButton(KeyArrRight);

        butAction = new MyButton(KeyAction);
        butRun = new MyButton(KeyRun);
        butJump = new MyButton(KeyJump);
        butLeftMouse = new MyButton(KeyLeftMouse);
        butRightMouse = new MyButton(KeyRightMouse);
        butDefense = new MyButton(KeyRightMouse);
        butHeavyAttack = new MyButton(KeyHeavyAttack);
        butLock = new MyButton(KeyLock);
    }

    // Update is called once per frame
    void Update()
    {
        //camera control
        Vup = Input.GetAxis("Mouse Y") * mouseSpeedY;
        Vright = Input.GetAxis("Mouse X") * mouseSpeedX;
        if (butArrUp.IsPressing()|| butArrDown.IsPressing()) {
            Vup = (butArrUp.IsPressing() ? 1.0f : 0) - (butArrDown.IsPressing() ? 1.0f : 0);
        }
        if (butArrUpRight.IsPressing() || butArrLeft.IsPressing()){
            Vright = (butArrUpRight.IsPressing() ? 1.0f : 0) - (butArrLeft.IsPressing() ? 1.0f : 0);
        }

        //sport control
        MovementValueCul();

        action = butAction.OnPressed();
        run = butRun.IsPressing();
        if(moveEnable)  jump = butJump.OnPressed();
        leftMouse = butLeftMouse.OnPressed();
        rightMouse = butRightMouse.OnPressed();
        defense = butDefense.IsPressing();
        heavyAttack = butHeavyAttack.IsPressing();
        lockon = butLock.OnPressed();
    }

    private void MovementValueCul()
    {
        if (moveEnable)
        {
            targetDup = (butForward.IsPressing() ? 1.0f : 0) - (butBack.IsPressing() ? 1.0f : 0);
            targetDRight = (butRight.IsPressing() ? 1.0f : 0)  - (butLeft.IsPressing() ? 1.0f : 0);

            Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
            Dright = Mathf.SmoothDamp(Dright, targetDRight, ref velocityDRight, 0.1f);
        }
        else {
            targetDup = targetDRight = 0;
            Dup = Dright = 0;
        }


        Vector2 diretion = SquareToCircle(Dright, Dup);
        Dforword = diretion.x * transform.right + diretion.y * transform.forward;
        Danima = Mathf.Sqrt((diretion.x * diretion.x) + (diretion.y * diretion.y));
    }
}
