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

    Coroutine _coDruation;

    public static void StartQSkill()
    {
        Skill QSkill = Managers.Skill.Find(skill => (skill.skillSlot == 0));
        _animator = Managers.Game.GetPlayer().GetComponent<Animator>();
        if (QSkill != null)
            _animator.CrossFade($"{QSkill.skillName}", 0.1f, -1, 0);
    }
    public static void StartWSkill()
    {
        Skill WSkill = Managers.Skill.Find(skill => (skill.skillSlot == 1));
        _animator = Managers.Game.GetPlayer().GetComponent<Animator>();
        if (WSkill != null)
            _animator.CrossFade($"{WSkill.skillName}", 0.1f, -1, 0);
    }
    public static void StartESkill()
    {
        Skill ESkill = Managers.Skill.Find(skill => (skill.skillSlot == 2));
        _animator = Managers.Game.GetPlayer().GetComponent<Animator>();
        if (ESkill != null)
            _animator.CrossFade($"{ESkill.skillName}", 0.1f, -1, 0);
    }
    public static void StartRSkill()
    {
        Skill RSkill = Managers.Skill.Find(skill => (skill.skillSlot == 3));
        _animator = Managers.Game.GetPlayer().GetComponent<Animator>();
        if (RSkill != null)
            _animator.CrossFade($"{RSkill.skillName}", 0.1f, -1, 0);
    }
    #region Skill
    public IEnumerator OnDSEvent() //방패찍기 스킬 이벤트
    {
        Skill skill = Managers.Skill.Find(skill => (skill.skillName == "방패 찍기"));

        GameObject player = Managers.Game.GetPlayer();
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        List<GameObject> skillEffects = new List<GameObject>();

        GameObject skillEffect1 =Managers.Resource.Instantiate("Effects/sacred/sacred_fx_11", player.transform);

        skillEffects.Add(skillEffect1);

        foreach (GameObject monster in monsters)
        {
            float distance = (monster.transform.position - player.transform.position).magnitude;
            if (distance <= skill.skillRange)
            {
                Stat targetStat = monster.GetComponent<Stat>();
                PlayerStat playerStat = player.GetComponent<PlayerStat>();
                targetStat.OnSkilled(gameObject, skill.skillDmg * playerStat.Attack * 0.5f);
            }
        }
        Managers.Game.GetPlayer().GetComponent<PlayerController>().State = Define.State.Idle;

        StartCoroutine(CoolTime(skill.skillCool, skill.skillSlot));

        yield return new WaitForSeconds(skill.skillCool);

        foreach(GameObject skillEffect in skillEffects){
            Managers.Resource.Destroy(skillEffect);
        }
    }

    public IEnumerator BattleCry() //증오의 함성 스킬 이벤트
    {
        Skill skill = Managers.Skill.Find(skill => (skill.skillName == "증오의 함성"));
        

        GameObject Myplayer = Managers.Game.GetPlayer();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        List<GameObject> effectedPlayer = new List<GameObject>();
        List<GameObject> skillEffects = new List<GameObject>();

        GameObject skillEffect1 = Managers.Resource.Instantiate("Effects/fire/fire_fx_02", Myplayer.transform);
        GameObject skillEffect2 = Managers.Resource.Instantiate("Effects/fire/fire_fx_48", Myplayer.transform);
        skillEffect2.transform.position += Vector3.up;

        skillEffects.Add(skillEffect1);
        skillEffects.Add(skillEffect2);

        foreach (GameObject player in players)
        {
            float distance = (player.transform.position - Myplayer.transform.position).magnitude;

            if (distance <= skill.skillRange)
            {
                Stat effectedStat = player.GetComponent<Stat>();
                effectedStat.Defense = (int)(effectedStat.Defense * 1.5f);
                effectedStat.Attack = (int)(effectedStat.Attack*1.5f);
                effectedStat.MaxHp = (int)(effectedStat.MaxHp *1.5f);
                effectedStat.Hp += (int)(effectedStat.MaxHp *1.5f);
                effectedStat.MoveSpeed = (int)(effectedStat.MoveSpeed * 1.5f);
                Util.FindGameSceneChild("UI_Status",true).GetComponent<UI_Status>().RefreshUI();
                effectedPlayer.Add(player);
            }
        }
        Myplayer.GetComponent<PlayerController>().State = Define.State.Idle;
        StartCoroutine(Duration(skill.skillDuration, skill));
        StartCoroutine(CoolTime(skill.skillCool, skill.skillSlot));

        yield return new WaitForSeconds(skill.skillDuration);
        
        foreach(GameObject skillEffect in skillEffects){
            Managers.Resource.Destroy(skillEffect);
        }

        foreach (var player in effectedPlayer)
        {
            Stat effectedStat = player.GetComponent<Stat>();
            effectedStat.Defense = (int)(effectedStat.Defense / 1.5f);
            effectedStat.Attack = (int)(effectedStat.Attack/1.5f);
            effectedStat.MaxHp = (int)(effectedStat.MaxHp /1.5f);
            effectedStat.MoveSpeed = (int)(effectedStat.MoveSpeed /1.5f);
            if(effectedStat.Hp>effectedStat.MaxHp)
                effectedStat.Hp = effectedStat.MaxHp;

            Util.FindGameSceneChild("UI_Status",true).GetComponent<UI_Status>().RefreshUI();
        }

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
