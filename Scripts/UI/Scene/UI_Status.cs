using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_Status : UI_Base
{
    PlayerStat _playerStat;
    GameObject equipSlots;
    UI_Status_Text statsText;
    GameObject player;
    PlayerController pc;
    public override void Init()
    {

    }

#region SetItem
    public void SetEquip()
    {   
        player = Managers.Game.GetPlayer();
        pc = player.GetComponent<PlayerController>();

        statsText = gameObject.transform.GetChild(1).gameObject.GetComponent<UI_Status_Text>();
        equipSlots = gameObject.transform.GetChild(2).gameObject;        
        
        foreach(Transform child in equipSlots.transform){
            UI_Equip_Item equip = child.gameObject.GetOrAddComponent<UI_Equip_Item>();
            equip._itemType = equip.gameObject.name;
        }
    }

    //장비 목록 새로고침
    public void RefreshUI(){
        List<Item> items = Managers.Equip.Items.Values.ToList();
      
        foreach (Item item in items)
		{
            switch(item.itemType){
                case "Helmets":
                    UI_Equip_Item helmets = equipSlots.transform.Find("Helmets").gameObject.GetComponent<UI_Equip_Item>();
                    if(helmets._itemType == item.itemType)
                        helmets.SetItem(item.itemType);
                    break;
                case "Armors":
                    UI_Equip_Item armors = equipSlots.transform.Find("Armors").gameObject.GetComponent<UI_Equip_Item>();
                    if(armors._itemType == item.itemType)
                        armors.SetItem(item.itemType);
                    break;
                case "Shoulders":
                    UI_Equip_Item shoulders = equipSlots.transform.Find("Shoulders").gameObject.GetComponent<UI_Equip_Item>();
                    if(shoulders._itemType == item.itemType)
                        shoulders.SetItem(item.itemType);
                    break;
                case "Boots":
                    UI_Equip_Item boots = equipSlots.transform.Find("Boots").gameObject.GetComponent<UI_Equip_Item>();;
                        if(boots._itemType == item.itemType)
                        boots.SetItem(item.itemType);
                    break;
                case "Weapons":
                    UI_Equip_Item weapons = equipSlots.transform.Find("Weapons").gameObject.GetComponent<UI_Equip_Item>();
                    if(weapons._itemType == item.itemType)
                        weapons.SetItem(item.itemType);
                    break;
                case "SubWeapons":
                    UI_Equip_Item subWeapons = equipSlots.transform.Find("SubWeapons").gameObject.GetComponent<UI_Equip_Item>();
                    if(subWeapons._itemType == item.itemType)
                        subWeapons.SetItem(item.itemType);
                    break;
            }
        }

        statsText.SetText(player);
    }

    public void OnClickQuit(){
        pc._stopMoving = false;
        gameObject.SetActive(false);
    }
#endregion
}
