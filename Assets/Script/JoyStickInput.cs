using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickInput : PlayerInput
{

    [Header("Input Key")]
    public string left_x = "left-X";
    public string left_y = "left-Y";
    public string right_x = "right-X";
    public string right_y = "right-Y";

    public string keyA = "A";
    public string keyB = "B";
    public string keyX = "X";
    public string keyY = "Y";

    public string keyLB = "LB";
    public string keyRB = "RB";
    void Update()
    {

        Vup = Input.GetAxis(right_y);
        Vright = -Input.GetAxis(right_x);


        MovementValueCul();

        run = Input.GetButton(keyA);
        defense = Input.GetButton(keyLB);

        //bool jump_key_press = Input.GetButtonDown(keyB);
        //if (jump_key_press != hasjump && jump_key_press == true && moveEnable)
        //{
        //    jump = true;

        //}
        //else
        //{
        //    jump = false;
        //}
        //hasjump = jump_key_press;

        //bool attack_key_press = Input.GetButtonDown(keyRB);
        //if (attack_key_press != hasattack && attack_key_press == true)
        //{
        //    attack = true;

        //}
        //else
        //{
        //    attack = false;
        //}
        //hasattack = attack_key_press;
    }

    private void MovementValueCul()
    {
        if (moveEnable)
        {
            targetDup = Input.GetAxis(left_y);
            targetDRight = -Input.GetAxis(left_x);
        }
        else
        {
            targetDup = targetDRight = 0;
        }

        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDRight, ref velocityDRight, 0.1f);

        Vector2 diretion = SquareToCircle(Dup, Dright);

        Dforword = diretion.x * transform.right + diretion.y * transform.forward;
        Danima = Mathf.Sqrt((diretion.x * diretion.x) + (diretion.y * diretion.y));
    }
}
