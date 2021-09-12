using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_Inven : UI_Base
{
    public List<UI_Inven_Item> Items{get;} = new List<UI_Inven_Item>();
    UI_Inven_Item _item;
    PlayerController player;
    public override void Init()
    {
       
    }

#region SetItem
    public void SetInven()
    {
        Items.Clear();
        player = Managers.Game.GetPlayer().GetComponent<PlayerController>();
        GameObject grid = gameObject.transform.GetChild(1).gameObject;
        foreach (Transform child in grid.transform){
            if(child == null){
                continue;
            }
            Managers.Resource.Destroy(child.gameObject);
        }

        for (int i = 0; i < Managers.Inven.BagSize; i++)
        {
            GameObject go = Managers.Resource.Instantiate("UI/SubIcon/UI_Inven_Item", grid.transform);
            _item = go.GetOrAddComponent<UI_Inven_Item>();
            _item.Init(i);
            Items.Add(_item);
        }
    }
    //아이템 목록 새로고침
    public void RefreshUI(){
        List<int> list = new List<int>();        
        List<Item> items = Managers.Inven.Items.Values.ToList();
		items.Sort((left, right) => { return left.itemSlot - right.itemSlot; });

		foreach(Item item in items){
            Items[item.itemSlot].SetItem(item.itemSlot);
            list.Add(item.itemSlot);
        }

        for(int i=0; i<Items.Count; i++){
            if(!list.Contains(i)){
                Items[i].ClearSlot();
            }
        }
    }
    
    public void OnClickSort(){
        Managers.Inven.SortSlot();
        RefreshUI();
    }
    public void OnClickQuit(){
        player._stopMoving = false;
        gameObject.SetActive(false);
    }
    #endregion
}
