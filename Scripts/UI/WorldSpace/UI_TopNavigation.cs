using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TopNavigation : UI_Scene{
    
    GameObject player;
    GameObject menuGrid;
    Button invenButton;
    Button statusButton;
    Button skillButton;
    
    Button setupButton;

    Button questButton; //todo

    GameObject invenUI;
    GameObject statusUI;
    GameObject skillUI;
    GameObject menuUI;

    Text goldText;
    Text timeText;
    PlayerController pc;
    PlayerStat ps;
    public override void Init()
    {
        player = Managers.Game.GetPlayer();
        pc = player.GetComponent<PlayerController>();
        ps = player.GetComponent<PlayerStat>();
        
        menuGrid = transform.GetChild(1).gameObject;
        invenButton = menuGrid.transform.GetChild(0).GetComponent<Button>();
        statusButton = menuGrid.transform.GetChild(1).GetComponent<Button>();
        skillButton = menuGrid.transform.GetChild(2).GetComponent<Button>();
        questButton = menuGrid.transform.GetChild(3).GetComponent<Button>();

        invenUI =  Util.FindGameSceneChild("UI_Inven",true);
        statusUI =  Util.FindGameSceneChild("UI_Status",true);
        skillUI =  Util.FindGameSceneChild("UI_SkillList",true);
        menuUI = Util.FindGameSceneChild("UI_Menu",true);

        goldText = transform.GetChild(2).GetComponentInChildren<Text>();
        timeText = transform.GetChild(4).GetComponent<Text>();

        
        setupButton = transform.GetChild(5).GetComponent<Button>();
        

        SetFunction_UI();
    }

    private void SetFunction_UI()
    {
        invenButton.onClick.AddListener(Function_InvenButton);
        statusButton.onClick.AddListener(Function_StatusButton);
        skillButton.onClick.AddListener(Function_SkillButton);
        setupButton.onClick.AddListener(Function_SetupButton);
    }

    private void Update() {
        goldText.text = ps.Gold.ToString() + "G";
        string time = DateTime.Now.ToString("HH:mm");
        timeText.text = time;
    }

    private void Function_InvenButton(){
        if(invenUI.activeSelf == true){
            pc._stopMoving = false;
            invenUI.SetActive(false);
        }else{
            invenUI.GetComponent<UI_Inven>().RefreshUI();
            invenUI.transform.SetAsLastSibling();
            pc._stopMoving = true;
            invenUI.SetActive(true);
        }
    }
    private void Function_StatusButton(){
        if(statusUI.activeSelf == true){
            pc._stopMoving = false;
            statusUI.SetActive(false);
        }else{
            statusUI.GetComponent<UI_Status>().RefreshUI();
            statusUI.transform.SetAsLastSibling();
            pc._stopMoving = true;
            statusUI.SetActive(true);
        }
    }
    private void Function_SkillButton(){
        if(skillUI.activeSelf == true){
            pc._stopMoving = false;
            skillUI.SetActive(false);
        }else{
            statusUI.transform.SetAsLastSibling();
            pc._stopMoving = true;
            skillUI.SetActive(true);
        }
    }

    private void Function_SetupButton(){
        if(menuUI.activeSelf == true){
            pc._stopMoving = false;
            menuUI.SetActive(false);
        }else{
            menuUI.transform.SetAsLastSibling();
            pc._stopMoving = true;
            menuUI.SetActive(true);
        }
    }
}

