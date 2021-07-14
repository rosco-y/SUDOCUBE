using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;

public class SudoCube : MonoBehaviour, IPointerClickHandler
{
    public int ID { get; private set; }
    int _SudoCubeValue;
    [SerializeField] Canvas _sudoValueCanvas;
    [SerializeField] Canvas _unkCanvas;
    [SerializeField] TMP_Text _sudoValueText;
    Camera _mainCamera;
    public List<int> Marks;
    Color32 _red = new Color32(255, 0, 0, 255);
    Color32 _black = new Color32(0, 0, 0, 255);
    public cCoords _coords;
    public cNeighbors NEIGHBORS { get; set; }

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

    internal void RemoveMark(int mark)
    {

        foreach (unkButton btn in _unkCanvas.GetComponentsInChildren<unkButton>())
        {
            if (btn.name.Contains(mark.ToString()))
            {
                // clear this mark from this unkButton.
                Marks.Remove(mark);
                TMP_Text text = btn.GetComponentInChildren<TMP_Text>();
                text.color = _black;
                text.fontStyle = FontStyles.Normal;
            }
        }
    }

    public int SudoCubeValue
    {
        set
        {
            _SudoCubeValue = value;

            bool showValue = _SudoCubeValue > 0;
            _unkCanvas.enabled = !showValue;
            _sudoValueCanvas.enabled = showValue;
            _sudoValueText.text = Mathf.Abs(_SudoCubeValue).ToString();
            _sudoValueText.enabled = showValue;
        }
        get
        {
            return _SudoCubeValue;
        }
    }


    private void Update()
    {        
        transform.LookAt(_mainCamera.transform);
    }

    public cCoords COORDS
    {
        get { return _coords; }
        set
        {
            _coords = value;
            // generate unique ID for SudoCell.
            ID = _coords.LAYER * 100 + _coords.ROW * 10 + _coords.COL;
        }
    }

    internal void RemoveMarks()
    {
        for (int i = 1; i <= g.PSIZE; i++)
        {
            RemoveMark(i);
        }
    }

    internal void AddMark(int mark)
    {
        // TODO -- COLOR MARKED BUTTON
        if (!Marks.Contains(mark))
        {
            Marks.Add(mark);
            foreach (unkButton button in _unkCanvas.GetComponentsInChildren<unkButton>())
            {
                {
                    if (button.name.Contains(mark.ToString()))
                    {
                        TMP_Text text = button.GetComponentInChildren<TMP_Text>();
                        text.color = _red;
                        text.fontStyle = FontStyles.Bold;
                    }

                }
            }
        }
    }
}
