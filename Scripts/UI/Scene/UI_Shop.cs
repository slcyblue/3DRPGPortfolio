using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

public class UI_Shop : UI_Base
{
    public List<UI_Shop_Item> Items{get;} = new List<UI_Shop_Item>();
    UI_Shop_Item _item;
    List<Data.Item> productList;
    public int _count;
    PlayerController player;
    public override void Init()
    {
       
    }

#region SetItem
    public void SetProduct(NpcData npcData)
    {
        Items.Clear();
        productList = npcData.products;
        player = Managers.Game.GetPlayer().GetComponent<PlayerController>();

        GameObject shopUI = gameObject.transform.GetChild(1).gameObject;
        GameObject grid = shopUI.transform.GetChild(0).transform.GetChild(0).gameObject;
        foreach (Transform child in grid.transform){
            if(child == null){
                continue;
            }
            Managers.Resource.Destroy(child.gameObject);
        }

        for (int i = 0; i < productList.Count; i++)
        {
            GameObject go = Managers.Resource.Instantiate("UI/SubIcon/UI_Shop_Item", grid.transform);
            _item = go.GetOrAddComponent<UI_Shop_Item>();
            
            if(productList[i].itemTemplate == npcData.npcType){
                Item shopItem = Item.MakeItem(productList[i].itemId);
                _item.Init(shopItem);
                Items.Add(_item);
            }
            else
                Managers.Resource.Destroy(go);
        }
    }
    //아이템 목록 새로고침
    public void RefreshUI(){
        //productList.Sort((left, right) => { return left.itemSlot - right.itemSlot; });

    }

    public void OnClickQuit(){
        player._stopMoving = false;
        gameObject.SetActive(false);
    }

    #endregion
}
