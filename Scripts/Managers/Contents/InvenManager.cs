using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InvenManager
{
	//플레이어가 가지고있는 item 딕셔너리 = Inven.Items
    public Dictionary<int, Item> Items { get; } = new Dictionary<int, Item>();
	public int BagSize = 56;
	Item[] items = new Item[56];
	bool BagIsFull = false;
	int _emptySlot;

	public void Add(Item item)
	{
		FindEmptySlot();

		if(!BagIsFull){
			item.itemSlot = _emptySlot;
			Items.Add(_emptySlot, item);
		}else{
			return;
		}		
	}

	public void Add(Item item, int itemSlot){
		item.itemSlot = itemSlot;
		Items.Add(itemSlot, item);
	}
	
	public void Remove(Item item){
		Items.Remove(item.itemSlot);
	}

	public Item Get(int slotIndex)
	{
		Item item = null;
		Items.TryGetValue(slotIndex, out item);
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

	public void FindEmptySlot(){
		List<Item> item = Managers.Inven.Items.Values.ToList();
		item.Sort((left, right) => { return (int)(left.itemSlot - right.itemSlot); });
		
		if(item.Count>=BagSize){
			Debug.Log("가방이 꽉 찼습니다!");
			BagIsFull = true;
			return;
		}

		item.CopyTo(items);

		for(int i=0; i<items.Length; i++){
			if(items[i] == null){
				_emptySlot = i;
				break;
			}else if(items[i].itemSlot != i){
				_emptySlot = i;
				break;
			}
		}

		Array.Clear(items, 0, items.Length);
	}

	public void SortSlot(){
		List<Item> item = Managers.Inven.Items.Values.ToList();
		item.Sort((left, right) => { return (int)(left.itemSlot - right.itemSlot); });
		
		item.CopyTo(items);

		for(int i=0; i<items.Length; i++){
			if(items[i] != null){
				if(items[i].itemSlot != i){
				Items.TryGetValue(items[i].itemSlot, out Item sortItem);
				Items.Remove(sortItem.itemSlot);
				Add(sortItem, i);
				}
			}else{
				break;
			}
			
		}
		Array.Clear(items, 0, items.Length);
	}

	public void Clear()
	{
		Items.Clear();
	}
}
