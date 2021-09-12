using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Alert : UI_Popup
{
    public override void Init(){
        base.Init();
    }

    public void SetText(string content){
       transform.GetChild(2).GetComponent<Text>().text = content;
    }

    public void OnClickConfirm(){
        Managers.UI.CloseAllPopupUI();
    }
}
