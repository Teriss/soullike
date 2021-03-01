using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionControl : MonoBehaviour
{
    private Animator anim;

    public void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnAnimatorMove()
    {

        SendMessageUpwards("onRMUpdate",(object)anim.deltaPosition);
    }

}
