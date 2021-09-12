using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
 	protected int _itemId;
	protected string _itemPath;
    protected string _itemName;
	protected string _itemTemplate;
	protected string _itemType;
    protected string _itemOption;
	protected int _itemCount;
    protected int _itemDmg;
    protected int _itemDefense;
    protected int _itemHp;
	protected int _itemMp;
	protected int _itemSlot;
	protected int _itemEndurance;
	protected int _itemEnhance;
	protected int _itemMaxEnhance;
	protected bool _itemTransaction;
    protected int _itemPrice;
	

    public int itemId { get { return _itemId; } set { _itemId = value; } }
    public string itemPath { get { return _itemPath; } set { _itemPath = value; } }
    public string itemName { get { return _itemName; } set { _itemName = value; } }
    public string itemTemplate { get { return _itemTemplate; } set { _itemTemplate = value; } }
	public string itemType { get { return _itemType; } set { _itemType = value; } }
    public string itemOption{ get { return _itemOption; } set { _itemOption = value; } }
    public int itemCount { get { return _itemCount; } set { _itemCount = value; } }
    public int itemDmg { get { return _itemDmg; } set { _itemDmg = value; } }
    public int itemMp { get { return _itemMp; } set { _itemMp = value; } }
    public int itemDefense { get { return _itemDefense; } set { _itemDefense = value; } }
	public int itemHp { get { return _itemHp; } set { _itemHp = value; } }
	public int itemSlot { get { return _itemSlot; } set { _itemSlot = value; } }
	public int itemEndurance { get { return _itemEndurance; } set { _itemEndurance = value; } }
	public int itemEnhance { get { return _itemEnhance; } set { _itemEnhance = value; } }
	public int itemMaxEnhance { get { return _itemMaxEnhance; } set { _itemMaxEnhance = value; } }
	public bool itemTransaction { get { return _itemTransaction; } set { _itemTransaction = value; } }
    public int itemPrice { get { return _itemPrice; } set { _itemPrice = value; } }

    public static Item MakeItem(int _itemId)
	{
        Item item = null;
		Data.Item itemData = null;

		Managers.Data.ItemDict.TryGetValue(_itemId, out itemData);

		if (itemData == null)
			return null;

        item = new Item();


		if (item != null)
		{
            item._itemId = itemData.itemId;
			item._itemPath = itemData.itemPath;
            item._itemName = itemData.itemName;
            item._itemTemplate = itemData.itemTemplate;
            item._itemType = itemData.itemType;
            item._itemCount = itemData.itemCount;
            item._itemDmg = itemData.itemDmg;
            item._itemMp = itemData.itemMp;
            item._itemDefense = itemData.itemDefense;
            item._itemHp = itemData.itemHp;
            item._itemSlot = itemData.itemSlot;
            item._itemEndurance = itemData.itemEndurance;
            item._itemEnhance = itemData.itemEnhance;
            item._itemMaxEnhance = itemData.itemMaxEnhance;
            item._itemTransaction = itemData.itemTransaction;
            item._itemPrice = itemData.itemPrice;
		}
		return item;
	}
}

