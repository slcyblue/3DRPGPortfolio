using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ExpBar : UI_Scene{
    Slider slider;
    PlayerStat player;
    Data.Stat stat;
    int level=1;
    public override void Init()
    {
        player = Managers.Game.GetPlayer().GetComponent<PlayerStat>();
        slider = transform.GetChild(0).GetComponent<Slider>();
    }

    private void Update() {
        level=player.Level;
        Managers.Data.StatDict.TryGetValue(level + 1, out stat);
        
        float ratio = player.Exp/(float)stat.totalExp*100;
        slider.value = ratio;
        slider.GetComponentInChildren<Text>().text = $"Exp {player.Exp}/{stat.totalExp}({ratio.ToString("0%")}%)";
    }
}
