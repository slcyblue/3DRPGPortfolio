using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillPlate : UI_Scene{
    public List<UI_Skill_Slot> Skills{get;} = new List<UI_Skill_Slot>();
    UI_Skill_Slot _skill;
    
    public override void Init()
    {
        GameObject grid = gameObject.transform.GetChild(0).gameObject;
        foreach (Transform child in grid.transform){
            if(child == null){
                continue;
            }
            Managers.Resource.Destroy(child.gameObject);
        }

        for (int i = 0; i < 4; i++)
        {
            GameObject go = Managers.Resource.Instantiate("UI/SubIcon/UI_Skill_Slot", grid.transform);
            _skill = go.GetOrAddComponent<UI_Skill_Slot>();
            _skill.SetInputKey(i);
            Skills.Add(_skill);
        }

        RefreshUI();
    }
    
    public void RefreshUI(){
        List<Skill> skills = Managers.Skill.Skills.Values.ToList();
		skills.Sort((left, right) => { return left.skillSlot - right.skillSlot; });

		foreach (Skill skill in skills)
		{
			if (skill.skillSlot < 0 || skill.skillSlot >= 4)
				continue;

			Skills[skill.skillSlot].SetSkill(skill.skillId);
		}
    }
}
