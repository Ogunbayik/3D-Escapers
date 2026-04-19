

public static class GameSignal 
{
    public class OnGridColorChanged { }
    public class OnGridChanged { }
    public class OnPlayerGridStatus
    {
        public GridStatus GridStatus;
        public OnPlayerGridStatus(GridStatus gridStatus) => GridStatus = gridStatus;
    }
}
