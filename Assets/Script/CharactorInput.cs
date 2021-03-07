using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharactorInput : MonoBehaviour
{
	protected CharacterController cc;
	public GameObject model;
	protected Animator anim;

	public bool moveable = true;

    public delegate void OnActionDelegate();
    public event OnActionDelegate OnAction;

	protected void Action() {
		OnAction.Invoke();
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

	public bool CheckState(string stateName, string layoutName = "Base Layers") {
		int layoutIndex = anim.GetLayerIndex(layoutName);
		return anim.GetCurrentAnimatorStateInfo(layoutIndex).IsName(stateName);
	}

	public bool CheckStateByTag(string tagName, string layoutName = "Base Layers") {
		int layoutIndex = anim.GetLayerIndex(layoutName);
		return anim.GetCurrentAnimatorStateInfo(layoutIndex).IsTag(tagName);
	}

}
