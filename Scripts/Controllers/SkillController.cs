using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillController : MonoBehaviour
{
    static Animator _animator;
    static public bool QcoolDown;
    static public bool WcoolDown;
    static public bool EcoolDown;
    static public bool RcoolDown;

    static GameObject player;

    private void Start() {
        player = Managers.Game.GetPlayer();
    }

    public static void StartQSkill()
    {
        Skill QSkill = Managers.Skill.Find(skill => (skill.skillSlot == 0));
        _animator = player.GetComponent<Animator>();
        if (QSkill != null)
            _animator.CrossFade($"{QSkill.skillName}", 0.1f, -1, 0);
    }
    public static void StartWSkill()
    {
        Skill WSkill = Managers.Skill.Find(skill => (skill.skillSlot == 1));
        _animator = player.GetComponent<Animator>();
        if (WSkill != null)
            _animator.CrossFade($"{WSkill.skillName}", 0.1f, -1, 0);
    }
    public static void StartESkill()
    {
        Skill ESkill = Managers.Skill.Find(skill => (skill.skillSlot == 2));
        _animator = player.GetComponent<Animator>();
        if (ESkill != null)
            _animator.CrossFade($"{ESkill.skillName}", 0.1f, -1, 0);
    }
    public static void StartRSkill()
    {
        Skill RSkill = Managers.Skill.Find(skill => (skill.skillSlot == 3));
        _animator = player.GetComponent<Animator>();
        if (RSkill != null)
            _animator.CrossFade($"{RSkill.skillName}", 0.1f, -1, 0);
    }
    #region Skill
    public IEnumerator OnDSEvent() //방패찍기 스킬 이벤트
    {
        Skill skill = Managers.Skill.Find(skill => (skill.skillName == "방패 찍기"));
        PlayerStat playerStat = player.GetComponent<PlayerStat>();
		
        Ray ray = new Ray(player.transform.position, Vector3.up);
		RaycastHit[] hits = Physics.SphereCastAll(ray, skill.skillRange);

        foreach(var hit in hits){
			if(hit.collider.CompareTag("Monster")){
				Stat targetStat = hit.collider.GetComponent<Stat>();
                targetStat.OnSkilled(gameObject, skill.skillDmg * playerStat.Attack * 0.5f);
			}
		}
        //GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        // foreach (GameObject monster in monsters)
        // {
        //     float distance = (monster.transform.position - player.transform.position).magnitude;
        //     if (distance <= skill.skillRange)
        //     {
        //         Stat targetStat = monster.GetComponent<Stat>();
        //         PlayerStat playerStat = player.GetComponent<PlayerStat>();
        //         targetStat.OnSkilled(gameObject, skill.skillDmg * playerStat.Attack * 0.5f);
        //     }
        // }

        player.GetComponent<PlayerController>().State = Define.State.Idle;

        StartCoroutine(CoolTime(skill.skillCool, skill.skillSlot));

        yield return new WaitForSeconds(skill.skillCool);
    }

    IEnumerator DSEffect(){
        List<GameObject> skillEffects = new List<GameObject>();

        GameObject skillEffect1 =Managers.Resource.Instantiate("Effects/sacred/sacred_fx_09", player.transform);

        skillEffects.Add(skillEffect1);

        yield return new WaitForSeconds(1.0f);

        foreach(var skill in skillEffects){
            Managers.Resource.Destroy(skill);
        }
    }

    public IEnumerator BattleCry() //증오의 함성 스킬 이벤트
    {
        Skill skill = Managers.Skill.Find(skill => (skill.skillName == "증오의 함성"));
        
        Ray ray = new Ray(player.transform.position, Vector3.up);
		RaycastHit[] hits = Physics.SphereCastAll(ray, skill.skillRange);
        
        List<GameObject> effectedPlayers = new List<GameObject>();
        
        foreach(var hit in hits){
			Debug.Log(hit.collider.name);
			if(hit.collider.CompareTag("Player")){
				Stat effectedStat = hit.collider.gameObject.GetComponent<Stat>();
                effectedStat.Defense = (int)(effectedStat.Defense * 1.5f);
                effectedStat.Attack = (int)(effectedStat.Attack*1.5f);
                effectedStat.MaxHp = (int)(effectedStat.MaxHp *1.5f);
                effectedStat.Hp += (int)(effectedStat.MaxHp * 1.5f - effectedStat.MaxHp);
                effectedStat.MoveSpeed = (int)(effectedStat.MoveSpeed * 1.5f);
                effectedPlayers.Add(hit.collider.gameObject);
			}
		}

        // GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
       

        // foreach (GameObject player in players)
        // {
        //     float distance = (player.transform.position - Myplayer.transform.position).magnitude;

        //     if (distance <= skill.skillRange)
        //     {
        //         Stat effectedStat = player.GetComponent<Stat>();
        //         effectedStat.Defense = (int)(effectedStat.Defense * 1.5f);
        //         effectedStat.Attack = (int)(effectedStat.Attack*1.5f);
        //         effectedStat.MaxHp = (int)(effectedStat.MaxHp *1.5f);
        //         effectedStat.Hp += (int)(effectedStat.MaxHp *1.5f);
        //         effectedStat.MoveSpeed = (int)(effectedStat.MoveSpeed * 1.5f);
        //         Util.FindGameSceneChild("UI_Status",true).GetComponent<UI_Status>().RefreshUI();
        //         effectedPlayer.Add(player);
        //     }
        // }
        //Myplayer.GetComponent<PlayerController>().State = Define.State.Idle;
        StartCoroutine(Duration(skill.skillDuration, skill));
        StartCoroutine(CoolTime(skill.skillCool, skill.skillSlot));

        yield return new WaitForSeconds(skill.skillDuration);
        
        foreach (var effectedPlayer in effectedPlayers)
        {
            Stat effectedStat = effectedPlayer.GetComponent<Stat>();
            effectedStat.Defense = (int)(effectedStat.Defense / 1.5f);
            effectedStat.Attack = (int)(effectedStat.Attack/1.5f);
            effectedStat.MaxHp = (int)(effectedStat.MaxHp /1.5f);
            effectedStat.MoveSpeed = (int)(effectedStat.MoveSpeed /1.5f);
            if(effectedStat.Hp>effectedStat.MaxHp)
                effectedStat.Hp = effectedStat.MaxHp;
        }
    }

    IEnumerator BattleCryEffect(){
        Skill skill = Managers.Skill.Find(skill => (skill.skillName == "증오의 함성"));

        GameObject skillEffect1 = Managers.Resource.Instantiate("Effects/fire/fire_fx_02", player.transform);
        
        yield return new WaitForSeconds(1.4f);
        
        Managers.Resource.Destroy(skillEffect1);

        GameObject skillEffect2 = Managers.Resource.Instantiate("Effects/fire/fire_fx_48", player.transform);
        skillEffect2.transform.position += Vector3.up * 1.5f;
        player.GetComponent<PlayerController>().State = Define.State.Idle;
        yield return new WaitForSeconds(skill.skillDuration - 1.5f);

        Managers.Resource.Destroy(skillEffect2);
    }
    #endregion

    #region CoolTime
    static IEnumerator CoolTime(float skillCool, int skillSlot)
    {
        float startTime = 0.0f;
        Image[] skillIcon = null;
        Text skillCoolTime = null;

        GameObject skillGrid = GameObject.Find("SkillGrid");
        switch (skillSlot)
        {
            case 0:
                skillIcon = skillGrid.transform.GetChild(0).GetComponentsInChildren<Image>();
                skillCoolTime = skillGrid.transform.GetChild(0).GetComponentInChildren<Text>();
                QcoolDown = true;
                break;
            case 1:
                skillIcon = skillGrid.transform.GetChild(1).GetComponentsInChildren<Image>();
                skillCoolTime = skillGrid.transform.GetChild(1).GetComponentInChildren<Text>();
                WcoolDown = true;
                break;
            case 2:
                skillIcon = skillGrid.transform.GetChild(2).GetComponentsInChildren<Image>();
                skillCoolTime = skillGrid.transform.GetChild(2).GetComponentInChildren<Text>();
                EcoolDown = true;
                break;
            case 3:
                skillIcon = skillGrid.transform.GetChild(3).GetComponentsInChildren<Image>();
                skillCoolTime = skillGrid.transform.GetChild(3).GetComponentInChildren<Text>();
                RcoolDown = true;
                break;
        }

        while (startTime < skillCool)
        {
            startTime += Time.deltaTime;

            int cool = (int)((skillCool + 0.99f) - startTime);

            if (cool > 0)
                skillCoolTime.text = $"{cool}";
            else
                skillCoolTime.text = "";

            skillIcon[1].type = Image.Type.Filled;
            skillIcon[1].fillAmount = (startTime / skillCool);
            yield return new WaitForFixedUpdate();
        }

        switch (skillSlot)
        {
            case 0:
                QcoolDown = false;
                break;
            case 1:
                WcoolDown = false;
                break;
            case 2:
                EcoolDown = false;
                break;
            case 3:
                RcoolDown = false;
                break;
        }
    }

    static IEnumerator Duration(float skillDuration, Skill skill)
    {
        GameObject buffIcon = null;
        Image _iconImage = null;
        Text _buffTime = null;
        float startTime = 0.0f;
        GameObject buffGrid = GameObject.Find("@UI_Root/UI_BuffPlate/BuffGrid");
        GameObject debuffGrid = GameObject.Find("@UI_Root/UI_BuffPlate/DebuffGrid");

        if (skill.skillType == "Buff")
        {
            buffIcon = Managers.Resource.Instantiate("UI/SubIcon/UI_BuffIcon", buffGrid.transform);
            Sprite icon = Managers.Resource.Load<Sprite>(skill.skillPath);
            _iconImage = buffIcon.transform.GetChild(1).GetComponent<Image>();
            _iconImage.sprite = icon;
            _buffTime = buffIcon.transform.GetChild(2).GetComponent<Text>();
        }
        else if (skill.skillType == "Debuff")
        {
            buffIcon = Managers.Resource.Instantiate("UI/SubIcon/UI_BuffIcon", debuffGrid.transform);
            Sprite icon = Managers.Resource.Load<Sprite>(skill.skillPath);
            _iconImage = buffIcon.transform.GetChild(1).GetComponent<Image>();
            _iconImage.sprite = icon;
            _buffTime = buffIcon.transform.GetChild(2).GetComponent<Text>();
        }

        while (startTime < skillDuration)
        {
            startTime += Time.deltaTime;

            int leftTime = (int)((skillDuration + 0.99f) - startTime);

            _buffTime.text = $"{leftTime}";

            _iconImage.type = Image.Type.Filled;
            _iconImage.fillAmount = (startTime / skillDuration);
            yield return new WaitForFixedUpdate();
        }

        Managers.Resource.Destroy(buffIcon);
    }
    #endregion
}
