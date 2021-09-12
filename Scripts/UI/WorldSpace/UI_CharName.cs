using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CharName : UI_Base
{
    enum GameObjects
    {
        CharGuild,
        CharName
    }
    public float up = 0.4f;
    Stat _stat;
    string _charGuild;
    string _charName;

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        _charName = transform.parent.gameObject.name;
        // string _charTag = transform.parent.tag;

        // switch(_charTag){
        //     case "Monster":
        //         transform.parent.GetComponent<MonsterController>();
        //         break;
        //     case "Player":
        //         transform.parent.GetComponent<PlayerController>();
        //     break;
        //     case "Npc":
        //         transform.parent.GetComponent<NpcController>();
        //         break;
        // }

        SetCharText();
    }

    private void Update()
    {
        Transform parent = transform.parent;
        transform.position = parent.position + Vector3.up * (parent.GetComponent<CapsuleCollider>().bounds.size.y) + new Vector3(0,up,0);
        transform.rotation = Camera.main.transform.rotation;
    }

    public void SetCharText(){
        GetObject((int)GameObjects.CharGuild).GetComponent<Text>().text = $"";
        GetObject((int)GameObjects.CharName).GetComponent<Text>().text = $"{_charName}";
    }
}
