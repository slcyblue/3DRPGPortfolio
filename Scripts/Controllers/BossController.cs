using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;
using UnityEngine.AI;

public class BossController : BaseController
{
	Stat _stat;
	Item _item;
	MonsterData monsterData = null;
	Animator animator;
	GameObject player;
	GameObject boxRange;
	GameObject fillRange;
	GameObject circleRange;

	bool _startEngage = false;

	[SerializeField]
	float _scanRange = 10;

	[SerializeField]
	float _attackRange = 3;

    public override void Init(int monsterId)
    {
		WorldObjectType = Define.WorldObject.Boss;
		_Id = monsterId;
		Managers.Data.MonsterDict.TryGetValue(_Id, out monsterData);

		_stat = gameObject.GetComponent<Stat>();
		animator = gameObject.GetComponent<Animator>();
		player = Managers.Game.GetPlayer();

		if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
			Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
	}

	protected override void UpdateIdle()
	{		
		if (player == null)
			return;

		float distance = (player.transform.position - transform.position).magnitude;
		if (distance <= _scanRange)
		{
			_lockTarget = player;
			State = Define.State.Moving;
			if(!_startEngage){
				StartCoroutine(StartEngage());
				_startEngage = true;
			}
			return;
		}else{
			_lockTarget = null;
			State = Define.State.Idle;
			return;
		}
	}

    private IEnumerator StartEngage()
    {
		yield return new WaitForSeconds(5.0f);
		
		State = Define.State.Skill;
		_destPos = transform.position;

		Stack<int> usedSkill = new Stack<int>();
		int randSkill = UnityEngine.Random.Range(0,3);
		
		if(usedSkill.Count > 0){
			if(usedSkill.Peek() == randSkill){
				randSkill = GetRandomNumber(randSkill);
			}
		}
		
			
		switch(randSkill){
			case 0:
				animator.CrossFade("SKILL1",0.1f);
				Debug.Log("스킬1");
				break;
			case 1:
				animator.CrossFade("SKILL2",0.1f);
				Debug.Log("스킬2");
				break;
			case 2:
				animator.CrossFade("SKILL3",0.1f);
				Debug.Log("스킬3");
				break;
		}
		usedSkill.Push(randSkill);
    }

    protected override void UpdateMoving()
	{
		// 플레이어가 내 사정거리보다 가까우면 공격
		if (_lockTarget != null)
		{
			_destPos = _lockTarget.transform.position;
			float distance = (_destPos - transform.position).magnitude;
			if (distance <= _attackRange)
			{
				if(_lockTarget.GetComponent<Stat>().Hp >0){
				NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
				nma.SetDestination(transform.position);
				State = Define.State.Attack;
				return;
				}else{
					_lockTarget = null;
					State = Define.State.Idle;
				}
			}
		}

		// 이동
		Vector3 dir = _destPos - transform.position;
		if (dir.magnitude < 0.1f)
		{
			State = Define.State.Idle;
		}
		else
		{
			NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
			nma.SetDestination(_destPos);
			nma.speed = _stat.MoveSpeed;

			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
		}
	}

	protected override void UpdateAttack()
	{
		if (_lockTarget != null)
		{
			Vector3 dir = _lockTarget.transform.position - transform.position;
			Quaternion quat = Quaternion.LookRotation(dir);
			transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
		}
	}

	private int GetRandomNumber(int excludeNum)
	{
		var exclude = new HashSet<int>() { excludeNum };
		var range = Enumerable.Range(0, 3).Where(i => !exclude.Contains(i));

		var rand = new System.Random();
		int index = rand.Next(0, 2 - exclude.Count);
		return range.ElementAt(index);
	}

	void SlayerHit(){
		if (_lockTarget != null)
		{
			// 체력
			Stat targetStat = _lockTarget.GetComponent<Stat>();
			targetStat.OnAttacked(gameObject);

			if (targetStat.Hp > 0)
			{
				float distance = (_lockTarget.transform.position - transform.position).magnitude;
				if (distance <= _attackRange)
					State = Define.State.Attack;
				else
					State = Define.State.Moving;
			}
			else
			{
				State = Define.State.Idle;
			}
		}
		else
		{
			State = Define.State.Idle;
		}
	}
	IEnumerator SetSkill1Range(){
		float startTime = 0.0f;
		float skillReadyTime = 2.0f;
		boxRange = Managers.Resource.Instantiate("SkillRange/BoxRange");
		fillRange = Managers.Resource.Instantiate("SkillRange/BoxRange");

		boxRange.transform.position = transform.position;
		fillRange.transform.position = transform.position;
		SpriteRenderer fillSprite = fillRange.GetComponent<SpriteRenderer>();
		
		Color fillColor = fillSprite.color;
		fillColor.a = 200;
		fillColor.g = 150;
		fillSprite.color = fillColor;
		
		Vector3 fillScale = fillRange.transform.localScale;
		Vector3 boxScale = boxRange.transform.localScale;

		fillScale = Vector3.zero;
		boxScale = new Vector3(3.0f,0.5f,0.0f);

		while(startTime <= skillReadyTime){
			startTime += Time.deltaTime;
			fillScale.x = boxScale.x/skillReadyTime * startTime;
			fillScale.y = boxScale.y;
			fillRange.transform.localScale = fillScale;
			yield return new WaitForFixedUpdate();
		}

		RaycastHit[] hits = Physics.BoxCastAll(boxRange.transform.position + new Vector3(4.0f,0,0), boxRange.transform.localScale/2, transform.up, boxRange.transform.rotation);
		foreach(var hit in hits){
			Debug.Log(hit.collider.name);
			//OnDrawGizmos();
			if(hit.collider.CompareTag("Player")){
				Debug.Log("피격");
				OnSkill1Hit();
			}
		}
	}

	IEnumerator SetSkill2Range(){
		float startTime = 0.0f;
		float skillReadyTime = 1.5f;
		circleRange = Managers.Resource.Instantiate("SkillRange/CircleRange");
		fillRange = Managers.Resource.Instantiate("SkillRange/CircleRange");

		circleRange.transform.position = transform.position;
		fillRange.transform.position = transform.position;
		SpriteRenderer fillSprite = fillRange.GetComponent<SpriteRenderer>();
		
		Color fillColor = fillSprite.color;
		fillColor.a = 200;
		fillColor.g = 150;
		fillSprite.color = fillColor;
		
		Vector3 fillScale = fillRange.transform.localScale;
		Vector3 circleScale = circleRange.transform.localScale;

		fillScale = Vector3.zero;

		while(startTime <= skillReadyTime){
			startTime += Time.deltaTime;
			fillScale.x = circleScale.x/skillReadyTime * startTime;
			fillScale.y = circleScale.y/skillReadyTime * startTime;
			fillRange.transform.localScale = fillScale;
			yield return new WaitForFixedUpdate();
		}

		Ray ray = new Ray(circleRange.transform.position, Vector3.up);
		RaycastHit[] hits = Physics.SphereCastAll(ray, circleRange.transform.localScale.x);
		foreach(var hit in hits){
			Debug.Log(hit.collider.name);
			//OnDrawGizmos();
			if(hit.collider.CompareTag("Player")){
				OnSkill2Hit();
			}
		}
	}

	// void OnDrawGizmos() {
	// 	if(boxRange != null){
			
	// 		float maxDistance = 100;
	// 		RaycastHit hit;
	// 		// Physics.BoxCast (레이저를 발사할 위치, 사각형의 각 좌표의 절판 크기, 발사 방향, 충돌 결과, 회전 각도, 최대 거리)
	// 		bool isHit = Physics.BoxCast (boxRange.transform.position+ new Vector3(4.0f,0,0), boxRange.transform.localScale/2, transform.up, out hit, boxRange.transform.rotation, maxDistance);
	// 		Gizmos.color = Color.red;
	// 		if (isHit) {
	// 			Gizmos.DrawRay (boxRange.transform.position+ new Vector3(4.0f,0,0), transform.up * 3);
	// 			Gizmos.DrawWireCube (boxRange.transform.position+ new Vector3(4.0f,0,0) + transform.up * 3, boxRange.transform.localScale/2);
	// 		} else {
	// 			Gizmos.DrawRay (boxRange.transform.position+ new Vector3(4.0f,0,0), transform.up * 3);
	// 		}
	// 	}else if(circleRange != null){
	// 		RaycastHit hit;
	// 		// Physics.SphereCast (레이저를 발사할 위치, 구의 반경, 발사 방향, 충돌 결과, 최대 거리)
	// 		bool isHit = Physics.SphereCast (circleRange.transform.position, circleRange.transform.localScale.x / 2, transform.up, out hit);
	
	// 		Gizmos.color = Color.red;
	// 		if (isHit) {
	// 			Gizmos.DrawRay (circleRange.transform.position, transform.up * 3);
	// 			Gizmos.DrawWireSphere (circleRange.transform.position + transform.up * 3, circleRange.transform.lossyScale.x / 2);
	// 		} else {
	// 			Gizmos.DrawRay (circleRange.transform.position, transform.up * 3);
	// 		}
	// 	}
    // }

	void OnSkill1Hit(){
		PlayerStat targetStat = player.GetComponent<PlayerStat>();
		targetStat.OnSkilled(gameObject, _stat.Attack * 3.0f);
	}
	void OnSkill2Hit(){
		PlayerStat targetStat = player.GetComponent<PlayerStat>();
		targetStat.OnSkilled(gameObject, _stat.Attack * 1.5f);
	}

	void EndSkill1Event(){
		State = Define.State.Idle;
		StartCoroutine(StartEngage());
	}


	void EndSkill2Event(){
		State = Define.State.Idle;
		StartCoroutine(StartEngage());
	}
	void EndSkill3Event(){
		State = Define.State.Idle;
		StartCoroutine(StartEngage());
	}

}
