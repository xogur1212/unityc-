using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public Action KeyAction = null;         //키액션  델리게이트 몰루 https://jeong-pro.tistory.com/51
    public Action<Define.MouseEvent> MouseAction = null;        //마우스액션 디자인패턴에서 리스너패턴 몰루 https://developyo.tistory.com/134

    bool _pressed = false;
    float _pressedTime = 0; 

    public void OnUpdate()      //monobehavior가아니라서
    {
        if (EventSystem.current.IsPointerOverGameObject())  //eventsystem.current 현재 eventsystem을 반환 .ispoint~ 해당포인터가  오브젝트위에 있는지여부체크
            return;

        if (Input.anyKey && KeyAction != null) //키나 키액션 입력있으면
				KeyAction.Invoke(); //invoke 전파

        if (MouseAction != null)
        {
            if (Input.GetMouseButton(0))    //왼쪽클릭
            {
                if (!_pressed)
                {
                    MouseAction.Invoke(Define.MouseEvent.PointerDown);
                    _pressedTime = Time.time;
                }
                MouseAction.Invoke(Define.MouseEvent.Press);
                _pressed = true;
            }
            else//클릭을떗을떄 이벤트발생
            {
                if (_pressed)
                {
                    if (Time.time < _pressedTime + 0.2f)
                        MouseAction.Invoke(Define.MouseEvent.Click);
                    MouseAction.Invoke(Define.MouseEvent.PointerUp);
                    
                }
                  
                _pressed = false;
                _pressedTime = 0;   
            }
            //드래그여기에추가해영~
        }
    }

    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }
}
