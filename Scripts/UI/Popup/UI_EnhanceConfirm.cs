using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_EnhanceConfirm : UI_Popup
{
    Item _itemData;
    
    public override void Init(){
        base.Init();
    }

    public void SetData(Item item){
        _itemData = item;
        transform.GetChild(2).GetComponent<Text>().text = $"(+{item.itemEnhance}){item.itemName}을 (+{item.itemEnhance+1}){item.itemName}로 강화하시겠습니까?";
    }

    public void OnClickConfirm(){
        Managers.Data.EnhanceDict.TryGetValue(_itemData.itemEnhance, out Data.Enhance expectedData);
        UI_EnhanceResult resultUI = Managers.UI.ShowPopupUI<UI_EnhanceResult>("UI_EnhanceResult");
        if(GetSuccess(expectedData.probability)){
            _itemData.itemEnhance += 1;
            _itemData.itemDmg += expectedData.increasedDmg;
            _itemData.itemDefense += expectedData.increasedDef;
            _itemData.itemHp += expectedData.increasedHp;
            _itemData.itemMp += expectedData.increasedMp;
            
            resultUI.SetData(_itemData, true);
        }else{
            _itemData.itemMaxEnhance -= 1;
            resultUI.SetData(_itemData, false);
        }
    }

    public void OnClickQuit(){
        Managers.UI.ClosePopupUI();
    }

    bool GetSuccess(int probability){
        int rand = UnityEngine.Random.Range(0,101);
        
        if(rand <= probability)
            return true;
        else
            return false;
    }
}
