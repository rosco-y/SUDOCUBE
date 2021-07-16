using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class LayerLabels : MonoBehaviour
{
    [SerializeField] TMP_Text[] _labels;// { get; private set; }
    int _currentLayer;
    Color32 _highLight = new Color32(238, 255, 0, 255);
    Color32 _white = new Color32(255, 255, 255, 255);
    bool _currentLayerSet = false;

    private void FixedUpdate()
    {
        if (!_currentLayerSet)
        {
            _currentLayer = g.Instance.CurrentLayer;
            setLayerLabels();
            _currentLayerSet = true;

        }
        else
        {
            if (_currentLayer != g.Instance.CurrentLayer)
            {
                _currentLayer = g.Instance.CurrentLayer;
                setLayerLabels();
            }
        }
    }

    public int CurrentLayer
    {
        set
        {
            _currentLayer = value;
            setLayerLabels();
        }
        get
        {
            return _currentLayer;
        }
    }

    public TMP_Text[] Labels
    {
        set
        {
            _labels = value;
        }
        get
        {
            return _labels;
        }
    }

    private void OnMouseDown()
    {
        string sNo = GetComponent<TMP_Text>().text;
    }

    public void setLayerLabels()
    {
        int i = 0;
        try
        {
            for (i = 0; i < g.PSIZE; i++)
            {
                string s = (i + 1).ToString();
                if (i == g.Instance.HomeLayer)
                {
                    s += "*";
                }
                if (i >= CurrentLayer || i == g.Instance.HomeLayer)
                {
                    if (i == CurrentLayer || i == g.Instance.HomeLayer)
                    {

                        if (i == g.Instance.CurrentLayer)
                        {
                            Labels[i].text = $"[{s}]";
                            Labels[i].color = _highLight; // only highlight current layer
                        }
                        else
                        {
                            Labels[i].text = $" {s}";
                            Labels[i].color = _white;
                        }
                    }
                    else
                    {
                        Labels[i].text = $" {s}";
                        Labels[i].color = _white;
                    }
                }
                else
                    Labels[i].text = String.Empty;
            }
        }
        catch (Exception x)
        {

            throw new Exception($"setLayerLabels index out of bounds: {i}");
        }
    }

    public void ShowLabels()
    {
        setLayerLabels();
    }
}
