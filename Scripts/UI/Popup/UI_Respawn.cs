using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Respawn : UI_Popup
{
    Button ConfirmButton;
    Button QuitButton;
    public override void Init(){
        base.Init();

        ConfirmButton = transform.GetChild(3).GetComponent<Button>();
        QuitButton = transform.GetChild(4).GetComponent<Button>();

        SetFunction_UI();
    }

    void SetFunction_UI(){
        ConfirmButton.onClick.AddListener(OnClickConfirm);
        QuitButton.onClick.AddListener(OnClickQuit);
    }
    
    public void SetText(string content){
        transform.GetChild(2).GetComponent<Text>().text = content;
    }

    public void OnClickConfirm(){
        GameObject player = Managers.Game.GetPlayer();
        player.GetComponent<PlayerController>().State = Define.State.Idle;
        PlayerStat playerStat = player.GetComponent<PlayerStat>();
        playerStat.Hp = playerStat.MaxHp;
        Managers.UI.ClosePopupUI();
    }

    public void OnClickQuit(){
        GameObject player = Managers.Game.GetPlayer();
        player.GetComponent<PlayerController>().State = Define.State.Idle;
        PlayerStat playerStat = player.GetComponent<PlayerStat>();
        playerStat.Hp = playerStat.MaxHp;
        LoadingScene.LoadScene("Game");
        Managers.UI.ClosePopupUI();
    }
}
