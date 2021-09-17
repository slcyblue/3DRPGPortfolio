using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Shop_Item : UI_Shop, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item _itemData;
    public Image _iconImage = null;
    public bool _isOver = false;
    bool _isClicked = false;
    PlayerStat _playerStat;
    Color color;
    Sprite icon;


    public void Init(Item itemData)
    {
        _itemData = itemData;
        _playerStat = Managers.Game.GetPlayer().GetComponent<PlayerStat>();

        icon = Managers.Resource.Load<Sprite>(_itemData.itemPath);
        _iconImage = gameObject.transform.GetChild(1).GetComponent<Image>();
        _iconImage.sprite = icon;
        color = _iconImage.color;
        color.a = 1;
        _iconImage.color = color;

        gameObject.transform.GetChild(2).GetComponent<Text>().text = _itemData.itemName;
        gameObject.transform.GetChild(3).GetComponent<Text>().text = $"{_itemData.itemPrice}G";
        //gameObject.GetComponentInChildren<Text>().text = $"{count}";
    }


    #region UIEvent
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (_iconImage != null)
            {
                if (_itemData.itemTemplate == Define.ItemTemplate.Equipment.ToString())
                {
                    //장비 구매
                    if (_playerStat.Gold <= 0 || _playerStat.Gold - _itemData.itemPrice < 0)
                    {
                        Managers.UI.ShowPopupUI<UI_Alert>("UI_Alert").SetText("소지금이 부족합니다");
                    }
                    else
                    {
                        UI_SetCount setCount = Managers.UI.ShowPopupUI<UI_SetCount>("UI_SetCount");
                        setCount.shopItem = this;
                    }
                }
                else if (_itemData.itemTemplate == Define.ItemTemplate.Consumable.ToString())
                {
                    //물약 구매
                    if (_playerStat.Gold <= 0 || _playerStat.Gold - _itemData.itemPrice < 0)
                    {
                        Managers.UI.ShowPopupUI<UI_Alert>("UI_Alert").SetText("소지금이 부족합니다");
                    }
                    else
                    {
                        UI_SetCount setCount = Managers.UI.ShowPopupUI<UI_SetCount>("UI_SetCount");
                        setCount.shopItem = this;
                    }
                }
                else
                {
                    return;
                }
            }
            // }else if(eventData.button == PointerEventData.InputButton.Left){
            //     if(_isClicked == false){
            //         icon = Managers.Resource.Load<Sprite>("Textures/Items/ClickedShopItem");
            //         _iconImage.sprite = icon;
            //         _isClicked = true;
            //     }else{
            //         icon = Managers.Resource.Load<Sprite>("Textures/Items/BasicShopItem");
            //         _iconImage.sprite = icon;
            //         _isClicked = false;
            //     }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _isOver = true;
        //todo
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isOver = false;
        //todo
    }

    // 마우스 드래그가 시작 됐을 때 발생하는 이벤트
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_iconImage != null)
        {
            DragSlot.instance.shopSlot = this;
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
        if (DragSlot.instance.iconImage != null)
        {
            DragSlot.instance.SetColor(0);
            DragSlot.instance.shopSlot = null;
        }
    }


    // 해당 슬롯에 무언가가 마우스 드롭 됐을 때 발생하는 이벤트
    public void OnDrop(PointerEventData eventData)
    {
        //inven -> shop
        if (DragSlot.instance.invenSlot != null)
            ChangeInvenSlot();
        //equip -> shop
        else if (DragSlot.instance.equipSlot != null)
            ChangeEquipSlot();
        //shop -> shop
        else if (DragSlot.instance.shopSlot != null)
            ChangeShopSlot();
    }

    private void ChangeInvenSlot() //inven -> shop : Sell
    {
        Image _tempImage = _iconImage;
        //if(DragSlot.instance.invenSlot._itemSlot == _itemSlot){  //TODO 거래불가 아이템 체크
        //    return;
        //}else{
        DragSlot.instance.invenSlot.ClearSlot(); //set dragSlot with dragSlot index

        Managers.Inven.Items.TryGetValue(DragSlot.instance.invenSlot._itemSlot, out Item dragItem); //get dragData
        Managers.Inven.Remove(dragItem); //Remove dragData
        _playerStat.Gold += (int)(dragItem.itemPrice * 0.5f);
        Managers.UI.ShowPopupUI<UI_Alert>("UI_Alert").SetText($"{dragItem.itemName}을 팔았습니다 : {(int)(dragItem.itemPrice * 0.5f)}");
        //}
    }

    private void ChangeEquipSlot()
    {  //equip->shop
        return;
    }

    private void ChangeShopSlot()
    {  //shop -> shop
        return;
    }
    #endregion
    public void purchase(int count)
    {
        if (_playerStat.Gold - (_itemData.itemPrice * count) >= 0)
        {
            if (count < 56 - Managers.Inven.Items.Count)
            {
                for (int i = 0; i < count; i++)
                {
                    Item newItem = Item.MakeItem(_itemData.itemId);

                    if (newItem.itemTemplate == Define.ItemTemplate.Equipment.ToString())
                    {
                        Managers.Inven.Add(newItem);
                    }
                    else if (newItem.itemTemplate == Define.ItemTemplate.Consumable.ToString())
                    {
                        Item potion = Managers.Inven.Find(item => (item.itemId == newItem.itemId));
                        if (potion != null)
                        {
                            potion.itemCount++;
                        }
                        else
                        {
                            Managers.Inven.Add(newItem);
                        }
                    }
                    _playerStat.Gold -= _itemData.itemPrice;
                }

                Managers.UI.ShowPopupUI<UI_Alert>("UI_Alert").SetText($"{_itemData.itemName}을 {count}개 구매하셨습니다");
            }
            else
                Managers.UI.ShowPopupUI<UI_Alert>("UI_Alert").SetText("가방공간이 부족합니다");

            Util.FindGameSceneChild("UI_Inven", true).GetComponent<UI_Inven>().RefreshUI();
        }
        else
        {
            Managers.UI.ShowPopupUI<UI_Alert>("UI_Alert").SetText("골드가 부족합니다");
        }
    }
}
