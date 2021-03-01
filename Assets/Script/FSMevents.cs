using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMevents : StateMachineBehaviour
{
    public string[] clearSignals;
    public string[] onStateEnters;
    public string[] onUpdateMessages;
    public string[] onStateExits;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (var signal in clearSignals) {
            animator.ResetTrigger(signal);
        }
        foreach (var msg in onStateEnters)
            animator.gameObject.SendMessageUpwards(msg);
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach(var msg in onUpdateMessages)
        {
            animator.gameObject.SendMessageUpwards(msg);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (var msg in onStateExits)
            animator.gameObject.SendMessageUpwards(msg);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
