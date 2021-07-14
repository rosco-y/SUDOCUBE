using System.Collections;
using System.Collections.Generic;

public class cCoords
{
    public int LAYER { get; set; }
    public int ROW { get; set; }
    public int COL { get; set; }

    public cCoords(int layer, int row, int col)
    {
        LAYER = layer;
        ROW = row;
        COL = col;
    }


    /******************************************
     * Each cell has an id constructed from it's
     * layer, row and col using the following
     * formula:
     * ID = LAYER * 100 + ROW * 10 + COL;
     * This constructor takes one integer, 
     * (the ID), and instantiates a cCoord object
     * by reverse engineering the LAYER, ROW and
     * COL.
     *****************************************/
    public cCoords(int id)
    {
        LAYER = id / 100; // integer math, disregard leftover.
        ROW = ((id - LAYER * 100)) / 10;
        COL = (id - (LAYER * 100 + ROW * 10));
    }
    public override string ToString()
    {
        return $"{LAYER}{ROW}{COL}";
    }

}
