using System;
using System.Collections;
using System.Collections.Generic;

public class cRegionNeighbors
{
    List<SudoCube> TOP { get; set; }
    List<SudoCube> FRONT { get; set; }
    List<SudoCube> SIDE { get; set; }
    SudoCube _thisCell;
    public cRegionNeighbors(SudoCube thisCell)
    {
        _thisCell = thisCell;
        TOP = new List<SudoCube>();
        FRONT = new List<SudoCube>();
        SIDE = new List<SudoCube>();
        // CreateRegionNeighborLists();
    }

    public void CreateRegionNeighborLists()
    {
        for (int Layer = 0; Layer < g.PSIZE; Layer++)
        {
            for (int R = 0; R <= 6; R += 3)
            {
                for (int C = 0; C <= 6; C += 3)
                {
                    CreateFrontLists(Layer, R, C);
                }
            }
        }
        for (int COL = 0; COL < g.PSIZE; COL++)
        {
            for (int L = 0; L <= 6; L += 3)
            {
                for (int R = 0; R <= 6; R += 3)
                {
                    CreateSideLists(L, R, COL);
                }
            }
        }

        for (int ROW = 0; ROW < g.PSIZE; ROW++)
        {
            for (int L = 0; L <= 6; L += 3)
            {
                for (int C = 0; C <= 6; C += 3)
                {
                    CreateTopLists(L, ROW, C);
                }
            }
        }
    }

    internal void ClearNieghboringMarks(int sudoValue)
    {
        // use the absolute value so we aren't trying to 
        // match a negative because cell is a hole.
        foreach (SudoCube cell in TOP)
            cell.RemoveMark(Math.Abs(sudoValue));
        foreach (SudoCube cell in FRONT)
            cell.RemoveMark(Math.Abs(sudoValue));
        foreach (SudoCube cell in SIDE)
            cell.RemoveMark(Math.Abs(sudoValue));
    }

    private void CreateTopLists(int layer, int ROW, int col)
    {
        int layerCorner = -1;
        int colCorner = -1;
        List<SudoCube> topRegion = null;
        for (int Layer = layer; Layer < layer + 3; Layer++)
        {
            for (int Col = col; Col < col + 3; Col++)
            {
                if (Layer % 3 == 0 && Col % 3 == 0)
                {
                    layerCorner = Layer; colCorner = Col;
                    buildTopRegion(layerCorner, ROW, colCorner);
                    topRegion = g.Instance.SudoCubes[layerCorner][ROW][colCorner].NEIGHBORS.Region.TOP;
                }
                else
                {
                    g.Instance.SudoCubes[Layer][ROW][Col].NEIGHBORS.Region.TOP = topRegion;
                }
            }
        }
    }
    private void buildTopRegion(int layerCorner, int ROW, int colCorner)
    {
        List<SudoCube> regionList = g.Instance.SudoCubes[layerCorner][ROW][colCorner].NEIGHBORS.Region.TOP;
        for (int ilayer = layerCorner; ilayer < layerCorner + 3; ilayer++)
        {
            for (int icol = colCorner; icol < colCorner + 3; icol++)
            {
                regionList.Add(g.Instance.SudoCubes[ilayer][ROW][icol]);
            }
        }
    }
    private void buildSideRegion(int layerCorner, int rowCorner, int COL)
    {
        List<SudoCube> regionList = g.Instance.SudoCubes[layerCorner][rowCorner][COL].NEIGHBORS.Region.SIDE;
        for (int irow = rowCorner; irow < rowCorner + 3; irow++)
        {
            for (int ilayer = layerCorner; ilayer < layerCorner + 3; ilayer++)
            {
                SudoCube cell = g.Instance.SudoCubes[ilayer][irow][COL];
                regionList.Add(cell);
            }
        }
    }
    private void CreateSideLists(int layer, int row, int COL)
    {
        // preserve layer corners
        // initialized illegal to ensure
        // we start with a valid corner.
        int rowCorner = -1;
        int layerCorner = -1;
        List<SudoCube> sideRegion = null;
        // Walk in and out on the sides, looking for corners on the sides.
        // the sides are on the columns, so we'll process each column prior
        // to moving to the next.
        for (int Layer = layer; Layer < layer + 3; Layer++)
        {
            for (int Row = row; Row < row + 3; Row++)
            {
                if (Layer % 3 == 0 && Row % 3 == 0)
                {
                    rowCorner = Row; layerCorner = Layer;
                    // build a list based on this corner
                    buildSideRegion(Layer, Row, COL);
                    sideRegion = g.Instance.SudoCubes[layerCorner][rowCorner][COL].NEIGHBORS.Region.SIDE;
                }
                else
                {
                    // set pointing to this regions built list
                    g.Instance.SudoCubes[Layer][Row][COL].NEIGHBORS.Region.SIDE = sideRegion;

                }
            }

        }
    }
    private void CreateFrontLists(int Layer, int row, int col)
    {
        // preserve layer corners
        // initialized illegal to ensure
        // we start with a valid corner.
        int rowCorner = -1;
        int colCorner = -1;
        for (int Row = row; Row < row + 3; Row++)
        {
            for (int Col = col; Col < col + 3; Col++)
            {
                if (Row % 3 == 0 && Col % 3 == 0) // corner?
                {
                    rowCorner = Row; colCorner = Col;
                    buildFrontRegion(Layer, Row, Col);
                }
                else
                {
                    SudoCube cell = g.Instance.SudoCubes[Layer][Row][Col];
                    cell.NEIGHBORS.Region.FRONT = g.Instance.SudoCubes[Layer][rowCorner][colCorner].NEIGHBORS.Region.FRONT;
                }
            }
        }
    }
    private void buildFrontRegion(int layer, int row, int col)
    {
        for (int irow = row; irow < row + 3; irow++)
        {
            for (int icol = col; icol < col + 3; icol++)
            {
                SudoCube cell = g.Instance.SudoCubes[layer][irow][icol];
                g.Instance.SudoCubes[layer][row][col].NEIGHBORS.Region.FRONT.Add(cell);
            }
        }
    }
   

}