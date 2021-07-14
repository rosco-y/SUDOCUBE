using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[System.Serializable]
public class GameData
{
    int[][][] _gameData;
    int[][][][] _marks;
    int _numErrors;
    float _version; // so we don't have to attempt backwards compatibility
    int _curLayer;
    eFace _curFace;
    public GameData()
    {        
    }

    public float Version
    {
        get { return _version; }
    }

    public void LoadData()
    {
        _gameData = new int[g.PSIZE][][];
        _marks = new int[g.PSIZE][][][];
        for (int L = 0; L < g.PSIZE; L++)
        {
            _marks[L] = new int[g.PSIZE][][];
            _gameData[L] = new int[g.PSIZE][];
            for (int R = 0; R < g.PSIZE; R++)
            {
                _gameData[L][R] = new int[g.PSIZE];
                _marks[L][R] = new int[g.PSIZE][];
                for (int C = 0; C < g.PSIZE; C++)
                {
                    SudoCube copyCell = g.Instance.SudoCubes[L][R][C];
                    copyCell.COORDS = new cCoords(copyCell.ID);
                    _gameData[L][R][C] = copyCell.SudoCubeValue;
                    int markLen = copyCell.Marks.Count;
                    _marks[L][R][C] = new int[markLen];
                    int iMark = 0;
                    foreach (int mark in copyCell.Marks)
                    {
                        _marks[L][R][C][iMark++] = mark;
                    }
                }
            }
        }
        _numErrors = g.Instance.NumErrors;
        _version = g.VERSION;
        _curLayer = g.Instance.CurrentLayer;
        _curFace = g.Instance.CurrentSide.EFACE;
    }

    public void RestoreGameData()
    {
        g.Instance.NumErrors = _numErrors;
        //if (_version != g.VERSION)
        //    throw new System.Exception("Incorrect SAVEFILE Version.");

        for (int l = 0; l < g.PSIZE; l++)
        {
            for (int r = 0; r < g.PSIZE; r++)
            {
                for (int c = 0; c < g.PSIZE; c++)
                {
                    g.Instance.SudoCubes[l][r][c].SudoCubeValue = _gameData[l][r][c]; // restore SudoCubeValues
                }
            }
        }

        /// mark data restored after basic cell setup, so correct canvas's are in use.
        RestoreMarkData();
    }

    public void RestoreMarkData()
    {
        for (int l = 0; l < g.PSIZE; l++)
        {
            for (int r = 0; r < g.PSIZE; r++)
            {
                for (int c = 0; c < g.PSIZE; c++)
                {
                    for (int i = 0; i < _marks[l][r][c].Length; i++)
                    {
                        g.Instance.SudoCubes[l][r][c].AddMark(_marks[l][r][c][i]); // restore marks
                    }
                }
            }
        }
    }
}
