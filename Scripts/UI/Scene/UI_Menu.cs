using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Menu : UI_Base{
    
    public Button ReturnButton;
    public Button EscapeButton;
    public Button EndGameButton;
    PlayerController pc;


    public override void Init()
    {

    }
    
    public void SetMenu()
    {   
        pc = Managers.Game.GetPlayer().GetComponent<PlayerController>();
        ReturnButton = transform.GetChild(2).GetComponent<Button>();
        EscapeButton = transform.GetChild(3).GetComponent<Button>();
        EndGameButton = transform.GetChild(4).GetComponent<Button>();

        SetFunction_UI();
    }

    private void SetFunction_UI()
    {
        //Reset
        ResetFunction_UI();

        ReturnButton.onClick.AddListener(Function_ReturnButton);
        EscapeButton.onClick.AddListener(Function_EscapeButton);
        EndGameButton.onClick.AddListener(Function_EndGameButton);
    }
    

    private void ResetFunction_UI()
    {
        ReturnButton.onClick.RemoveAllListeners();
        EscapeButton.onClick.RemoveAllListeners();
        EndGameButton.onClick.RemoveAllListeners();
    }
    private void Function_ReturnButton()
    {
        if(pc._stopMoving)
            pc._stopMoving = false;
            
        gameObject.SetActive(false);
    }
    private void Function_EscapeButton(){
        LoadingScene.LoadScene("Game");
    }
    private void Function_EndGameButton(){
        Application.Quit();
    }

}
