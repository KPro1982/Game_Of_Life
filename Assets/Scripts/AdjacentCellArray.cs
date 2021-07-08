using System;
using UnityEngine;

public class AdjacentCellArray
{
    public delegate bool AdjacentMethod(Vector2Int arrayPos, Vector3 _relativePosition);
    
    public CellElement[][] Cells = new CellElement[3][];


    public AdjacentCellArray(Cell _parentCell)
    {
        ParentCell = _parentCell;
        Cells[0] = new CellElement[3];
        Cells[1] = new CellElement[2];
        Cells[2] = new CellElement[3];
        
        IterateAdjacent(InitializeArray);
    }

    bool InitializeArray(Vector2Int arrayPos, Vector3 relativePos)
    {
        Cells[arrayPos.x][arrayPos.y] = new CellElement();
        return true;
    }

    public Cell ParentCell { get; set; }

    public void IterateAdjacent()
    {
        IterateAdjacent(CheckAdjacent);
    }
    public void IterateAdjacent(AdjacentMethod action)
    {
        Vector3 relativePos;
        Vector2Int arrayPos;

        int x, y, iiMax;

        for (var i = 0; i < 3; i++)
        {
            iiMax = i == 1 ? 2 : 3;
            for (var ii = 0; ii < iiMax; ii++)
            {
                arrayPos = new Vector2Int(i, ii);
                x = i - 1;

                if (i != 1)
                {
                    y = ii - 1;
                }
                else
                {
                    y = ii == 0 ? -1 : 1;
                }

                relativePos = new Vector3(x, y, 0); // assumes 2D so z is not necessary

                action(arrayPos, relativePos);
            }
        }
    }

    private bool CheckAdjacent(Vector2Int arrayPos, Vector3 relativePos)
    {
        var neighborPos = ParentCell.SnappedPosition + relativePos;
        if (!Helper.IsEmpty(neighborPos))
        {
            Cells[arrayPos.x][arrayPos.y].Cell = Helper.GetCell(neighborPos);
            Cells[arrayPos.x][arrayPos.y].EmptyValue = -1;
            Debug.Log($"Has Cell: {arrayPos}");
        }

        return true;
    }
}