using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_Enhance : UI_Base
{
    public Item _enhanceItem;
    public Data.Enhance _expectedData;
    public UI_Enhance_Item _enhanceSlot;
    public Text _enhanceName;
    public Text _entryText;
    public GameObject _currentText;
    public GameObject _expectedText;
    public bool _popFinished = false;
    PlayerController player;
    
    public override void Init()
    {

    }

#region SetItem
    public void SetEnhanceUI()
    {
        _enhanceSlot = transform.GetChild(0).GetComponent<UI_Enhance_Item>();
        _enhanceName = transform.GetChild(1).gameObject.GetComponent<Text>();
        _entryText = transform.GetChild(2).gameObject.GetComponent<Text>();
        _currentText = transform.GetChild(3).gameObject;
        _expectedText = transform.GetChild(4).gameObject;

        player = Managers.Game.GetPlayer().GetComponent<PlayerController>();

        RefreshUI();
    }
    
    public void RefreshUI(){
        if(_enhanceSlot._itemData != null){
            _enhanceName.text = $"(+{_enhanceItem.itemEnhance}){_enhanceItem.itemName}";
            _entryText.gameObject.SetActive(false);
            _currentText.SetActive(true);
            _expectedText.SetActive(true);
            SetCurrentText(_enhanceItem);
            SetExpectedText(_enhanceItem);
        }else{
            _enhanceName.text = "";
            string text = "아이템을 등록해주세요";
            noItem(text);
        }
    }

    protected void SetCurrentText(Item enhanceItem){
        _currentText.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = $"{enhanceItem.itemEnhance}";
        _currentText.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = $"{enhanceItem.itemDmg}";
        _currentText.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = $"{enhanceItem.itemDefense}";
        _currentText.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = $"{enhanceItem.itemHp}";
        _currentText.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = $"{enhanceItem.itemMp}";
    }

    protected void SetExpectedText(Item enhanceItem){
        Managers.Data.EnhanceDict.TryGetValue(enhanceItem.itemEnhance, out _expectedData);
        if(_expectedData!=null){
            _expectedText.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = $"{_expectedData.nextEnhance}";
            _expectedText.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = $"{enhanceItem.itemDmg+_expectedData.increasedDmg} (+{_expectedData.increasedDmg})";
            _expectedText.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = $"{enhanceItem.itemDefense+_expectedData.increasedDef} (+{_expectedData.increasedDef})";
            _expectedText.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = $"{enhanceItem.itemHp+_expectedData.increasedHp} (+{_expectedData.increasedHp})";
            _expectedText.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = $"{enhanceItem.itemMp+_expectedData.increasedMp} (+{_expectedData.increasedMp})";
        }else{
            string text = "강화가 최대치입니다";
            noItem(text);
        }
        
    }

    protected void noItem(string text){
        _entryText.gameObject.SetActive(true);
        _entryText.text = $"{text}";
        _currentText.SetActive(false);
        _expectedText.SetActive(false);
    }


    public void OnClickEnhance(){
        
        if(_enhanceSlot._itemData != null &&_enhanceSlot._itemData.itemMaxEnhance <= 0){
            Managers.UI.ShowPopupUI<UI_Alert>("UI_Alert").SetText("강화횟수를 모두 소진했습니다");
        }
        else if(_enhanceSlot._itemData != null && _enhanceSlot._itemData.itemEnhance >= Managers.Data.EnhanceDict.Count){
            Managers.UI.ShowPopupUI<UI_Alert>("UI_Alert").SetText("최대치로 강화했습니다.");
        }
        else if(_enhanceSlot._itemData != null){
            UI_EnhanceConfirm confirmUI = Managers.UI.ShowPopupUI<UI_EnhanceConfirm>("UI_EnhanceConfirm");
            confirmUI.SetData(_enhanceSlot._itemData);
        }else{
            Managers.UI.ShowPopupUI<UI_Alert>("UI_Alert").SetText("강화할 아이템이 없습니다");
        }       
    }

    public void OnClickCancle(){
        if(_enhanceItem != null)
            Managers.Inven.Add(_enhanceItem);
       
        _enhanceItem = null;
        _enhanceSlot.ClearSlot();
        RefreshUI();
        Util.FindGameSceneChild("UI_Inven",true).GetComponent<UI_Inven>().RefreshUI();
        player._stopMoving = false;
        gameObject.SetActive(false);
    }

    public void OnClickQuit(){
        if(_enhanceItem != null)
            Managers.Inven.Add(_enhanceItem);
       
        _enhanceItem = null;
        _enhanceSlot.ClearSlot();
        RefreshUI();
        Util.FindGameSceneChild("UI_Inven",true).GetComponent<UI_Inven>().RefreshUI();
        player._stopMoving = false;
        gameObject.SetActive(false);
    }
#endregion
}
