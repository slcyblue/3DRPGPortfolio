using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SetCount : UI_Popup
{
    public InputField inputField; 
    public Button confirmButton;
    public Button quitButton;
    public Slider slider;
    public int _count;
    int min = 0;
    int max = 100;
    public UI_Shop_Item shopItem;
    public override void Init()
    {
        base.Init();
        inputField = transform.GetChild(3).GetComponent<InputField>();
        slider = transform.GetChild(4).GetComponent<Slider>();
        confirmButton = transform.GetChild(5).GetComponent<Button>();
        quitButton = transform.GetChild(6).GetComponent<Button>();
        inputField.contentType = InputField.ContentType.IntegerNumber;
        inputField.placeholder.GetComponent<Text>().text = "갯수를 입력하세요";

        slider.wholeNumbers = true;
        slider.minValue = min;
        slider.maxValue = max;

        SetFunction_UI();
    }

    private void SetFunction_UI()
    {
        //Reset
        ResetFunction_UI();

        confirmButton.onClick.AddListener(Function_ConfirmButton);
        quitButton.onClick.AddListener(Function_QuitButton);
        inputField.onValueChanged.AddListener(Function_InputField); 
        inputField.onEndEdit.AddListener(Function_InputField_EndEdit); 
        slider.onValueChanged.AddListener(Function_Slider);
    }
    
    
    private void Function_InputField(string _data)
    {
        slider.value = int.Parse(_data);
    }

    private void Function_InputField_EndEdit(string _data)
    {
        slider.value = int.Parse(_data);
    }
    private void Function_Slider(float _data){
        slider.value = _data;
        inputField.text = _data.ToString();
    }

    private void ResetFunction_UI()
    {
        confirmButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
        slider.onValueChanged.RemoveAllListeners();
        inputField.onValueChanged.RemoveAllListeners();
        inputField.onEndEdit.RemoveAllListeners();
    }
    private void Function_ConfirmButton()
    {
        _count = int.Parse(inputField.text);
        shopItem.purchase(_count);
    }
    private void Function_QuitButton(){
        Managers.UI.ClosePopupUI();
    }
}
