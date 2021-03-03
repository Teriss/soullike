using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
	private CharacterController charactorController;
	public GameObject model;
	private Animator anim;

	[Header("=====Input values & Output singals=====")]
	private float x;
	private float z;
	private bool moveable = true;
	[SerializeReference]
	private Vector3 moveDir;
	private Vector3 animDelPos;
	private Vector3 forword;
	private Vector2 diretion;
	private float animForward;
	private bool run;
	private bool jump;
	private bool attackR;
	private bool attackL;
	private bool heaveAttack;
	private bool defense;
	private bool attackable;
	[SerializeReference]
	private float RecordHeight;

	[Header("=====Buttons=====")]
	private MyButton butJump;
	private MyButton butAttack;
	private MyButton butDefense;
	private MyButton butAction;


	[Header("=====Parameter setting=====")]
	public float MoveSpeed;
	public float RunSpeedMul;
	public float JumpSpeed;
	public float RotateSpeed;
	public float gravity;
	public float margin;
	public float minFalltoRollHeitht;

	private bool leftIsShield = true;
	public delegate void OnActionDelegate();
	public event OnActionDelegate OnAction;

	private int time = 0;
	public int aaa;


	void Start() {
		charactorController = GetComponent<CharacterController>();
		anim = model.GetComponent<Animator>();

		butJump = new MyButton("space");
		butAttack = new MyButton("mouse 0");
		butDefense = new MyButton("mouse 1");
		butAction = new MyButton("e");

	}

	// Update is called once per frame
	void Update() {
		//lock

	}

	private void FixedUpdate() {
		SetTrigger("attack");
		anim.SetBool("subHand", false);


		Control();



    }

	public void Control() {
		if (IsGrounded()) {
			SetBool("stillFall", false);
			attackable = true;
			if (moveable) {
				x = Input.GetAxis("Horizontal");
				z = Input.GetAxis("Vertical");
				run = Input.GetButton("Run");
				jump = butJump.OnPressed();
			}
			else {
				x = z = 0;
			}

			//move
			moveDir = (x * transform.right + z * transform.forward) * MoveSpeed * (run ? RunSpeedMul : 1.0f);

			diretion = SquareToCircle(x, z);
			forword = diretion.x * transform.right + diretion.y * transform.forward;
			animForward = Mathf.Sqrt((diretion.x * diretion.x) + (diretion.y * diretion.y)) * (run ? 2.0f : 1.0f);
			anim.SetFloat("forward", animForward);
			anim.SetFloat("right", 0);

			//jump
			if (jump) {
				if (animForward > 1) {
					SetTrigger("jump");
					moveDir.y = JumpSpeed;
					time = aaa;
				}
				else if (animForward <= 1 && animForward > 0)
					SetTrigger("jump");
				else
					anim.SetTrigger("jab");

				attackable = false;

			}

			//falling
			if (RecordHeight - transform.localPosition.y >= minFalltoRollHeitht)
				SetTrigger("roll");
			RecordHeight = 0;

		}
		else {
			//fall
			attackable = true;
			Mathf.Lerp(moveDir.x, 0, 0.5f);
			Mathf.Lerp(moveDir.z, 0, 0.5f);
			moveDir.y -= gravity * Time.fixedDeltaTime;
			if (RecordHeight > 1.48f)
				SetBool("stillFall", true);
		}

		if (time > 0) {
			charactorController.Move(moveDir * Time.fixedDeltaTime);
			time--;
		}
		else {
			charactorController.SimpleMove(moveDir);
		}
		charactorController.transform.position += animDelPos;
		animDelPos = Vector3.zero;

		if (forword != Vector3.zero)
			model.transform.forward = forword;

		//record height
		if (transform.localPosition.y > RecordHeight) {
			RecordHeight = transform.localPosition.y;
		}

		//attack
		attackR = butAttack.OnPressed();
		attackL = butDefense.OnPressed();
		heaveAttack = Input.GetButton("Run");
		defense = butDefense.IsPressing();

		if (attackable) {
			if (attackR) {
				if (heaveAttack) {

				}
				else {
					anim.SetBool("subHand", false);
					anim.SetTrigger("attack");
				}
			}
			else if (attackL) {
				if (leftIsShield) {
					if (heaveAttack) {
						SetTrigger("counterBack");
					}
					else {

					}
				}
				else {
					if (heaveAttack) {

					}
					else {
						anim.SetBool("subHand", true);
						anim.SetTrigger("attack");
					}
				}
			}
		}
		//defense
		if ((CheckState("ground") || CheckState("block")) && leftIsShield)
			anim.SetBool("defense", defense);
		else
			anim.SetBool("defense", false);

		//action
		if (butAction.OnPressed())
			OnAction.Invoke();
	}

	bool IsGrounded() {
		return (Physics.Raycast(transform.position, -Vector3.up, margin));
	}

	private Vector2 SquareToCircle(float x, float y) {
		Vector2 diretion = Vector2.zero;
		diretion.x = x * Mathf.Sqrt(1 - (y * y) / 2.0f);
		diretion.y = y * Mathf.Sqrt(1 - (x * x) / 2.0f);
		return diretion;
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


	//Animator State Event received by messages

	public void LockMovement() {
		moveable = false;
	}
	public void UnLockMovement() {
		moveable = true;
	}

	public void Stop() {
		animDelPos = Vector3.zero;
	}

	public void LockWeapon() {
		model.SendMessage("AttackDisable");
	}

	public void onDeathEnter() {
		LockMovement();
		LockWeapon();
	}

	public void onCounterBackExit() {
		model.SendMessage("CounterBackDisable");
	}

	public void onDefenseEnter() {
		anim.SetLayerWeight(anim.GetLayerIndex("Defence Layer"), 1);
	}

	public void onDefenseExit() {
		anim.SetLayerWeight(anim.GetLayerIndex("Defence Layer"), 0);
	}


	public void onRMUpdate(object deltaPosition) {
		if (CheckStateByTag("attackL") || CheckStateByTag("attackR"))
			animDelPos += (Vector3)deltaPosition;

		if (CheckState("jump") || CheckState("roll"))
			animDelPos += (Vector3)deltaPosition * 0.5f;
	}
}
