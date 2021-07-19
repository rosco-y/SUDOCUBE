using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using TMPro;

using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] SudoCube _SudoCube; // my cube prefab
    [SerializeField] TMP_Text _statusBar;
    [SerializeField] GameObject _sudoCenter;
    SudoCube _clone;

    private void Awake()
    {
        // setup defaults and current environment
        g.Instance.InitialCameraPosition = Camera.main.transform.position;
        g.Instance.CurrentSide = new cCur(5, 6);
        g.SaveFile = Application.persistentDataPath + "/SAVEFILE.BIN";
        g.PUZZLEPATH = @"C:\PROJECTS\SUDOPROJECTS\DATA\PUZZLES\";
        //Application.quitting += quitGame;
        _statusBar.text = g.PUZZLEPATH;
    }

    void Start()
    {
        if (!File.Exists(g.SaveFile))
            NewGame();
        else
        {
            initializeGlobalArrays();
            instantiateAndPlaceSudoCubeObjects();
            if (!loadGame())
            {
                NewGame(); // if loadGame fails, start a new game.
            }
            makeNeighborLists();
        }
    }
    private bool loadGame()
    {
        bool restored = false;
        if (File.Exists(g.SaveFile))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            if (File.ReadAllBytes(g.SaveFile).Length > 0)
            {
                FileStream stream = new FileStream(g.SaveFile, FileMode.Open);
                GameData data = formatter.Deserialize(stream) as GameData;
                if (data.Version == g.VERSION)
                {
                    data.RestoreGameData();
                    restored = true;
                }
                stream.Close();
            }
        }
        return restored;
    }

    private void makeNeighborLists()
    {
        for (int L = 0; L < g.PSIZE; L++)
        {
            for (int R = 0; R < g.PSIZE; R++)
            {
                for (int C = 0; C < g.PSIZE; C++)
                {
                    SudoCube cell = g.Instance.SudoCubes[L][R][C];
                    cell.NEIGHBORS = new cNeighbors(cell);
                }
            }
        }
        g.Instance.SudoCubes[0][0][0].NEIGHBORS.Region.CreateRegionNeighborLists();
    }
    public void NewGame()
    {
        // DESTROY EXISTING SudoCube Prefabs.
        for (int L = 0; L < g.PSIZE; L++)
        {
            for (int R = 0; R < g.PSIZE; R++)
            {
                for (int C = 0; C < g.PSIZE; C++)
                {
                    if (g.Instance.SudoCubes[L][R][C] != null)
                    {
                        Destroy(g.Instance.SudoCubes[L][R][C]);
                        g.Instance.SudoCubes[L][R][C] = null;
                    }
                }
            }
        }
        initializeGlobalArrays();
        instantiateAndPlaceSudoCubeObjects();
        g.Instance.CreateLayers();

    }

    void initializeGlobalArrays()
    {
        g.Instance.SudoCubes = new SudoCube[g.PSIZE][][];
        g.Instance.PuzzleData = new int[g.PSIZE][][];
        for (int layer = 0; layer < g.PSIZE; layer++)
        {
            g.Instance.SudoCubes[layer] = new SudoCube[g.PSIZE][];
            g.Instance.PuzzleData[layer] = new int[g.PSIZE][];
            for (int row = 0; row < g.PSIZE; row++)
            {
                g.Instance.SudoCubes[layer][row] = new SudoCube[g.PSIZE];
                g.Instance.PuzzleData[layer][row] = new int[g.PSIZE];
            }
        }
    }

    private void instantiateAndPlaceSudoCubeObjects()
    {
        GameObject SudoCubeArray = new GameObject("SudoCubeArray");
        int psize = g.PSIZE;
        System.Random _rnd = new System.Random();
        for (int layer = 0; layer < psize; layer++)
        {
            for (int row = 0; row < psize; row++)
            {
                for (int col = 0; col < psize; col++)
                {
                    SudoCube cell = Instantiate(_SudoCube);
                    Vector3 pos = setPosition(layer, row, col);
                    cell.COORDS = new cCoords(layer, row, col);
                    cell.name = $"cell_{layer}{row}{col}";
                    int value = layer + 1;
                    if (_rnd.NextDouble() > .5)
                        value = -value;

                    cell.SudoCubeValue = value;
                    cell.transform.position = pos;
                    cell.transform.SetParent(SudoCubeArray.transform);
                    cell.tag = "SudoCube";
                    g.Instance.SudoCubes[layer][row][col] = cell;
                }
            }
        }
        g.ReadData(g.Instance.SudoCubes); // read csv data
        makeHoles(60);
        g.Instance.CopyDataToSudoKube();

    }
    private Vector3 setPosition(int layer, int row, int col)
    {
        Vector3 pos = new Vector3();
        pos.z = layer * _size + spaces(layer);
        pos.y = row * _size + spaces(row);
        pos.x = col * _size + spaces(col);
        return pos;
    }
    const float _size = 2.5f;
    

    private float spaces(int layerRowCol)
    {
        float offset = -5f;
        int numSpaces = layerRowCol / 3;
        return (float)numSpaces * (_size / 3) + offset;
    }

    private void makeHoles(float difficulty = 50)
    {

        difficulty = 100.0f - difficulty; // subtract arg from 100
        System.Random rnd = new System.Random();
        for (int l = 0; l < g.PSIZE; l++)
        {
            for (int r = 0; r < g.PSIZE; r++)
            {
                for (int c = 0; c < g.PSIZE; c++)
                {
                    if (rnd.Next(1, 100) > difficulty)
                    {
                        g.Instance.PUZZLEDATA[l][r][c] *= -1;
                    }
                }
            }
        }
    }
}
