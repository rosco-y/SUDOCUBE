using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;

public class unkButton : MonoBehaviour, IPointerClickHandler
{
    //*
    [SerializeField] Canvas _errorGuessCanvas;
    [SerializeField] Canvas _sudoUnknownCanvas;
    [SerializeField] SudoGuessCanvasScript _sudoGuessCanvasScript;

    int _buttonNo;
    int _id;

    private void Awake()
    {
        _sudoUnknownCanvas = GetComponentInParent<Canvas>();
        _id = GetComponentInParent<SudoCube>().ID;        
    }

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
        print($"middle-click {_id}-{_buttonNo}.");
    }

    private void leftClick()
    {
        //  check if _buttoNo is the SudoCube's given
        print($"left-click {_id}-{_buttonNo}.");
        SudoCube parent = GetComponentInParent<SudoCube>();
        if (_buttonNo == Mathf.Abs( parent.SudoCubeValue))
        {
            parent.SudoCubeValue = _buttonNo;
        }
        else
        {
            
            _sudoUnknownCanvas.gameObject.SetActive(false);
            _errorGuessCanvas.gameObject.SetActive(true);
            _sudoGuessCanvasScript.ErrorGuess = _buttonNo;
        }
    }

   

    private void rightClick()
    {
        print($"right-click {_id}-{_buttonNo}.");
    }
    //*/
}
