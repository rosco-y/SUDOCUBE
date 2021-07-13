using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SudoGuessCanvasScript : MonoBehaviour
{
    [SerializeField] TMP_Text _errorGuess;
    [SerializeField] Canvas _sudoUnknownCanvas;
    [SerializeField] Canvas _sudoGuessCanvas;
    //[SerializeField] TMP_Text _sudoValueText;


    public int ErrorGuess
    {
        set
        {
            _errorGuess.text = value.ToString();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _sudoGuessCanvas.gameObject.SetActive(false);
            _sudoUnknownCanvas.gameObject.SetActive(true);
        }
    }
}
