using System;
using UnityEngine;

public class GridCell 
{
    public event Action<ColorType> OnColorChanged;
    public int Height { get; private set; }
    public int Width { get; private set; }
    public CellType CellType;
    public ColorType ColorType;

    public GridCell(int width, int height, CellType cellType)
    {
        Width = width;
        Height = height;
        CellType = cellType;
    }
    public void SetCellType(CellType cellType) => CellType = cellType;
    public void SetCellColor(ColorType colorType)
    {
        if (ColorType == colorType)
            return;

        ColorType = colorType;
        OnColorChanged?.Invoke(colorType);
    }
}
