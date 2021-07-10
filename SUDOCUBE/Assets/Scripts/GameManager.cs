using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int _sudoValue;
    [SerializeField] SudoCube _cube;
    void Start()
    {
        SudoCube clone = Instantiate(_cube);
        _cube.SudoCellValue = _sudoValue;
        _cube.transform.position = Vector3.zero;
    }

  
}
