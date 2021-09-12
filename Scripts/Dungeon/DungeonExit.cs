using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonExit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){
            Managers.UI.ShowPopupUI<UI_DungeonExit>("UI_DungeonExit").SetText("던전에서 나가시겠습니까?");
        }else{
            return;
        }
    }
}
