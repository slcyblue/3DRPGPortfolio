using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_BuffIcon : UI_BuffPlate, IPointerClickHandler
{
    public override void Init()
    {

    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right){
            //우클릭시 스킬 버프 삭제
            Debug.Log("버프 클릭");
        }
    }
}
