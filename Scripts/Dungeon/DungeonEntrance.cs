using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEntrance : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){
            Managers.UI.ShowPopupUI<UI_DungeonEnter>("UI_DungeonEnter").SetText("던전에 입장하겠습니까?");
        }else{
            return;
        }
    }
}
