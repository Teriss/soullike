using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : CharactorInput
{
	public enum EnemyState { idel, patrol, seek, attack, back ,death }
	private EnemyState aiState = new EnemyState();
    public EnemyState AIstate { get => aiState;}

	private AnimatorStateInfo animState;
	private NavMeshAgent navMeshAgent;
	private GameObject target;

	private Vector3 originPos;
	private Vector3 originForword;
	private Vector3 targetPos = Vector3.zero;
	public float maxSeekDistance;
	[SerializeReference]
	private float distanceToOri;

	private bool wait = true;
	private float timer;
	private int attackAction;


    void Start() {
		navMeshAgent = GetComponent<NavMeshAgent>();
		anim = model.GetComponent<Animator>();
		cc = GetComponent<CharacterController>();
		aiState = EnemyState.idel;
		originPos = transform.position;
		originForword = transform.forward;
	}
    // Update is called once per frame
    void Update() {
		animState = anim.GetCurrentAnimatorStateInfo(0);
		distanceToOri = Vector3.Distance(transform.position, originPos);

		if (animState.IsName("ground")) {
			navMeshAgent.isStopped = false;
            switch (aiState) {
                case EnemyState.idel:
                    Idel();
                    break;
                case EnemyState.patrol:
                    Patrol();
                    break;
                case EnemyState.seek:
                    Seek();
                    break;
                case EnemyState.attack:
                    Attack();
                    break;
                case EnemyState.back:
					Back();
                    break;
                case EnemyState.death:
                    break;
            }
        }
        else {
			navMeshAgent.isStopped = true;
        }
    }


    private void Idel() {
		anim.SetFloat("forward", 0);
		timer -= Time.deltaTime;
		if(timer <= 0) {
			//transform.forward = -transform.forward;
			transform.Rotate(new Vector3(0, 90, 0));
			timer = 3.0f;
			aiState = EnemyState.patrol;
        }
	}

	private void Patrol() {
		anim.SetFloat("forward", 1.0f);
		if (targetPos == Vector3.zero) {
			targetPos = transform.position + 10 * transform.forward;
        }
		float dis = Vector3.Distance(transform.position, targetPos);
		if (dis > 0.1f) {
			navMeshAgent.SetDestination(targetPos);
		}
        else {
			//navMeshAgent.ResetPath();
			targetPos = Vector3.zero;
			aiState = EnemyState.idel;
        }
		
	}

	private void Seek() {
		if (distanceToOri > maxSeekDistance) {
			aiState = EnemyState.back;
			return;
        }

		anim.SetFloat("forward", 2.0f);
		transform.LookAt(target.transform.position);
		navMeshAgent.SetDestination(target.transform.position);
	}

	private void Attack() {
		attackAction = UnityEngine.Random.Range(0, 4);
        anim.SetFloat("forward", 0);
		anim.SetTrigger("attack");
		anim.SetInteger("attackType",attackAction);
	}

	private void Back() {
		target = null;
		targetPos = originPos;
		navMeshAgent.ResetPath();
		anim.SetFloat("forward", 0);
		if (wait)
			wait = false;
        else {
			anim.SetFloat("forward", 1.0f);
			if (distanceToOri >0.1f) {
				navMeshAgent.SetDestination(targetPos);
			}
			else {
				targetPos = Vector3.zero;
				transform.forward = originForword;
				aiState = EnemyState.idel;
			}
		}
    }


	public void FoundTarget(GameObject target) {
		if(this.target == null && aiState != EnemyState.back && aiState != EnemyState.death) {
			this.target = target;
			aiState = EnemyState.seek;
		}
    }

	public void CanAttack(bool value) {
        if (value) {
			if (target != null && aiState == EnemyState.seek) {
				aiState = EnemyState.attack;
				navMeshAgent.ResetPath();
			}
        }
        else {
			if (target != null && aiState == EnemyState.attack)
				aiState = EnemyState.seek;
		}
    }

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
