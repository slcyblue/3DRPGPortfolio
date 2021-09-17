using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class NpcController : BaseController{
    GameObject player;
    UI_Shop shopUI;
    UI_Enhance enhanceUI;
    public NpcData npcData = null;
    Vector3 startPos;
    Quaternion startRotate;
    bool playerIsNear = false;

    public override void Init(int npcId) {
        WorldObjectType = Define.WorldObject.Npc;
        _Id = npcId;
        Managers.Data.NpcDict.TryGetValue(_Id, out npcData);
        State = Define.State.Idle;
        startPos = transform.position;
        startRotate = transform.rotation;
        player = Managers.Game.GetPlayer();

        shopUI = GameObject.Find("@UI_Root/UI_GameScene/UI_Shop").GetComponent<UI_Shop>();
        enhanceUI = GameObject.Find("@UI_Root/UI_GameScene/UI_Enhance").GetComponent<UI_Enhance>();

        if (gameObject.GetComponentInChildren<UI_CharName>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_CharName>(transform);
    }
    protected override void UpdateIdle() {
        if(playerIsNear)
            transform.LookAt(player.transform);

        if(player != null && (player.transform.position - transform.position).magnitude > 3){
            if(enhanceUI.gameObject.activeSelf == true){  
                if(enhanceUI.transform.GetChild(0).GetComponent<UI_Enhance_Item>()._iconImage != null){ //등록된 아이템이 있을 경우
                    Item enhanceItem = enhanceUI.transform.GetChild(0).GetComponent<UI_Enhance_Item>()._itemData;
                    Managers.Inven.Add(enhanceItem);
                }
                enhanceUI.gameObject.SetActive(false);
            }
            else if(shopUI.gameObject.activeSelf == true)
                shopUI.gameObject.SetActive(false);
            player = null;
        }
        else{    
            transform.position = startPos;
            transform.rotation = startRotate;
        }
    }

    public void ShowUI(GameObject go){
        player = go;
        switch(npcData.npcType){
            case "Equipment":
                ShowShopUI();
                break;
            case "Consumable":
                ShowShopUI();
                break;
            case "Enhance":
                ShowEnhanceUI();
                break;
        }
    }

    private void ShowShopUI(){
        shopUI.SetProduct(npcData);
        shopUI.gameObject.SetActive(true);
    }
    private void ShowEnhanceUI(){
        enhanceUI.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")){
            playerIsNear = false;
        }
    }
}
