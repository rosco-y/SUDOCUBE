using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
internal class cvsDataLoader
{
    StreamReader _rdr;
    string dataPathTemplate;
    public cvsDataLoader()
    {
        //dataPathTemplate = @"D:\PROJECTS\SUDOKUBE\DATA\";
        dataPathTemplate = g.PUZZLEPATH;
        //_rdr = new StreamReader(@"D:\PROJECTS\SUDOKUBE\DATA\");
    }


    internal void LoadData(int newGameNumber = 2)
    {
        allocateMemoryForPuzzleData();
        string dataPath = dataPathTemplate + newGameNumber.ToString() + ".csv";
        _rdr = new StreamReader(dataPath);

        try
        {
            string lineRead = _rdr.ReadLine();
            if (lineRead.StartsWith("LID"))
                lineRead = _rdr.ReadLine(); // read past header row
            do
            {
                parseReadLine(lineRead);
            }
            while ((lineRead = _rdr.ReadLine()) != null);
        }
        catch (Exception x)
        {
            throw new Exception($"Error in LoadData(): {x.Message}");
        }
    }
    private void parseReadLine(string lineRead)
    {
        int commaPos = lineRead.IndexOf(',');
        int lid = int.Parse(lineRead.Substring(0, commaPos));
        lineRead = lineRead.Substring(commaPos + 1);
        commaPos = lineRead.IndexOf(',');
        int rid = int.Parse(lineRead.Substring(0, commaPos));
        lineRead = lineRead.Substring(commaPos + 1);
        for (int c = 0; c < g.PSIZE; c++)
        {
            int value;
            if (lineRead.Contains(","))
            {
                commaPos = lineRead.IndexOf(',');
                value = int.Parse(lineRead.Substring(0, commaPos));
                //_puzzleData[lid][rid][c] = value;
                lineRead = lineRead.Substring(commaPos + 1);
            }
            else
                value = int.Parse(lineRead);

            g.Instance.PUZZLEDATA[lid][rid][c] = value;

        }
    }
    private void allocateMemoryForPuzzleData()
    {
        g.Instance.PUZZLEDATA = new int[g.PSIZE][][];
        for (int L = 0; L < g.PSIZE; L++)
        {
            g.Instance.PUZZLEDATA[L] = new int[g.PSIZE][];
            for (int R = 0; R < g.PSIZE; R++)
            {
                g.Instance.PUZZLEDATA[L][R] = new int[g.PSIZE];
            }
        }
    }
}