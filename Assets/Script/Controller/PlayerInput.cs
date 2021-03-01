using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerInput : MonoBehaviour
{
    [Header("Output Signal")]
    //diretion signal
    public float Dup;
    public float Dright;
    public float Danima;        //动画混合树变量
    public Vector3 Dforword;    //人物方向向量

    //visual angle signal
    public float Vup;
    public float Vright;

    //state signal
    public bool action;
    public bool run;
    public bool jump;
    public bool defense;
    public bool lockon;
    public bool leftMouse;
    public bool rightMouse;
    public bool heavyAttack;

    public bool moveEnable = true;


    [Header("Others")]
    protected float targetDup;
    protected float targetDRight;
    protected float velocityDup;
    protected float velocityDRight;



    protected Vector2 SquareToCircle(float x, float y)
    {
        Vector2 diretion = Vector2.zero;
        diretion.x = x * Mathf.Sqrt(1 - (y * y) / 2.0f);
        diretion.y = y * Mathf.Sqrt(1 - (x * x) / 2.0f);
        return diretion;
    }
}