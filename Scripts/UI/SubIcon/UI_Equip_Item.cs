using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Equip_Item : UI_Status, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
	public Item _itemData;
	public Image _iconImage = null;
    public string _itemType;
    public bool isOver = false;
    Color color;
    PlayerStat _playerStat;
    UI_Status_Text statsText;

	public override void Init()
	{
        statsText = transform.parent.transform.GetChild(1).gameObject.GetOrAddComponent<UI_Status_Text>();
        _playerStat = Managers.Game.GetPlayer().GetComponent<PlayerStat>();
	}

	public void SetItem(string itemType)
	{
		//Text _itemCount = gameObject.GetComponent<Text>();
		Managers.Equip.Items.TryGetValue(itemType, out _itemData);

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
        _iconImage.sprite = null;
        color = _iconImage.color;
        color.a = 0;
        _iconImage.color = color;

        _itemData = null;
        _iconImage = null;
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
                        Managers.Equip.Items.TryGetValue(_itemType, out Item clickItem);
                        Managers.Equip.Remove(clickItem);
                        Managers.Inven.Add(clickItem);
                        
                        ClearSlot();
                        UI_Inven go = GameObject.Find("@UI_Root/UI_GameScene/UI_Inven").GetComponent<UI_Inven>();
                        go.Items[clickItem.itemSlot].SetItem(clickItem.itemSlot);

                        _playerStat.AbsEquipStat(clickItem);
                        statsText.SetText(_playerStat);
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
            DragSlot.instance.equipSlot = this;
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
            DragSlot.instance.equipSlot = null;
        }
    }


// 해당 슬롯에 무언가가 마우스 드롭 됐을 때 발생하는 이벤트
   public void OnDrop(PointerEventData eventData)
    {
        //inven -> equip
        if (DragSlot.instance.invenSlot != null)
            ChangeInvenSlot();
        //equip -> equip
        else if(DragSlot.instance.equipSlot != null)
            ChangeEquipSlot();
        else if(DragSlot.instance.shopSlot != null)
            ChangeShopSlot();
    }
    
    private void ChangeInvenSlot() //inven(drag) -> equip(drop)
    {
        Image _tempImage = _iconImage;
        //ItemType Check
        if(_itemType.Substring(0, _itemType.Length-1) == DragSlot.instance.invenSlot._itemData.itemType.Substring(0,DragSlot.instance.invenSlot._itemData.itemType.Length-1)){
            if (_tempImage != null){ //having Data
                if(_itemType == DragSlot.instance.invenSlot._itemData.itemType){
                //DB Update  
                Managers.Inven.Items.TryGetValue(DragSlot.instance.invenSlot._itemSlot, out Item dragItem);  //get dragData
                Managers.Equip.Items.TryGetValue(_itemData.itemType, out Item dropItem); //get dropData

                Managers.Inven.Remove(dragItem); //remove dragData
                Managers.Equip.Remove(dropItem); //remove dropData

                Managers.Equip.Add(dragItem); //Add dragData to equipSlot with itemId
                Managers.Inven.Add(dropItem, DragSlot.instance.invenSlot._itemSlot); //Add dropData to dragSlot

                DragSlot.instance.invenSlot.SetItem(DragSlot.instance.invenSlot._itemSlot);
                SetItem(DragSlot.instance.invenSlot._itemData.itemType);

                _playerStat.AbsEquipStat(dropItem);
                _playerStat.PlusEquipStat(dragItem);
                statsText.SetText(_playerStat);
                }else
                    Debug.Log("아이템 형식이 다릅니다");
            }
            else{ //nonData
                Managers.Inven.Items.TryGetValue(DragSlot.instance.invenSlot._itemSlot, out Item dragItem); //get dragData
                Managers.Inven.Remove(dragItem); //Remove dragData
                Managers.Equip.Add(dragItem); //Add dragData to dropSlot index

                SetItem(_itemType); //set dragSlot with dragSlot index
                DragSlot.instance.invenSlot.ClearSlot(); //clear dragSlot

                _playerStat.PlusEquipStat(dragItem);
                statsText.SetText(_playerStat);
            }
        }else
            Debug.Log("아이템 형식이 다릅니다");
    }

    private void ChangeEquipSlot() //equip -> equip
    {
        Image _tempImage = _iconImage;
        if(_itemType.Substring(0, _itemType.Length-1) == DragSlot.instance.invenSlot._itemData.itemType.Substring(0,DragSlot.instance.invenSlot._itemData.itemType.Length-1)){
            if(this == DragSlot.instance.equipSlot){
                return;
            }else if (_tempImage != null){
                SetItem(DragSlot.instance.equipSlot._itemData.itemType); //Set DragSlot to EquipItem
                DragSlot.instance.equipSlot.SetItem(_itemData.itemType); //Set EquipSlot to DragItem
                
                //DB Update
                Managers.Equip.Items.TryGetValue(DragSlot.instance.equipSlot._itemData.itemType, out Item dragItem); //get dragData
                Managers.Equip.Items.TryGetValue(_itemData.itemType, out Item dropItem); //get dropData

                Managers.Equip.Remove(dragItem); //remove dragData
                Managers.Equip.Remove(dropItem); //remove dropData

                Managers.Equip.Add(dragItem); //Add dragData to dropSlot index
                Managers.Equip.Add(dropItem); //Add dropData to dragSlot
            }else{
                SetItem(DragSlot.instance.equipSlot._itemData.itemType);
                DragSlot.instance.equipSlot.ClearSlot(); //set dragSlot with dragSlot index

                Managers.Equip.Items.TryGetValue(DragSlot.instance.equipSlot._itemData.itemType, out Item dragitem); //get dragData
                Managers.Equip.Remove(dragitem); //Remove dragData
                Managers.Equip.Add(dragitem); //Add dragData to dropSlot index
            }
        }else
            Debug.Log("아이템 형식이 다릅니다");
    }

    private void ChangeShopSlot(){
        return;
    }
#endregion
}
