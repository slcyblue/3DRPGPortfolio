using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
	[SerializeField]
	protected Vector3 _destPos;

	[SerializeField]
	protected Define.State _state = Define.State.Idle;

	[SerializeField]
	public GameObject _lockTarget;
	public int _Id {get; set;}

	public Define.WorldObject WorldObjectType { get; protected set; } = Define.WorldObject.Unknown;

	public virtual Define.State State
	{
		get { return _state; }
		set
		{
			_state = value;

			Animator anim = GetComponent<Animator>();
			switch (_state)
			{
				case Define.State.Die:
					anim.CrossFade("DIE", 0.1f);
					break;
				case Define.State.Idle:
					anim.CrossFade("WAIT", 0.1f);
					break;
				case Define.State.Moving:
					anim.CrossFade("RUN", 0.1f);
					break;
				case Define.State.Attack:
					StartCoroutine(Attack());
					//anim.CrossFade("ATTACK", 0.1f, -1, 0);
					break;
				case Define.State.Skill:
					break;
			}
		}
	}

	IEnumerator Attack(){
		Animator anim = GetComponent<Animator>();
		anim.CrossFade("ATTACK", 0.1f, -1, 0);
		yield return new WaitForSeconds(1.0f);
	}

	public void Start() {
		Init(_Id);
	}

	public virtual void Init(int id){}

	void Update()
	{
		switch (State)
		{
			case Define.State.Die:
				UpdateDie();
				break;
			case Define.State.Moving:
				UpdateMoving();
				break;
			case Define.State.Idle:
				UpdateIdle();
				break;
			case Define.State.Attack:
				UpdateAttack();
				break;
		}
	}

	protected virtual void UpdateDie() { }
	protected virtual void UpdateMoving() { }
	protected virtual void UpdateIdle() { }
	protected virtual void UpdateAttack() { }
}
