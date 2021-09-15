using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameScene : UI_Scene
{
   public UI_Status StatusUI {get; set;}
   public UI_Inven InvenUI{get; set;}
   public UI_Shop ShopUI{get;set;}
   public UI_Enhance EnhanceUI{get;set;}
   public UI_SkillList SkillUI{get;set;}
   public UI_Menu MenuUI{get;set;}

   public override void Init(){
        base.Init();
        Canvas canvas = Util.GetOrAddComponent<Canvas>(gameObject);
        canvas.sortingOrder = 2;

        StatusUI = GetComponentInChildren<UI_Status>();
        InvenUI = GetComponentInChildren<UI_Inven>();
        ShopUI = GetComponentInChildren<UI_Shop>();
        EnhanceUI = GetComponentInChildren<UI_Enhance>();
        SkillUI = GetComponentInChildren<UI_SkillList>();
        MenuUI = GetComponentInChildren<UI_Menu>();

        EnhanceUI.SetEnhanceUI();
        StatusUI.SetEquip();
        StatusUI.RefreshUI();
        InvenUI.SetInven();
        InvenUI.RefreshUI();
        SkillUI.SetSkillGrid();
        MenuUI.SetMenu();
        
        EnhanceUI.gameObject.SetActive(false);
        InvenUI.gameObject.SetActive(false);
        StatusUI.gameObject.SetActive(false);
        ShopUI.gameObject.SetActive(false);
        SkillUI.gameObject.SetActive(false);
        MenuUI.gameObject.SetActive(false);
    }
}
