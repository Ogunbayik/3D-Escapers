using System;
using UnityEngine;

public class GridCell 
{
    public event Action<GridStatus> OnGridStatusChanged;
    public int CellID { get; private set; }
    public int Height { get; private set; }
    public int Width { get; private set; }
    public GridType CellType;
    public GridStatus GridStatus;

    public GridCell(int cellID,int width, int height, GridType cellType, GridStatus gridStatus)
    {
        CellID = cellID;
        Width = width;
        Height = height;
        CellType = cellType;
        GridStatus = gridStatus;
    }   
    public void SetCellType(GridType cellType) => CellType = cellType;
    public void SetGridStatus(GridStatus newStatus)
    {
        if (GridStatus == newStatus)
            return;

        GridStatus = newStatus;
        OnGridStatusChanged?.Invoke(newStatus);
    }
}
