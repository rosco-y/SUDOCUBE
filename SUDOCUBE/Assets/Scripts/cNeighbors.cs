using System;
using System.Collections;
using System.Collections.Generic;


public class cNeighbors
{
    public cColumnNeighbors Column { get; set; }
    public cRegionNeighbors Region { get; set; }
    SudoCube _thisCell;
    public cNeighbors(SudoCube thisCell)
    {
        _thisCell = thisCell;
        Column = new cColumnNeighbors(thisCell);
        Region = new cRegionNeighbors(thisCell);
    }
    internal void ClearNeighboringMarks(int sudoValue)
    {
        Column.ClearNeighboringMarks(sudoValue);
        Region.ClearNieghboringMarks(sudoValue);
    }
}
