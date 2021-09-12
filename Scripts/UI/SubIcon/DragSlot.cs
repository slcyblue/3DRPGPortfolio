using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
	static public DragSlot instance;
	public UI_Inven_Item invenSlot;
	public UI_Equip_Item equipSlot;
	public UI_Shop_Item shopSlot;
	public UI_Enhance_Item enhanceSlot;
	public UI_Skill_Slot skillSlot;
	public UI_Skill_List skillList;
	public Image iconImage = null;

	public void Start()
	{
		instance=this;
	}

	public void DragSetImage(Image _iconImage)
    {
		iconImage = gameObject.GetComponent<Image>();
        iconImage.sprite = _iconImage.sprite;
		SetColor(1);
    }

	public void SetColor(float _alpha)
    {
        Color color = iconImage.color;
        color.a = _alpha;
        iconImage.color = color;
    }
	
}
