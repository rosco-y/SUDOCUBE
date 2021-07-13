using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] int _sudoValue; // 1-9 and negative 1-9
    [SerializeField] SudoCube _cube; // my cube prefab
    //[SerializeField] int _numCubes;
    SudoCube _clone;



    void Start()
    {
        //_cube.SudoCellValue = _sudoValue;
        _clone = Instantiate(_cube);
        _clone.SudoCellValue = _sudoValue;
        _clone.transform.position = Vector3.zero;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print($"Update Count:");
            _clone.SudoCellValue += 1;
            _clone.SudoCellValue %= 10;
        }
    }
}
