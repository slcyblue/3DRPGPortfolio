using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EnhanceResult : UI_Popup
{
    UI_Enhance enhanceUI;
    Item _itemData;
    public override void Init(){
        base.Init();

        enhanceUI = Util.FindGameSceneChild("UI_Enhance", true).GetComponent<UI_Enhance>();
    }

    public void SetData(Item item, bool result){
        _itemData = item;
        if(result)
            transform.GetChild(2).GetComponent<Text>().text = $"(+{item.itemEnhance}){item.itemName}로 강화에 성공했습니다!";
        else
            transform.GetChild(2).GetComponent<Text>().text = $"(+{item.itemEnhance}){item.itemName}로의 강화에 실패했습니다.";
    }

    public void OnClickConfirm(){
        enhanceUI._enhanceItem = _itemData;
        enhanceUI.RefreshUI();
        Managers.UI.CloseAllPopupUI();
    }
}
