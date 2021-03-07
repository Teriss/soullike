using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : CharactorInput
{
	//private CharacterController cc;
	//public GameObject model;
	//private Animator anim;

	private NavMeshAgent navMeshAgent;
	public GameObject target;



	public float maxSeekDistance;
	private float times = 10.0f;
	private int Action;


	private enum EnemyState { idel,seek,run,walk,attack}
	private EnemyState state = new EnemyState();
	private AnimatorStateInfo animState;
	private float distanceToTraget;

	void Start() {
		navMeshAgent = GetComponent<NavMeshAgent>();//获取navmeshagent
		anim = model.GetComponent<Animator>();
		cc = GetComponent<CharacterController>();
	}
    // Update is called once per frame
    void Update() {
        //navMeshAgent.SetDestination(target.transform.position);//设置导航的目标点

        //CheckState();
        //switch (state) {
        //    case EnemyState.idel:
        //        //idel
        //        Idel();
        //        break;
        //    case EnemyState.seek:
        //        Seek();
        //        break;
        //}
    }

    private void CheckState() {
		distanceToTraget = Vector3.Distance(target.transform.position, this.transform.position);
		animState = anim.GetCurrentAnimatorStateInfo(0);
		if (distanceToTraget > maxSeekDistance)
			state = EnemyState.idel;
		else
			state = EnemyState.seek;
	}

	private void Idel() {
		times -= Time.deltaTime;
		if (times < 0) {
			Action = Random.Range(0, 5);
			times = 3.0f;
		}
		switch (Action) {
			case 0:
				//anim.SetBool("isWalk", false);
				transform.Rotate(0, 8 * Time.deltaTime, 0);
				break;
			case 1:
				//anim.SetBool("isWalk", false);
				transform.Rotate(0, -8 * Time.deltaTime, 0);
				break;
			case 2:
				//anim.SetBool("isWalk", true);
				cc.Move(transform.forward * 3 * Time.deltaTime);
				break;
			default:
				//anim.SetBool("isWalk", false);
				break;
		}
	}

	private void Seek() {
		if(distanceToTraget > 3) {
			transform.LookAt(target.transform.position);
			navMeshAgent.SetDestination(target.transform.position);
        }
        else {
			anim.SetTrigger("attack");
			navMeshAgent.ResetPath();
        }
		
	}

	//public void SetTrigger(string name) {
	//	anim.SetTrigger(name);
 //   }

	//public bool CheckState(string stateName, string layoutName = "Base Layers") {
	//	int layoutIndex = anim.GetLayerIndex(layoutName);
	//	return anim.GetCurrentAnimatorStateInfo(layoutIndex).IsName(stateName);
	//}

	//public bool CheckStateByTag(string tagName, string layoutName = "Base Layers") {
	//	int layoutIndex = anim.GetLayerIndex(layoutName);
	//	return anim.GetCurrentAnimatorStateInfo(layoutIndex).IsTag(tagName);
	//}


	public void LockMovement() {
		//moveable = false;
	}
	public void UnLockMovement() {
		//moveable = true;
	}

	public void LockWeapon() {
		model.SendMessage("AttackDisable");
	}

}
