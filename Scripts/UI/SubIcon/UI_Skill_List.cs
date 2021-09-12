using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Skill_List : UI_SkillList, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public int _skillSlot;
    public Skill _skillData;

    public Text _skillInput;
    public Color color;
    public Image _iconImage;
    public void Init(Skill skillData)
    {
        _skillData = skillData;

        Sprite icon = Managers.Resource.Load<Sprite>(_skillData.skillPath);
        _iconImage = gameObject.transform.GetChild(1).GetComponent<Image>();
        _iconImage.sprite = icon;
        color = _iconImage.color;
        color.a = 1;
        _iconImage.color = color;

        transform.GetChild(2).GetComponent<Text>().text = _skillData.skillName;
        _skillInput = transform.GetChild(3).GetComponent<Text>();
        _skillInput.text = skillData.skillInput;
        //gameObject.transform.GetChild(3).GetComponent<Text>().text = $"{_skillData.InputKey}G";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (_iconImage != null)
            {
                if (Managers.Skill.Skills.Count < 4)
                {
                    if(Managers.Skill.Get(_skillData.skillId) == null)
                        Managers.Skill.Add(_skillData);
                    else
                        Managers.UI.ShowPopupUI<UI_Alert>().SetText("이미 등록된 스킬입니다.");
                    
                    GameObject.Find("@UI_Root/UI_SkillPlate").GetComponent<UI_SkillPlate>().RefreshUI();
                }
                else
                {
                    switch (_skillData.skillSlot)
                    {
                        case 0:
                            if (!SkillController.QcoolDown)
                                SkillController.StartQSkill();
                            else
                                Debug.Log("재사용 대기중입니다");
                            break;
                        case 1:
                            if (!SkillController.WcoolDown)
                                SkillController.StartWSkill();
                            else
                                Debug.Log("재사용 대기중입니다");
                            break;
                        case 2:
                            if (!SkillController.EcoolDown)
                                SkillController.StartESkill();
                            else
                                Debug.Log("재사용 대기중입니다");
                            break;
                        case 3:
                            if (!SkillController.RcoolDown)
                                SkillController.StartRSkill();
                            else
                                Debug.Log("재사용 대기중입니다");
                            break;
                    }
                }
            }
            else
                return;
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_iconImage != null)
        {
            DragSlot.instance.skillList = this;
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
            DragSlot.instance.skillList = null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        //skillSlot -> skillList
        if (DragSlot.instance.skillSlot != null)
            ChangeSkillSlot();
        //skillList -> skillList
        else if (DragSlot.instance.skillList != null)
            return;
    }

    private void ChangeSkillSlot()
    { //Slot -> List
        if(Managers.Skill.Get(_skillData.skillId)==null){
            Managers.Skill.Skills.TryGetValue(DragSlot.instance.skillSlot._skillData.skillId, out Skill dragSkill); //get dropData

            Managers.Skill.Remove(dragSkill); //remove dragData
            Managers.Skill.Add(_skillData, DragSlot.instance.skillSlot._skillSlot); //Add dropData to dragSlot
            DragSlot.instance.skillSlot.SetSkill(_skillData.skillId);
        }else
            Managers.UI.ShowPopupUI<UI_Alert>().SetText("이미 등록된 스킬입니다.");
        
    }
}
