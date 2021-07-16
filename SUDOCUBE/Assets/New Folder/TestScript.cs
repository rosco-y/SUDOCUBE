using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] SudoCube _cube;
    [SerializeField] int _sudoValue;
    void Start()
    {
        SudoCube cube = Instantiate(_cube);
        cube.transform.position = Vector3.zero;
        cube.SudoCubeValue = _sudoValue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
