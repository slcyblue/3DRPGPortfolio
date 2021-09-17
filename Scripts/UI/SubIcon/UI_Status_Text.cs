using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Status_Text : UI_Status
{
    public void SetText(GameObject player){
        PlayerStat playerStat = player.GetComponent<PlayerStat>();
        GameObject stats = Util.FindGameSceneChild("UI_Status/Stats", true);
        stats.transform.GetChild(0).GetComponent<Text>().text = $"{player.name}   LV {playerStat.Level}";
        Transform statTexts = stats.transform.GetChild(1);
        statTexts.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = $"{playerStat.MaxHp}";
        statTexts.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = $"{playerStat.Attack}";
        statTexts.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = $"{playerStat.Defense}";
        stats.transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "";
        stats.transform.GetChild(3).transform.GetChild(0).GetComponent<Text>().text = "";
    }
}
