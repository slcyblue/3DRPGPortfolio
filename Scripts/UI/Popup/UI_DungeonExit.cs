using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_DungeonExit : UI_Popup
{
    public override void Init(){
        base.Init();

        // ConfirmButton = transform.GetChild(3).GetComponent<Button>();
        // QuitButton = transform.GetChild(4).GetComponent<Button>();

        // SetFunction_UI();
    }

    // void SetFunction_UI(){
    //     ConfirmButton.onClick.AddListener(OnClickConfirm);
    //     QuitButton.onClick.AddListener(OnClickQuit);
    // }
    
    public void SetText(string content){
        transform.GetChild(2).GetComponent<Text>().text = content;
    }

    public void OnClickConfirm(){
        LoadingScene.LoadScene("Game");
    }

    public void OnClickQuit(){
        Managers.UI.ClosePopupUI();
    }
}
