using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;

public class SudoCube : MonoBehaviour, IPointerClickHandler
{
    int _sudoCellValue;
    [SerializeField] Canvas _sudoValueCanvas;
    [SerializeField] Canvas _unkCanvas;
    [SerializeField] TMP_Text _sudoValueText;
    Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    #region UNCOMMENT WHEN USING IPointerClickHandler
    int _sudoButtonValue;
    public void OnPointerClick(PointerEventData eventData)
    {

        TMP_Text t = GetComponentInChildren<TMP_Text>();
        _sudoButtonValue = int.Parse(t.text);
        if (eventData.button == PointerEventData.InputButton.Right)
            rightClick();
        else if (eventData.button == PointerEventData.InputButton.Left)
            leftClick();
        else if (eventData.button == PointerEventData.InputButton.Middle)
            middleClick();

    }

    private void middleClick()
    {
        print($"middle-click {_sudoButtonValue}.");
    }

    private void leftClick()
    {
        print($"left-click {_sudoButtonValue}.");
    }

    private void rightClick()
    {
        print($"right-click {_sudoButtonValue}.");
    }
    #endregion

    public int SudoCellValue
    {
        set
        {
            _sudoCellValue = value;

            bool showValue = _sudoCellValue > 0;
            _unkCanvas.enabled = !showValue;
            _sudoValueCanvas.enabled = showValue;
            _sudoValueText.text = Mathf.Abs(_sudoCellValue).ToString();
        }
        get
        {
            return _sudoCellValue;
        }
    }


    private void Update()
    {        
        transform.LookAt(_mainCamera.transform);
    }

    
    

}
