using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Status_Text : UI_Status
{
    public void SetText(PlayerStat playerStat){
        GameObject stats = GameObject.Find("@UI_Root/UI_GameScene/UI_Status/Stats");
        GameObject statTexts = stats.transform.GetChild(1).gameObject;
        statTexts.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = $"{playerStat.MaxHp}";
        statTexts.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = $"{playerStat.Attack}";
        statTexts.transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = $"{playerStat.Defense}";
        stats.transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "";
        stats.transform.GetChild(3).transform.GetChild(0).GetComponent<Text>().text = "";
    }
}
