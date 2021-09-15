using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : BaseController
{
    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster) | (1 << (int)Define.Layer.Npc);

    PlayerStat _stat;
    bool _stopAttack = false;
    bool _stopCast = false;
    Skill _skill;

    Animator _animator;
	Text skillText;
    public bool _stopMoving = false;
    NpcController _npc;

    UI_Inven invenUI;
    UI_Status statusUI;
    UI_SkillList skillUI;
    UI_Menu menuUI;

    public override void Init(int playerId)
    {
        WorldObjectType = Define.WorldObject.Player;
        _stat = gameObject.GetComponent<PlayerStat>();
        _animator = gameObject.GetComponent<Animator>();

        invenUI = Util.FindGameSceneChild("UI_Inven", true).GetComponent<UI_Inven>();
        statusUI = Util.FindGameSceneChild("UI_Status", true).GetComponent<UI_Status>();
        skillUI = Util.FindGameSceneChild("UI_SkillList", true).GetComponent<UI_SkillList>();
        menuUI = Util.FindGameSceneChild("UI_Menu", true).GetComponent<UI_Menu>();
		
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;

        Managers.Input.KeyAction -= OnKeyEvent;
        Managers.Input.KeyAction += OnKeyEvent;


        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);

        if (gameObject.GetComponentInChildren<UI_NamePlate>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_NamePlate>(transform);

        // if (gameObject.GetComponentInChildren<UI_CharName>() == null)
        //     Managers.UI.MakeWorldSpaceUI<UI_CharName>(transform);
        DontDestroyOnLoad(gameObject);
    }


    protected override void UpdateMoving()
    {
        // 몬스터가 내 사정거리보다 가까우면 공격
        if (_lockTarget != null)
        {
            _destPos = _lockTarget.transform.position;
            float distance = (_destPos - transform.position).magnitude;
            if (distance <= 1)
            {
                State = Define.State.Attack;
                return;
            }
        }
        if(!_stopMoving){ 
            // 이동
            Vector3 dir = _destPos - transform.position;
            //dir.y = 0;

            if (dir.magnitude < 0.5f)
            {
                State = Define.State.Idle;
            }
            else
            {
                if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
                {
                    if (Input.GetMouseButton(1) == false)
                        State = Define.State.Idle;
                    return;
                }

                float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
                transform.position += dir.normalized * moveDist;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
            }
        }else
            State = Define.State.Idle;
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

    public void OnDieEvent()
    {
        if(GameObject.Find("UI_Respawn") == null){
            Managers.UI.ShowPopupUI<UI_Respawn>().SetText("부활하시겠습니까?");
        }else
            return;
    }

    void OnHitEvent()
    {
        if (_lockTarget != null)
        {
            //락온된 상대의 스탯에 플레이어의 스탯을 보내 감소되게 함.
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            targetStat.OnAttacked(gameObject);
        }

        if (_stopAttack)
        {
            State = Define.State.Idle;
        }
        else
        {
            State = Define.State.Attack;
        }
    }


    void OnMouseEvent(Define.MouseEvent evt)
    {
        switch (State)
        {
            case Define.State.Idle:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Moving:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Attack:
                {
                    if (evt == Define.MouseEvent.PointerUp)
                        _stopAttack = true;
                }
                break;
            case Define.State.Skill:
                _stopCast = true;
                OnMouseEvent_IdleRun(evt);
                break;
        }
    }

    void OnMouseEvent_IdleRun(Define.MouseEvent evt)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mask);
        //Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        switch (evt)
        {
            case Define.MouseEvent.PointerDown:
                {
                    if (raycastHit)
                    {
                        _destPos = hit.point;
                        if(_stopMoving!=true)
                            State = Define.State.Moving;
                        else
                            State = Define.State.Idle;
                        _stopAttack = false;

                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster){
                            _destPos.y = 0;
                            _lockTarget = hit.collider.gameObject;
                        }
                        else if (hit.collider.gameObject.layer == (int)Define.Layer.Npc)
                        {
                            _destPos.y = 0;
                            GameObject npc = hit.collider.gameObject;
                            _npc = npc.GetComponent<NpcController>();

                            if ((npc.transform.position - transform.position).magnitude < 3)
                                _npc.ShowUI(gameObject);
                            else
                                Debug.Log("조금 더 가까이 가야합니다");
                        }
                        else
                            _lockTarget = null;
                    }
                }
                break;
            case Define.MouseEvent.Press:
                {   
                    if (_lockTarget == null && raycastHit){
                        _destPos = hit.point;
                        
                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster){
                            _destPos.y = 0;
                        }
                        else if (hit.collider.gameObject.layer == (int)Define.Layer.Npc)
                        {
                            _destPos.y = 0;
                        }
                    }
                }
                break;
            case Define.MouseEvent.PointerUp:
                _stopAttack = true;
                break;
        }
    }

    void OnKeyEvent(Define.KeyEvent evt)
    {
        OnKeyEvent_UIEvent(evt);
        //각 상태에서 KeyEvent가 실행될 경우
        switch (State)
        {
            case Define.State.Idle:
                OnKeyEvent_StartSkill(evt);
                break;
            case Define.State.Moving:
                State = Define.State.Idle;
                OnKeyEvent_StartSkill(evt);
                break;
            case Define.State.Attack:
                State = Define.State.Idle;
                //애매함.
                {
                    if (evt == Define.KeyEvent.KeyUp)
                        _stopCast = true;
                }
                break;
            case Define.State.Skill:
                {
                    if (evt == Define.KeyEvent.KeyUp)
                        _stopCast = true;
                }
                break;
        }
    }

    void OnKeyEvent_StartSkill(Define.KeyEvent evt)
    {
        switch (evt)
        {
            case Define.KeyEvent.KeyDown:
                if (Input.GetKey(KeyCode.Q))
                {
                    if (SkillController.QcoolDown == true)
                    {
                        Debug.Log("쿨타임 대기중입니다");
                        break;
                    }
                    else
                    {
                        State = Define.State.Skill;
                        SkillController.StartQSkill();
                    }
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    if (SkillController.WcoolDown == true)
                    {
                        Debug.Log("쿨타임 대기중입니다");
                        break;
                    }
                    else
                    {
                        State = Define.State.Skill;
                        SkillController.StartWSkill();
                    }
                }
                else if (Input.GetKey(KeyCode.E))
                {
                    if (SkillController.EcoolDown == true)
                    {
                        Debug.Log("쿨타임 대기중입니다");
                        break;
                    }
                    else
                    {
                        State = Define.State.Skill;
                        SkillController.StartESkill();
                    }
                }
                else if (Input.GetKey(KeyCode.R))
                {
                    if (SkillController.RcoolDown == true)
                    {
                        Debug.Log("쿨타임 대기중입니다");
                        break;
                    }
                    else
                    {
                        State = Define.State.Skill;
                        SkillController.StartRSkill();
                    }
                }
                break;
            case Define.KeyEvent.Press:
                //Charge Skill TODO
                break;
            case Define.KeyEvent.KeyUp:
                _stopCast = true;
                break;
        }
    }

    void OnKeyEvent_UIEvent(Define.KeyEvent evt)
    {
        if (evt == Define.KeyEvent.KeyDown)
        {
            if (Input.GetKey(KeyCode.I))
            {
                if (invenUI.gameObject.activeSelf == false)
                {
                    invenUI.RefreshUI();
                    invenUI.transform.SetAsLastSibling();
                    invenUI.gameObject.SetActive(true);
                    _stopMoving = true;
                }
                else{
                    invenUI.gameObject.SetActive(false);
                    _stopMoving = false;
                }
            }
            else if (Input.GetKey(KeyCode.C))
            {
                if (statusUI.gameObject.activeSelf == false)
                {
                    statusUI.RefreshUI();
                    statusUI.transform.SetAsLastSibling();
                    statusUI.gameObject.SetActive(true);
                    _stopMoving = true;
                }
                else{
                    statusUI.gameObject.SetActive(false);
                    _stopMoving = false;
                }
                    
            }else if(Input.GetKey(KeyCode.K)){
                if(skillUI.gameObject.activeSelf == false){
                    skillUI.transform.SetAsLastSibling();
                    skillUI.gameObject.SetActive(true);
                    _stopMoving = true;
                }
                else{
                    skillUI.gameObject.SetActive(false);
                    _stopMoving = false;
                }
            }else if(Input.GetKey(KeyCode.Escape)){
                GameObject gameUI = GameObject.Find("@UI_Root/UI_GameScene");
                bool checkUIActive = false;
                for(int i = 5; i<0;i--){
                    if(gameUI.transform.GetChild(i).gameObject.activeSelf == true){
                        gameUI.transform.GetChild(i).gameObject.SetActive(false);
                        checkUIActive = true;
                        break;
                    }
                }

                if(!checkUIActive){
                    if(_lockTarget != null){
                        _lockTarget = null;
                    }else{
                        if(menuUI.gameObject.activeSelf==false){
                            menuUI.transform.SetAsLastSibling();
                            menuUI.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
    }
}
