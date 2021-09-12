using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill
{
    protected int _skillId;
    protected string _skillName;
    protected string _skillType;
    protected string _skillAnim;
    protected string _skillPath;
    protected string _skillInput;
    protected int _skillDmg;
    protected int _skillMp;
    protected int _skillCool;
    protected int _skillSlot;
    protected float _skillRange;
    protected float _skillDuration;

    public int skillId { get { return _skillId; } set { _skillId = value; } }
    public string skillName { get { return _skillName; } set { _skillName = value; } }
    public string skillType { get { return _skillType; } set { _skillType = value; } }
    public string skillAnim { get { return _skillAnim; } set { _skillAnim = value; } }
    public string skillPath { get { return _skillPath; } set { _skillPath = value; } }
    public string skillInput { get { return _skillInput; } set { _skillInput = value; } }
    public int skillDmg { get { return _skillDmg; } set { _skillDmg = value; } }
    public int skillMp { get { return _skillMp; } set { _skillMp = value; } }
    public int skillCool { get { return _skillCool; } set { _skillCool = value; } }
    public int skillSlot { get { return _skillSlot; } set { _skillSlot = value; } }
    public float skillRange { get { return _skillRange; } set { _skillRange = value; } }
    public float skillDuration { get { return _skillDuration; } set { _skillDuration = value; } }



    public static Skill MakeSkill(int skillId){
        Skill skill = null;
		Data.Skill skillData = null;

		Managers.Data.SkillDict.TryGetValue(skillId, out skillData);

		if (skillData == null)
			return null;

        skill = new Skill();


		if (skill != null){
            skill._skillId = skillData.skillId;
            skill._skillName = skillData.skillName;
            skill._skillType = skillData.skillType;
            skill._skillDmg = skillData.skillDmg;
            skill._skillMp = skillData.skillMp;
            skill._skillCool = skillData.skillCool;
            skill._skillSlot = skillData.skillSlot;
            skill._skillPath = skillData.skillPath;
            skill._skillRange = skillData.skillRange;
            skill._skillInput = skillData.skillInput;
            skill._skillDuration = skillData.skillDuration;
        }

        return skill;
    }
}
