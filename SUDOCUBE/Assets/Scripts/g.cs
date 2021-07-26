using System;
using System.Collections;
using TMPro;
using System.Collections.Generic;

using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class g : Singleton<g>
{
    protected g() { }  // optional, prevent non-singleton constructor use.

    #region CONSTANTS
    public static int PSIZE { get; } = 9; // puzzle is 9x9x9 array of SudoCubes.
    public static float VERSION { get; } = 1.0f;
    #endregion
    int _currentLayer;  // property access to protect it's legal values.
    public bool[] HiddenLayers { get; set; }
    public cCur CurrentSide;
    public int[][][] PUZZLEDATA;
    public static string PUZZLEPATH;
    public int HomeLayer { get; set; } = -1;
    public static string SaveFile { get; internal set; }
    public int NumErrors { get; internal set; }
    public static int PuzzleNumber { get; internal set; }
//    public Vector3 InitialSudoCenterPosition;

    #region SudoCube LAYERS
    public SudoCube[][][] SudoCubes;
    public int[][][] PuzzleData;
    public Dictionary<int, List<SudoCube>> FRONT2BACK_LAYERS;
    public Dictionary<int, List<SudoCube>> TOP2BOTTOM_LAYERS;
    public Dictionary<int, List<SudoCube>> LEFT2RIGHT_LAYERS;
    #endregion

    public int CurrentLayer
    {
        get
        {
            return _currentLayer;
        }
        set
        {
            if (value < 0)
                _currentLayer = 0;
            else if (value > g.PSIZE - 1)
            {
                _currentLayer = g.PSIZE - 1;
            }
            else
                _currentLayer = value;
        }
    }

    public Vector3 InitialCameraPosition { get; internal set; }




    /// <summary>
    /// Instantiate FRONT2BACK_LAYERS, LEFT2RIGHT_LAYERS and TOP2BOTTOM_LAYERS
    /// </summary>
    public void CreateLayers()
    {

        g.Instance.FRONT2BACK_LAYERS = new Dictionary<int, List<SudoCube>>();
        g.Instance.LEFT2RIGHT_LAYERS = new Dictionary<int, List<SudoCube>>();
        g.Instance.TOP2BOTTOM_LAYERS = new Dictionary<int, List<SudoCube>>();
        for (int i = 0; i < g.PSIZE; i++)
        {
            g.Instance.FRONT2BACK_LAYERS.Add(i, new List<SudoCube>());
            g.Instance.LEFT2RIGHT_LAYERS.Add(i, new List<SudoCube>());
            g.Instance.TOP2BOTTOM_LAYERS.Add(i, new List<SudoCube>());

        }
        for (int layer = 0; layer < g.PSIZE; layer++)
        {

            for (int row = 0; row < g.PSIZE; row++)
            {
                for (int col = 0; col < g.PSIZE; col++)
                {
                    SudoCube cell = g.Instance.SudoCubes[layer][row][col];
                    g.Instance.FRONT2BACK_LAYERS[layer].Add(cell);
                    g.Instance.TOP2BOTTOM_LAYERS[row].Add(cell);
                    g.Instance.LEFT2RIGHT_LAYERS[col].Add(cell);
                }
            }
        }

        giveNameToCenterCells();
    }

    internal void CopyDataToSudoKube()
    {
        for (int layer = 0; layer < PSIZE; layer++)
        {
            for (int row = 0; row < PSIZE; row++)
            {
                for (int col = 0; col < PSIZE; col++)
                {
                    SudoCubes[layer][row][col].SudoCubeValue = PUZZLEDATA[layer][row][col];
                    SudoCubes[layer][row][col].RemoveMarks();
                }
            }
        }
    }

    internal static void ReadData(SudoCube[][][] SudoCubes)
    {
        cvsDataLoader loader = new cvsDataLoader();
        loader.LoadData();
    }

    public bool RestoreSavedGame()
    {
        bool restored = false;
        if (File.Exists(SaveFile))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(SaveFile, FileMode.Open);
            GameData data = formatter.Deserialize(stream) as GameData;
            data.RestoreGameData();
            restored = true;
            stream.Close();
        }
        return restored;
    }

    void giveNameToCenterCells()
    {
        var LAYERS = FRONT2BACK_LAYERS;
        for (int layer = 0; layer < LAYERS.Count; layer++)
        {
            foreach (var CELL in LAYERS[layer])
            {
                if (CELL.name.EndsWith("44"))
                    CELL.name = "Z" + CELL.COORDS;
            }
        }
        LAYERS = LEFT2RIGHT_LAYERS;
        for (int layer = 0; layer < LAYERS.Count; layer++)
        {
            foreach (var CELL in LAYERS[layer])
            {
                if (CELL.name.EndsWith("44"))
                    CELL.name = "X" + CELL.COORDS;
            }
        }
    }

}
