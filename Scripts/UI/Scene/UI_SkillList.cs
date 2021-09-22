using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

public class UI_SkillList : UI_Base
{
    public List<UI_Skill_List> Skills{get;} = new List<UI_Skill_List>();
    UI_Skill_List _slist;
    List<Data.Skill> skillList;
    public int _count;
    PlayerController player;
    public override void Init()
    {
       
    }

#region SetItem
    public void SetSkillGrid()
    {
        Skills.Clear();
        player = Managers.Game.GetPlayer().GetComponent<PlayerController>();

        skillList = Managers.Data.SkillDict.Values.ToList();
        GameObject skillUI = gameObject.transform.GetChild(1).gameObject;
        GameObject grid = skillUI.transform.GetChild(0).transform.GetChild(0).gameObject;
        foreach (Transform child in grid.transform){
            if(child == null){
                continue;
            }
            Managers.Resource.Destroy(child.gameObject);
        }

        for (int i = 0; i < skillList.Count; i++)
        {
            GameObject go = Managers.Resource.Instantiate("UI/SubIcon/UI_Skill_List", grid.transform);
            _slist = go.GetOrAddComponent<UI_Skill_List>();
        
            Skill skill = Skill.MakeSkill(skillList[i].skillId);
            _slist.Init(skill);
            Skills.Add(_slist);
        }
    }
    
    public void OnClickQuit(){
        player._stopMoving = false;
        gameObject.SetActive(false);
    }

    #endregion
}
