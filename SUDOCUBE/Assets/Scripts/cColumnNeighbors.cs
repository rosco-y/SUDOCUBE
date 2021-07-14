using System;
using System.Collections;
using System.Collections.Generic;

public class cColumnNeighbors
{
    public List<SudoCube> HEIGHT { get; set; }
    public List<SudoCube> WIDTH { get; set; }
    public List<SudoCube> DEPTH { get; set; }
    SudoCube _thisCell;

    public cColumnNeighbors(SudoCube thisCell)
    {
        _thisCell = thisCell;
        HEIGHT = new List<SudoCube>();
        WIDTH = new List<SudoCube>();
        DEPTH = new List<SudoCube>();
        CreateColumnNeighborLists();
    }

    internal void ClearNeighboringMarks(int sudoValue)
    {
        foreach (SudoCube cell in HEIGHT)
            cell.RemoveMark(sudoValue);
        foreach (SudoCube cell in WIDTH)
            cell.RemoveMark(sudoValue);
        foreach (SudoCube cell in DEPTH)
            cell.RemoveMark(sudoValue);
    }

    internal void checkForSuccess()
    {
        displaySuccess(HEIGHT);
        displaySuccess(WIDTH);
        displaySuccess(DEPTH);
    }

    private void displaySuccess(List<SudoCube> CELLS)
    {
        //int sum = 0;
        //foreach (SudoCube cell in CELLS)
        //    sum += cell.SudoValue;

        //if (sum == 45)
        //    foreach (SudoCube cell in CELLS)
        //        cell.successDisplay();
    }



    /************************
    /* g.Instance.SudoCubes[D][H][W] *
    /***********************/
    private void CreateColumnNeighborLists()
    {
        CreateDepthLists();
        CreateHeightLists();
        CreateWidthLists();
    }



    private void CreateWidthLists()
    {
        /* if col == 0, build entire list */
        if (_thisCell.COORDS.COL == 0)
        {
            int iLayer = _thisCell.COORDS.LAYER;
            int iRow = _thisCell.COORDS.ROW;
            for (int col = 0; col < g.PSIZE; col++)
            {                
                WIDTH.Add(g.Instance.SudoCubes[iLayer][iRow][col]);
            }
        }
        else
        {
            // simply point at completed list.
            WIDTH = g.Instance.SudoCubes[_thisCell.COORDS.LAYER][_thisCell.COORDS.ROW][0].NEIGHBORS.Column.WIDTH;
        }
    }

    private void CreateHeightLists()
    {
        /* IF row == 0, build entire list */
        if (_thisCell.COORDS.ROW == 0)
        {
            int iLayer = _thisCell.COORDS.LAYER;
            int iCol = _thisCell.COORDS.COL;
            for (int row = 0; row < g.PSIZE; row++)
            {
                SudoCube cell = g.Instance.SudoCubes[iLayer][row][iCol];
                HEIGHT.Add(cell);
            }
        }
        else
        {
            /* set this cell's Width reference to the
             * completed list found at ROW 0 */
            HEIGHT = g.Instance.SudoCubes[_thisCell.COORDS.LAYER][0][_thisCell.COORDS.COL].NEIGHBORS.Column.HEIGHT;
        }
    }

    private void CreateDepthLists()
    {
        // DEPTH
        /* IF thisCell is on layer0, then the depth
         * is traversed, adding all cells in the depth
         * to the list */
        if (_thisCell.COORDS.LAYER == 0)
        {
            int iRow = _thisCell.COORDS.ROW;
            int iCol = _thisCell.COORDS.COL;
            for (int layer = 0; layer < g.PSIZE; layer++)
            {
                SudoCube cell = g.Instance.SudoCubes[layer][iRow][iCol];
                DEPTH.Add(cell);
            }
        }
        /*
         * Else thisCell is not on layer 0, so simply point 
         * thisCell's DEPTH list reference to the cell in this
         * row/col's layer 0, since the list on layer 0 contains
         * a completed DEPTH list.
         */
        else
        {
            DEPTH = g.Instance.SudoCubes[0][_thisCell.COORDS.ROW][_thisCell.COORDS.COL].NEIGHBORS.Column.DEPTH;
        }
    }
}
