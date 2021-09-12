using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public Action<Define.KeyEvent> KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null;

    bool _pressedKey = false;
    float _pressedKeyTime = 0;
    bool _pressedRMouse = false;
    bool _pressedLMouse = false;
    bool _ClickedRMouse = false;
    float _pressedRMouseTime = 0;
    float _ClickedRMouseTime = 0;
    float _pressedLMouseTime = 0;

    public void OnUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.anyKey && KeyAction != null){
            if(Input.GetKeyDown(KeyCode.Q)){
                if (!_pressedKey){
                    {
                        KeyAction.Invoke(Define.KeyEvent.KeyDown);
                        _pressedKeyTime = Time.time;
                    }
                    KeyAction.Invoke(Define.KeyEvent.Press);
                    _pressedKey = true;
                }
            }
            else
            {
                if (_pressedKey)
                {
                    if (Time.time < _pressedKeyTime + 0.2f)
                        KeyAction.Invoke(Define.KeyEvent.Click);
                    KeyAction.Invoke(Define.KeyEvent.KeyUp);
                }
                _pressedKey = false;
                _pressedKeyTime = 0;
            }
            if(Input.GetKeyDown(KeyCode.W)){
                if (!_pressedKey){
                    {
                        KeyAction.Invoke(Define.KeyEvent.KeyDown);
                        _pressedKeyTime = Time.time;
                    }
                    KeyAction.Invoke(Define.KeyEvent.Press);
                    _pressedKey = true;
                }
            }
            else
            {
                if (_pressedKey)
                {
                    if (Time.time < _pressedKeyTime + 0.2f)
                        KeyAction.Invoke(Define.KeyEvent.Click);
                    KeyAction.Invoke(Define.KeyEvent.KeyUp);
                }
                _pressedKey = false;
                _pressedKeyTime = 0;
            }
            if(Input.GetKeyDown(KeyCode.E)){
                if (!_pressedKey){
                    {
                        KeyAction.Invoke(Define.KeyEvent.KeyDown);
                        _pressedKeyTime = Time.time;
                    }
                    KeyAction.Invoke(Define.KeyEvent.Press);
                    _pressedKey = true;
                }
            }
            else
            {
                if (_pressedKey)
                {
                    if (Time.time < _pressedKeyTime + 0.2f)
                        KeyAction.Invoke(Define.KeyEvent.Click);
                    KeyAction.Invoke(Define.KeyEvent.KeyUp);
                }
                _pressedKey = false;
                _pressedKeyTime = 0;
            }
            if(Input.GetKeyDown(KeyCode.R)){
                if (!_pressedKey){
                    {
                        KeyAction.Invoke(Define.KeyEvent.KeyDown);
                        _pressedKeyTime = Time.time;
                    }
                    KeyAction.Invoke(Define.KeyEvent.Press);
                    _pressedKey = true;
                }
            }
            else
            {
                if (_pressedKey)
                {
                    if (Time.time < _pressedKeyTime + 0.2f)
                        KeyAction.Invoke(Define.KeyEvent.Click);
                    KeyAction.Invoke(Define.KeyEvent.KeyUp);
                }
                _pressedKey = false;
                _pressedKeyTime = 0;
            }
            if(Input.GetKeyDown(KeyCode.I)){
                if (!_pressedKey){
                    {
                        KeyAction.Invoke(Define.KeyEvent.KeyDown);
                        _pressedKeyTime = Time.time;
                    }
                    KeyAction.Invoke(Define.KeyEvent.Press);
                    _pressedKey = true;
                }
            }
            else
            {
                if (_pressedKey)
                {
                    if (Time.time < _pressedKeyTime + 0.2f)
                        KeyAction.Invoke(Define.KeyEvent.Click);
                    KeyAction.Invoke(Define.KeyEvent.KeyUp);
                }
                _pressedKey = false;
                _pressedKeyTime = 0;
            }
            if(Input.GetKeyDown(KeyCode.C)){
                if (!_pressedKey){
                    {
                        KeyAction.Invoke(Define.KeyEvent.KeyDown);
                        _pressedKeyTime = Time.time;
                    }
                    KeyAction.Invoke(Define.KeyEvent.Press);
                    _pressedKey = true;
                }
            }
            else
            {
                if (_pressedKey)
                {
                    if (Time.time < _pressedKeyTime + 0.2f)
                        KeyAction.Invoke(Define.KeyEvent.Click);
                    KeyAction.Invoke(Define.KeyEvent.KeyUp);
                }
                _pressedKey = false;
                _pressedKeyTime = 0;
            }
        }if(Input.GetKeyDown(KeyCode.K)){
                if (!_pressedKey){
                    {
                        KeyAction.Invoke(Define.KeyEvent.KeyDown);
                        _pressedKeyTime = Time.time;
                    }
                    KeyAction.Invoke(Define.KeyEvent.Press);
                    _pressedKey = true;
                }
            }
            else
            {
                if (_pressedKey)
                {
                    if (Time.time < _pressedKeyTime + 0.2f)
                        KeyAction.Invoke(Define.KeyEvent.Click);
                    KeyAction.Invoke(Define.KeyEvent.KeyUp);
                }
                _pressedKey = false;
                _pressedKeyTime = 0;
            }

        if (MouseAction != null)
        {
            if (Input.GetMouseButton(1))
            {
                if (!_pressedRMouse)
                {
                    MouseAction.Invoke(Define.MouseEvent.PointerDown);
                    _pressedRMouseTime = Time.time;
                }
                MouseAction.Invoke(Define.MouseEvent.Press);
                _pressedRMouse = true;
            }else
            {
                if (_pressedRMouse)
                {
                    if (Time.time < _pressedRMouseTime + 0.2f){
                        MouseAction.Invoke(Define.MouseEvent.Click);
                    }
                    MouseAction.Invoke(Define.MouseEvent.PointerUp);
                }
                _pressedRMouse = false;
                _pressedRMouseTime = 0;
            }
            
            if (Input.GetMouseButton(0))
            {
                if (!_pressedLMouse)
                {
                    MouseAction.Invoke(Define.MouseEvent.PointerDown);
                    _pressedLMouseTime = Time.time;
                }
                MouseAction.Invoke(Define.MouseEvent.Press);
                _pressedLMouse = true;
            }
            else
            {
                if (_pressedLMouse)
                {
                    if (Time.time < _pressedLMouseTime + 0.2f)
                        MouseAction.Invoke(Define.MouseEvent.Click);
                    MouseAction.Invoke(Define.MouseEvent.PointerUp);
                }
                _pressedLMouse = false;
                _pressedLMouseTime = 0;
            }
        }
    }

    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }
}
