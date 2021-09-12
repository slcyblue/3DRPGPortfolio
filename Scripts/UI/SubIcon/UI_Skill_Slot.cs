using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Skill_Slot : UI_SkillPlate, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public int _skillSlot;
    public Skill _skillData;
    GameObject _invenUI;
    Text _skillInput;
    Color color;
    public Image _iconImage;

    public override void Init()
    {
        _invenUI = GameObject.Find("@UI_Root/UI_GameScene/UI_Inven");
    }

    public void SetSkill(int skillId)
    {
        Managers.Skill.Skills.TryGetValue(skillId, out _skillData);
        
        Sprite icon = Managers.Resource.Load<Sprite>(_skillData.skillPath);
        _iconImage = gameObject.transform.GetChild(1).GetComponent<Image>();
        _iconImage.sprite = icon;
        color = _iconImage.color;
        color.a = 1;
        _iconImage.color = color;

        _skillData.skillInput = _skillInput.text;
    }

    public void SetInputKey(int index){
        _skillInput = transform.GetChild(3).GetComponent<Text>();
        switch(index){
            case 0:
                _skillInput.text = "Q";
                break;
            case 1:
                _skillInput.text = "W";
                break;
            case 2:
                _skillInput.text = "E";
                break;
            case 3:
                _skillInput.text = "R";
                break;
        }
        _skillSlot = index;
    }
    public void ClearSlot()
    {
        _iconImage.sprite = null;
        color = _iconImage.color;
        color.a = 0;
        _iconImage.color = color;

        _skillData = null;
        _iconImage = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right){
            if(_skillData != null){
                switch(_skillData.skillSlot){
                case 0:
                    if(!SkillController.QcoolDown)
                        SkillController.StartQSkill();
                    else
                        Debug.Log("재사용 대기중입니다");
                    break;
                case 1:
                    if(!SkillController.WcoolDown)
                        SkillController.StartWSkill();
                    else
                        Debug.Log("재사용 대기중입니다");
                    break;
                case 2:
                    if(!SkillController.EcoolDown)
                        SkillController.StartESkill();
                    else
                        Debug.Log("재사용 대기중입니다");
                    break;
                case 3:
                    if(!SkillController.RcoolDown)
                        SkillController.StartRSkill();
                    else
                        Debug.Log("재사용 대기중입니다");
                    break;
                }
            }else
                return;
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_iconImage != null)
        {
            DragSlot.instance.skillSlot = this;
            DragSlot.instance.DragSetImage(_iconImage);
            DragSlot.instance.transform.position = eventData.position;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (_iconImage != null)
            DragSlot.instance.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(DragSlot.instance.iconImage != null){
            DragSlot.instance.SetColor(0);
            DragSlot.instance.skillSlot = null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        //skillSlot -> skillSlot
        if (DragSlot.instance.skillSlot != null)
            ChangeSkillSlot();
        //skillSlot -> skillSlot
        else if(DragSlot.instance.skillList != null)
            ChangeSkillList();
    }

    private void ChangeSkillSlot(){ //Slot -> Slot
        Image _tempImage = _iconImage;
        Skill _tempSkill = _skillData;

        if (DragSlot.instance.skillSlot._skillSlot == _skillSlot)
        {
            return;
        }
        else
        {
            SetSkill(DragSlot.instance.skillSlot._skillData.skillId); //1번에 0번 스킬 등록

            if (_tempImage != null)
            {
                DragSlot.instance.skillSlot.SetSkill(_tempSkill.skillId); //0번에 1번스킬 등록

                //DB Update
                Managers.Skill.Skills.TryGetValue(_skillData.skillId, out Skill dragSkill); //get dragData
                Managers.Skill.Skills.TryGetValue(DragSlot.instance.skillSlot._skillData.skillId, out Skill dropSkill); //get dropData

                Managers.Skill.Remove(dragSkill); //remove dragData
                Managers.Skill.Remove(dropSkill); //remove dropData

                Managers.Skill.Add(dragSkill, _skillSlot); //Add dragData to dropSlot index
                Managers.Skill.Add(dropSkill, DragSlot.instance.skillSlot._skillSlot); //Add dropData to dragSlot
            }
            else
            {
                Managers.Skill.Skills.TryGetValue(DragSlot.instance.skillSlot._skillData.skillId, out Skill dragSkill); //get dragData
                
                Managers.Skill.Remove(dragSkill); //Remove dragData
                Managers.Skill.Add(dragSkill, _skillSlot); //Add dragData to dropSlot index
                
                DragSlot.instance.skillSlot.ClearSlot();
            }
        }
    }
    private void ChangeSkillList(){ //List -> Slot
        Image _tempImage = _iconImage;
        Skill _tempSkill = _skillData;

        if (_tempImage != null)
        {
            if(Managers.Skill.Get(DragSlot.instance.skillList._skillData.skillId)==null){
                //DB Update
                Managers.Skill.Skills.TryGetValue(_skillData.skillId, out Skill dropSkill); //get dropData

                Managers.Skill.Remove(dropSkill); //remove dropData

                Managers.Skill.Add(DragSlot.instance.skillList._skillData, _skillSlot); //Add dragData to dropSlot index
                
                SetSkill(DragSlot.instance.skillList._skillData.skillId); //1번에 0번 스킬 등록
            }else
                Managers.UI.ShowPopupUI<UI_Alert>().SetText("이미 등록된 스킬입니다.");
            
        }
        else
        {
            Managers.Skill.Add(DragSlot.instance.skillList._skillData, _skillSlot); //Add dragData to dropSlot index
            GameObject.Find("@UI_Root/UI_SkillPlate").GetComponent<UI_SkillPlate>().RefreshUI();
        }
    }
}
