using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;

public class unkButton : MonoBehaviour, IPointerClickHandler
{
    //*
    int _buttonNo;
    public void OnPointerClick(PointerEventData eventData)
    {
        TMP_Text t = GetComponentInChildren<TMP_Text>();
        _buttonNo = int.Parse(t.text);
        if (eventData.button == PointerEventData.InputButton.Right)
            rightClick();
        else if (eventData.button == PointerEventData.InputButton.Left)
            leftClick();
        else if (eventData.button == PointerEventData.InputButton.Middle)
            middleClick();
    }

    private void middleClick()
    {
        print($"middle-click {_buttonNo}.");
    }

    private void leftClick()
    {
        print($"left-click {_buttonNo}.");
    }

    private void rightClick()
    {
        print($"right-click {_buttonNo}.");
    }
    //*/
}
