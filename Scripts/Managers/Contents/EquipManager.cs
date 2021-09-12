using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipManager
{
	//플레이어가 장착한 item 딕셔너리 = Equip.Items
    public Dictionary<string, Item> Items { get; } = new Dictionary<string, Item>();
	String[] compareArr = {"Helmets", "Amulets", "Armors", "Belts", "Shoulders", "Gloves", "Pants", "Boots", "Ring1", "Ring2", "EarRing1", "EarRing2", "Weapons", "SubWeapons"};

	public void Add(Item item){
		if(Items.TryGetValue(item.itemType, out Item component)){
			Debug.Log("이미 값이 있습니다");
			return;
		}

		if(CheckTypeIsValid(item) != false)
			Items.Add(item.itemType, item);
		else
			Debug.Log("잘못된 아이템 형식입니다.");
	}
	
	public void Remove(Item item){
		Items.Remove(item.itemType);
	}

	public Item Get(string itemType)
	{
		Item item = null;
		Items.TryGetValue(itemType, out item);
		return item;
	}


	public Item Find(Func<Item, bool> condition)
	{
		foreach (Item item in Items.Values)
		{
			if (condition.Invoke(item))
				return item;
		}

		return null;
	}

	public bool CheckTypeIsValid(Item item){
		if(item.itemType != null){
			for(int i=0; i<compareArr.Length; i++){
				if(item.itemType == compareArr[i])
					return true;
			}
			return false;
		}else{
			Debug.Log("아이템 형식이 없습니다");
			return false;
		}
	}

	public void Clear()
	{
		Items.Clear();
	}
}
