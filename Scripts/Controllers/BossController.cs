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
	Animator animator;
	GameObject player;
	GameObject boxRange;
	GameObject _fillRange;
	GameObject _circleRange;

	bool _startEngage = false;

	[SerializeField]
	float _skill1Range = 2.5f;
	[SerializeField]
	float _skill2Range = 8.0f;
	float _skill2HitRange;

	[SerializeField]
	float _skill3Range = 10.0f;
	[SerializeField]
	float _scanRange = 10;
	[SerializeField]
	float _attackRange = 3;

    public override void Init(int monsterId)
    {
		WorldObjectType = Define.WorldObject.Boss;
		_Id = monsterId;

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
				StartCoroutine(StartSkill());
				_startEngage = true;
			}
			return;
		}else{
			_lockTarget = null;
			State = Define.State.Idle;
			return;
		}
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

	private IEnumerator StartSkill()
    {
		yield return new WaitForSeconds(5.0f);
		
		State = Define.State.Skill;

		NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
		nma.SetDestination(transform.position);

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

#region Skill1
	IEnumerator Skill1ReadyEffect(){
		GameObject skillEffect1 = Managers.Resource.Instantiate("Effects/sacred/sacred_fx_11", gameObject.transform);
		yield return new WaitForSeconds(1.5f);
		Managers.Resource.Destroy(skillEffect1);
	}
	
	IEnumerator SetSkill1Range(){
		float startTime = 0.0f;
		float skillReadyTime = 1.0f;
		Vector3 randPos;

		for(int i=0; i<10;i++){
			GameObject circleRange = Managers.Resource.Instantiate("SkillRange/CircleRange");
			GameObject fillRange = Managers.Resource.Instantiate("SkillRange/CircleRange");
			fillRange.name = "FillRange";

			circleRange.transform.localScale = new Vector3(_skill1Range,_skill1Range, 0);
			fillRange.transform.localScale = Vector3.zero;
			
			SpriteRenderer fillSprite = fillRange.GetComponent<SpriteRenderer>();
			
			Color fillColor = fillSprite.color;
			fillColor.a = 200;
			fillColor.g = 150;
			fillSprite.color = fillColor;

			NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();

			while (true)
			{
				Vector3 randDir = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(0, 20.0f);
				randPos = transform.position + randDir;

				NavMeshPath path = new NavMeshPath();
				if (nma.CalculatePath(randPos, path))
					break;
			}
			
			randPos = Util.FindGround(randPos);
			randPos.y += 0.1f;
			
			circleRange.transform.position = randPos;
			fillRange.transform.position = randPos;

			StartCoroutine(Skill1FillRange(startTime,skillReadyTime,circleRange,fillRange));
			
			yield return new WaitForSeconds(0.5f);
		}
	}
	IEnumerator Skill1FillRange(float startTime, float skillReadyTime, GameObject circleRange, GameObject fillRange){
		while(startTime <= skillReadyTime){
			startTime += Time.deltaTime;
			fillRange.transform.localScale = circleRange.transform.localScale/skillReadyTime * startTime;
			yield return new WaitForFixedUpdate();
		}

		StartCoroutine(Skill1Effect(circleRange, fillRange));
	}
	
	IEnumerator Skill1Effect(GameObject circleRange, GameObject fillRange){
		GameObject skillEffect1 =Managers.Resource.Instantiate("Effects/fire/fire_fx_47");
		

		float range = circleRange.transform.lossyScale.x;
		Vector3 pos = circleRange.transform.position;

		Managers.Resource.Destroy(circleRange);
		Managers.Resource.Destroy(fillRange);
		
		skillEffect1.transform.position = pos + new Vector3(-0.05f,0.1f,-3.0f);
		
		Ray ray = new Ray(pos, Vector3.up);
		RaycastHit[] hits = Physics.SphereCastAll(ray, range);
		
		foreach(var hit in hits){
			Debug.Log(hit.collider.name);
			//OnDrawGizmos();
			if(hit.collider.CompareTag("Player")){
				PlayerStat targetStat = hit.collider.GetComponent<PlayerStat>();
				targetStat.OnSkilled(gameObject, _stat.Attack * 1.3f);
			}
		}

		yield return new WaitForSeconds(0.5f);
		
		Managers.Resource.Destroy(skillEffect1);
	}

	void EndSkill1Event(){
		State = Define.State.Idle;
		StartCoroutine(StartSkill());
	}
#endregion
#region Skill2
	IEnumerator Skill2ReadyEffect(){
		GameObject skillEffect1 = Managers.Resource.Instantiate("Effects/dark/dark_fx_15", gameObject.transform);
		yield return new WaitForSeconds(1.5f);
		Managers.Resource.Destroy(skillEffect1);
	}

	IEnumerator SetSkill2Range(){
		float startTime = 0.0f;
		float skillReadyTime = 1.3f;
		_circleRange = Managers.Resource.Instantiate("SkillRange/CircleRange");
		_fillRange = Managers.Resource.Instantiate("SkillRange/CircleRange");

		_circleRange.transform.position = transform.position;
		_fillRange.transform.position = transform.position;

		_circleRange.transform.localScale = new Vector3(_skill2Range,_skill2Range,0);
		_fillRange.transform.localScale = Vector3.zero;

		SpriteRenderer fillSprite = _fillRange.GetComponent<SpriteRenderer>();
		
		Color fillColor = fillSprite.color;
		fillColor.a = 200;
		fillColor.g = 150;
		fillSprite.color = fillColor;

		while(startTime <= skillReadyTime){
			startTime += Time.deltaTime;
			_fillRange.transform.localScale = _circleRange.transform.localScale/skillReadyTime * startTime;
			yield return new WaitForFixedUpdate();
		}
		_skill2HitRange=_circleRange.transform.lossyScale.x;
		Managers.Resource.Destroy(_circleRange);
		Managers.Resource.Destroy(_fillRange);
	}

	IEnumerator Skill2Effect(){
		List<GameObject> skillEffects = new List<GameObject>();
        GameObject skillEffect1 = Managers.Resource.Instantiate("Effects/lightning/lightning_fx_08");
		skillEffect1.transform.position = gameObject.transform.position + new Vector3(0.5f,0,-1.5f);
		skillEffect1.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
		yield return new WaitForSeconds(0.5f);
		Managers.Resource.Destroy(skillEffect1);
	}

	void Skill2Hit(){
		Ray ray = new Ray(transform.position, Vector3.up);
		RaycastHit[] hits = Physics.SphereCastAll(ray, 5.5f);
		
		foreach(var hit in hits){
			Debug.Log(hit.collider.name);
			//OnDrawGizmos();
			if(hit.collider.CompareTag("Player")){
				PlayerStat targetStat = hit.collider.GetComponent<PlayerStat>();
				targetStat.OnSkilled(gameObject, _stat.Attack * 1.5f);
			}
		}
	}

	void EndSkill2Event(){
		State = Define.State.Idle;
		StartCoroutine(StartSkill());
	}
#endregion
#region Skill3
	IEnumerator Skill3ReadyEffect(){
		GameObject skillEffect1 = Managers.Resource.Instantiate("Effects/fire/fire_fx_39", gameObject.transform);
		yield return new WaitForSeconds(1.5f);
		Managers.Resource.Destroy(skillEffect1);
	}

	IEnumerator Skill3Effect(){
		List<GameObject> skillEffects = new List<GameObject>();
        GameObject skillEffect1 =Managers.Resource.Instantiate("Effects/fire/fire_fx_50", gameObject.transform);

		yield return new WaitForSeconds(2.0f);

		Managers.Resource.Destroy(skillEffect1);
	}
	void EndSkill3Event(){
		State = Define.State.Idle;
		StartCoroutine(StartSkill());
	}
#endregion

	

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

}
