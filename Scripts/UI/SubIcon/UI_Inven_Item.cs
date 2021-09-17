using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Inven_Item : UI_Inven, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item _itemData;
    public int _itemSlot;
    public Image _iconImage = null;
    public Text _iconText = null;
    public bool isOver = false;
    Color color;
    PlayerStat _playerStat;
    GameObject _player;
    UI_Status_Text statsText;
    GameObject _shopUI;
    GameObject _enhanceUI;
    GameObject _equipUI;
    GameObject _invenUI;

    public void Init(int itemSlot)
    {
        _itemSlot = itemSlot;
        statsText = transform.parent.transform.GetChild(1).gameObject.GetOrAddComponent<UI_Status_Text>();
        _player = Managers.Game.GetPlayer();
        _playerStat = _player.GetComponent<PlayerStat>();
        _shopUI = Util.FindGameSceneChild("UI_Shop", true);
        _enhanceUI = Util.FindGameSceneChild("UI_Enhance", true);
        _equipUI = Util.FindGameSceneChild("UI_Status", true);
        _invenUI = Util.FindGameSceneChild("UI_Inven", true);
    }

    public void SetItem(int itemSlot)
    {
        Managers.Inven.Items.TryGetValue(itemSlot, out _itemData);

        if (_itemData == null)
        {
            return;
        }
        else
        {
            Sprite icon = Managers.Resource.Load<Sprite>(_itemData.itemPath);
            _iconImage = gameObject.transform.GetChild(1).GetComponent<Image>();
            _iconText = _iconImage.GetComponentInChildren<Text>();

            _iconImage.sprite = icon;
            color = _iconImage.color;
            color.a = 1;
            _iconImage.color = color;
            
            if(_itemData.itemCount > 1)
                _iconText.text = $"{_itemData.itemCount}";
            else
                _iconText.text = "";
        }
    }

    public void ClearSlot()
    {
        if (_iconImage != null)
        {
            _iconImage.sprite = null;
            color = _iconImage.color;
            color.a = 0;
            _iconImage.color = color;
            
            _iconText = null;
            _itemData = null;
            _iconImage = null;
        }
    }

    #region UIEvent
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (_equipUI.activeSelf == true || (_enhanceUI.activeSelf == false && _shopUI.activeSelf == false))
            { //swap equip Item
                if (_itemData != null)
                {
                    if (_itemData.itemTemplate == Define.ItemTemplate.Equipment.ToString())
                    {
                        //아이템 장착
                        Managers.Inven.Items.TryGetValue(_itemSlot, out Item clickItem);

                        if (!Managers.Equip.Items.TryGetValue(clickItem.itemType, out Item equipItem))
                        {
                            Managers.Inven.Remove(clickItem);
                            Managers.Equip.Add(clickItem);

                            ClearSlot();
                            GameObject go = GameObject.Find("@UI_Root/UI_GameScene/UI_Status/EquipSlots");
                            go.transform.Find(clickItem.itemType).GetComponentInChildren<UI_Equip_Item>().SetItem(clickItem.itemType);

                            _playerStat.PlusEquipStat(clickItem);
                            statsText.SetText(_player);
                        }
                        else
                        {
                            //장착중인 아이템이 있는 경우
                            Managers.Equip.Remove(equipItem);
                            Managers.Inven.Remove(clickItem);
                            Managers.Equip.Add(clickItem);
                            Managers.Inven.Add(equipItem, _itemSlot);

                            SetItem(_itemSlot);
                            GameObject go = GameObject.Find("@UI_Root/UI_GameScene/UI_Status/EquipSlots");
                            go.transform.Find(clickItem.itemType).GetComponentInChildren<UI_Equip_Item>().SetItem(clickItem.itemType);

                            //after DB switch
                            _playerStat.AbsEquipStat(equipItem);
                            _playerStat.PlusEquipStat(clickItem);
                            statsText.SetText(_player);
                        }
                    }
                    else if (_itemData.itemTemplate == Define.ItemTemplate.Consumable.ToString())
                    {
                        if (_itemData.itemCount > 1)
                        {
                            _itemData.itemCount -= 1;
                            Util.FindGameSceneChild("UI_Inven", true).GetComponent<UI_Inven>().RefreshUI();
                        }
                        else if (_itemData.itemCount == 1)
                        {
                            ClearSlot();
                            Managers.Inven.Items.TryGetValue(_itemSlot, out Item potion);
                            Managers.Inven.Remove(potion);
                            Util.FindGameSceneChild("UI_Inven", true).GetComponent<UI_Inven>().RefreshUI();
                        }
                        _playerStat.Hp += 100;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else if (_enhanceUI.activeSelf == true)
            { //entry enhance Item
                if(_itemData == null){
                    return;
                }
                UI_Enhance_Item enhanceSlot = _enhanceUI.GetComponentInChildren<UI_Enhance_Item>();

                if (_itemData.itemTemplate == Define.ItemTemplate.Equipment.ToString())
                {
                    if (enhanceSlot._iconImage != null)
                    {
                        Managers.Inven.Items.TryGetValue(_itemSlot, out Item clickItem);
                        Managers.Inven.Remove(clickItem);
                        Managers.Inven.Add(enhanceSlot._itemData, _itemSlot);

                        enhanceSlot._itemData = clickItem;
                        enhanceSlot.SetItem();
                        SetItem(_itemSlot);
                        _enhanceUI.GetComponent<UI_Enhance>().RefreshUI();
                    }
                    else
                    {
                        Managers.Inven.Items.TryGetValue(_itemSlot, out Item clickItem);
                        Managers.Inven.Remove(clickItem);

                        enhanceSlot._itemData = clickItem;
                        enhanceSlot.SetItem();

                        ClearSlot();
                        _invenUI.GetComponent<UI_Inven>().RefreshUI();
                        _enhanceUI.GetComponent<UI_Enhance>().RefreshUI();
                    }
                }
                else
                    Managers.UI.ShowPopupUI<UI_Alert>("UI_Alert").SetText("강화할 수 있는 아이템이 아닙니다.");
            }
            else if (_shopUI.activeSelf == true)
            { //sell Item
                Managers.Inven.Items.TryGetValue(_itemSlot, out Item clickItem);
                Managers.Inven.Remove(clickItem);
                ClearSlot();
                _playerStat.Gold += (int)(clickItem.itemPrice * 0.5f);
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
        if (_itemData != null)
        {
            DragSlot.instance.invenSlot = this;
            DragSlot.instance.DragSetImage(_iconImage);
            DragSlot.instance.transform.position = eventData.position;
        }
        else
        {
            return;
        }
    }

    // 마우스 드래그 중일 때 계속 발생하는 이벤트
    public void OnDrag(PointerEventData eventData)
    {
        if (_itemData != null)
            DragSlot.instance.transform.position = eventData.position;
    }

    // 마우스 드래그가 끝났을 때 발생하는 이벤트
    public void OnEndDrag(PointerEventData eventData)
    {
        if (DragSlot.instance.iconImage != null)
        {
            DragSlot.instance.SetColor(0);
            DragSlot.instance.iconImage = null;
            DragSlot.instance.invenSlot = null;
        }
    }


    // 해당 슬롯에 무언가가 마우스 드롭 됐을 때 발생하는 이벤트
    public void OnDrop(PointerEventData eventData)
    {
        //inven -> inven
        if (DragSlot.instance.invenSlot != null)
            ChangeInvenSlot();
        //equip -> inven
        else if (DragSlot.instance.equipSlot != null)
            ChangeEquipSlot();
        //shop -> inven
        else if (DragSlot.instance.shopSlot != null)
            ChangeShopSlot();
        //enhance -> inven
        else if (DragSlot.instance.enhanceSlot != null)
            ChangeEnhanceSlot();
    }

    private void ChangeInvenSlot() //inven -> inven
    {
        Image _tempImage = _iconImage;
        if (DragSlot.instance.invenSlot._itemSlot == _itemSlot)
        {
            return;
        }
        else
        {
            SetItem(DragSlot.instance.invenSlot._itemSlot); //7번 슬롯에 2번 데이터 저장

            if (_tempImage != null)
            {
                DragSlot.instance.invenSlot.SetItem(_itemSlot);

                //DB Update
                Managers.Inven.Items.TryGetValue(_itemSlot, out Item dropitem); //get dropData
                Managers.Inven.Items.TryGetValue(DragSlot.instance.invenSlot._itemSlot, out Item dragitem); //get dragData

                Managers.Inven.Remove(dragitem); //remove dragData
                Managers.Inven.Remove(dropitem); //remove dropData

                Managers.Inven.Add(dragitem, _itemSlot); //Add dragData to dropSlot index
                Managers.Inven.Add(dropitem, DragSlot.instance.invenSlot._itemSlot); //Add dropData to dragSlot
            }
            else
            {
                DragSlot.instance.invenSlot.ClearSlot(); //set dragSlot with dragSlot index

                Managers.Inven.Items.TryGetValue(DragSlot.instance.invenSlot._itemSlot, out Item dragitem); //get dragData
                Managers.Inven.Remove(dragitem); //Remove dragData
                Managers.Inven.Add(dragitem, _itemSlot); //Add dragData to dropSlot index
            }
        }
    }

    private void ChangeEquipSlot()
    {  //equip->inven
        Image _tempImage = _iconImage;

        if (_tempImage != null)
        {
            if (_itemData.itemType.Substring(0, _itemData.itemType.Length - 1) == DragSlot.instance.equipSlot._itemData.itemType.Substring(0, DragSlot.instance.equipSlot._itemData.itemType.Length - 1))
            {
                //DB Update
                Managers.Equip.Items.TryGetValue(DragSlot.instance.equipSlot._itemData.itemType, out Item dragItem); //get dragData
                Managers.Inven.Items.TryGetValue(_itemSlot, out Item dropItem); //get dropData

                Managers.Equip.Remove(dragItem); //remove dragData
                Managers.Inven.Remove(dropItem); //remove dropData

                Managers.Inven.Add(dragItem, _itemSlot); //Add dragData to dropSlot index
                Managers.Equip.Add(dropItem); //Add dropData to dragSlot

                SetItem(_itemSlot); //Set DragSlot to EquipItem
                DragSlot.instance.equipSlot.SetItem(_itemData.itemType); //Set EquipSlot to DragItem

                //after DB switch
                _playerStat.AbsEquipStat(dragItem);
                _playerStat.PlusEquipStat(dropItem);
                statsText.SetText(_player);
            }
        }
        else
        {
            Managers.Equip.Items.TryGetValue(DragSlot.instance.equipSlot._itemType, out Item dragItem); //get dragData
            Managers.Equip.Remove(dragItem); //Remove dragData
            Managers.Inven.Add(dragItem, _itemSlot); //Add dragData to dropSlot index

            SetItem(_itemSlot);
            DragSlot.instance.equipSlot.ClearSlot(); //set dragSlot with dragSlot index

            _playerStat.AbsEquipStat(dragItem);
            statsText.SetText(_player);
        }
    }

    private void ChangeShopSlot() //Shop -> inven
    {
        Managers.UI.ShowPopupUI<UI_SetCount>().shopItem = DragSlot.instance.shopSlot;
    }

    private void ChangeEnhanceSlot() //Enhance -> inven
    {
        Image _tempImage = _iconImage;

        if (_tempImage != null)
        {
            if (_itemData.itemTemplate == Define.ItemTemplate.Equipment.ToString())
            {
                Managers.Inven.Items.TryGetValue(_itemSlot, out Item dropItem);
                Managers.Inven.Remove(dropItem);
                Managers.Inven.Add(DragSlot.instance.enhanceSlot._itemData, _itemSlot);
                DragSlot.instance.enhanceSlot._itemData = dropItem;

                DragSlot.instance.enhanceSlot.SetItem();
                SetItem(_itemSlot);
                DragSlot.instance.enhanceSlot.RefreshUI();
            }
            else
                Managers.UI.ShowPopupUI<UI_Alert>("UI_Alert").SetText("강화할 수 있는 아이템이 아닙니다.");
        }
        else
        {
            Managers.Inven.Add(DragSlot.instance.enhanceSlot._itemData, _itemSlot);

            DragSlot.instance.enhanceSlot.ClearSlot();
            SetItem(_itemSlot);
            DragSlot.instance.enhanceSlot.transform.parent.GetComponent<UI_Enhance>().RefreshUI();
        }
    }
    #endregion
}