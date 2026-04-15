

public static class GameSignal 
{
    public class OnGridColorChanged { }
    public class OnGridChanged
    {
        public GridCell Grid;
        public OnGridChanged(GridCell newGrid) => Grid = newGrid;
    }
}
