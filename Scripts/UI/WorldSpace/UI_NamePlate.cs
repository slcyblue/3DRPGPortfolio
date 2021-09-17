using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_NamePlate : UI_Base
{
    enum GameObjects
    {
        HPBar,
        TargetName,
        TargetHp
        //TargetDegree
    }

    Stat _stat;
    BaseController _bc;
    private GameObject Plate;
    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, true);
        Bind<GameObject>(typeof(GameObjects));
        
        _bc = transform.parent.GetComponent<BaseController>();
        Plate = gameObject.transform.Find("Plater").gameObject;
        Plate.SetActive(false);
    }

    private void Update()
    {
        if(_bc._lockTarget != null){
            Plate.SetActive(true);
            _stat = _bc._lockTarget.transform.GetComponent<Stat>();
            float ratio = _stat.Hp / (float)_stat.MaxHp;
            SetHpRatio(ratio);
            Get<GameObject>((int)GameObjects.TargetName).GetComponent<Text>().text = _bc._lockTarget.name;
            Get<GameObject>((int)GameObjects.TargetHp).GetComponent<Text>().text = $"{_stat.Hp}/{_stat.MaxHp} ({ratio.ToString("0%")})";
                
            if(_stat.Hp <= 0)
                Plate.SetActive(false);
        }else{
            Plate.SetActive(false);
        }
    }

    public void SetHpRatio(float ratio)
    {
        GetObject((int)GameObjects.HPBar).GetComponent<Slider>().value = ratio;
    }
}
