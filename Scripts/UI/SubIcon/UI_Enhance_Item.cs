using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Enhance_Item : UI_Enhance, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
	public Item _itemData = null;
	public Image _iconImage = null;
    public bool isOver = false;
    Color color;
    GameObject _invenUI;

	public override void Init()
	{
        _invenUI = Util.FindGameSceneChild("UI_Inven", true);
	}

    public void SetItem()
	{
        transform.parent.GetComponent<UI_Enhance>()._enhanceItem = _itemData;
        _iconImage = transform.GetChild(1).GetComponent<Image>();
        Sprite icon = Managers.Resource.Load<Sprite>(_itemData.itemPath);
        _iconImage.sprite = icon;
        color = _iconImage.color;
        color.a = 1;
        _iconImage.color = color;
		//gameObject.GetComponentInChildren<Text>().text = $"{count}";
	}

	public void ClearSlot()
	{
        if(_iconImage!=null){
            transform.parent.GetComponent<UI_Enhance>()._enhanceItem = null;
            _iconImage.sprite = null;
            color = _iconImage.color;
            color.a = 0;
            _iconImage.color = color;

            _itemData = null;
            _iconImage = null;
        }
	}
	
#region UIEvent
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if (_iconImage != null)
            {
                if(_itemData.itemTemplate == Define.ItemTemplate.Equipment.ToString())
                {
                    if(Managers.Inven.Items.Count<=24){
                        //아이템 해제
                        Managers.Inven.Add(_itemData);
                        
                        ClearSlot();
                        _invenUI.GetComponent<UI_Inven>().RefreshUI();
                        transform.parent.GetComponent<UI_Enhance>().RefreshUI();
                    }else{
                        Debug.Log("가방이 가득 찼습니다");
                    }
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isOver = true;
        //todo
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOver = false;
        //todo
    }

       // 마우스 드래그가 시작 됐을 때 발생하는 이벤트
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(_iconImage != null)
        {
            DragSlot.instance.enhanceSlot = this;
            DragSlot.instance.DragSetImage(_iconImage);
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    // 마우스 드래그 중일 때 계속 발생하는 이벤트
    public void OnDrag(PointerEventData eventData)
    {
        if (_iconImage != null)
            DragSlot.instance.transform.position = eventData.position;
    }

    // 마우스 드래그가 끝났을 때 발생하는 이벤트
    public void OnEndDrag(PointerEventData eventData)
    {
		if(DragSlot.instance.iconImage != null){
            DragSlot.instance.SetColor(0);
            DragSlot.instance.iconImage = null;
            DragSlot.instance.enhanceSlot = null;
        }
    }


// 해당 슬롯에 무언가가 마우스 드롭 됐을 때 발생하는 이벤트
   public void OnDrop(PointerEventData eventData)
    {
        //inven -> enhance
        if (DragSlot.instance.invenSlot != null)
            ChangeInvenSlot();
        //equip -> enhance
        else if(DragSlot.instance.equipSlot != null)
            ChangeEquipSlot();
    }
    
    private void ChangeInvenSlot() //inven(drag) -> enhance(drop)
    {
        Image _tempImage = _iconImage;
        //ItemType Check
        
            if (_tempImage != null){ //having Data
                if(DragSlot.instance.invenSlot._itemData.itemTemplate == Define.ItemTemplate.Equipment.ToString()){
                    //DB Update  
                    Managers.Inven.Items.TryGetValue(DragSlot.instance.invenSlot._itemSlot, out Item dragItem);  //get dragData
                    Item dropItem = _itemData;
                    _itemData = dragItem;
                    Managers.Inven.Remove(dragItem); //remove dragData

                    Managers.Inven.Add(dropItem, DragSlot.instance.invenSlot._itemSlot); //Add dropData to dragSlot

                    DragSlot.instance.invenSlot.SetItem(DragSlot.instance.invenSlot._itemSlot);
                    SetItem();
                    transform.parent.GetComponent<UI_Enhance>().RefreshUI();
                }else
                    Debug.Log("아이템 형식이 다릅니다");
            }
            else{ //nonData
                if(DragSlot.instance.invenSlot._itemData.itemTemplate == Define.ItemTemplate.Equipment.ToString()){
                    //DB Update  
                    Managers.Inven.Items.TryGetValue(DragSlot.instance.invenSlot._itemSlot, out Item dragItem);  //get dragData
                    Managers.Inven.Remove(dragItem); //remove dragData
                    _itemData = dragItem;
                    SetItem();
                    DragSlot.instance.invenSlot.ClearSlot();
                    transform.parent.GetComponent<UI_Enhance>().RefreshUI();
                }
                else
                    Debug.Log("아이템 형식이 다릅니다");
            }
    }

    private void ChangeEquipSlot() //equip -> enhance
    {
        Debug.Log("착용중인 장비는 강화할 수 없습니다");
        return;
    }    
#endregion
}
