
public class GridCell 
{
    public int Height { get; private set; }
    public int Width { get; private set; }
    public GridType GridType;

    public GridCell(int width, int height, GridType gridType)
    {
        Width = width;
        Height = height;
        GridType = gridType;
    }
    public void SetGridType(GridType gridType) => GridType = gridType;
}
