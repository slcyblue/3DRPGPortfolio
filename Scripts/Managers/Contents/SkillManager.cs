using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager
{
    //플레이어가 등록한 Skill 딕셔너리 = Skill.Skills
    public Dictionary<int, Skill> Skills { get; } = new Dictionary<int, Skill>();
    public int _emptySlot;
    Skill[] skills = new Skill[4];
    bool SkillIsFull = false;

    public void Add(Skill skill)
    {
        FindEmptySlot();

        if (!SkillIsFull)
        {
            skill.skillSlot = _emptySlot;
            switch (_emptySlot)
            {
                case 0:
                    skill.skillInput = "Q";
                    break;
                case 1:
                    skill.skillInput = "W";
                    break;
                case 2:
                    skill.skillInput = "E";
                    break;
                case 3:
                    skill.skillInput = "R";
                    break;
            }
            Skills.Add(skill.skillId, skill);
        }
        else
        {
            return;
        }
    }

    public void Add(Skill skill, int skillSlot)
    {
        skill.skillSlot = skillSlot;
        switch (skillSlot)
        {
            case 0:
                skill.skillInput = "Q";
                break;
            case 1:
                skill.skillInput = "W";
                break;
            case 2:
                skill.skillInput = "E";
                break;
            case 3:
                skill.skillInput = "R";
                break;
        }
        Skills.Add(skill.skillId, skill);
    }

    public void Remove(Skill skill)
    {
        Skills.Remove(skill.skillId);
    }

    public Skill Get(int skillId)
    {
        Skill skill = null;
        Skills.TryGetValue(skillId, out skill);
        return skill;
    }


    public Skill Find(Func<Skill, bool> condition)
    {
        foreach (Skill skill in Skills.Values)
        {
            if (condition.Invoke(skill))
                return skill;
        }

        return null;
    }

    public void FindEmptySlot()
    {
        List<Skill> skill = Managers.Skill.Skills.Values.ToList();
        skill.Sort((left, right) => { return (int)(left.skillSlot - right.skillSlot); });

        if (skill.Count >= 4)
        {
            Debug.Log("스킬목록이 꽉 찼습니다!");
            SkillIsFull = true;
            return;
        }

        skill.CopyTo(skills);

        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i] == null)
            {
                _emptySlot = i;
                break;
            }
            else if (skills[i].skillSlot != i)
            {
                _emptySlot = i;
                break;
            }
        }

        Array.Clear(skills, 0, skills.Length);
    }

    public void Clear()
    {
        Skills.Clear();
    }
}
