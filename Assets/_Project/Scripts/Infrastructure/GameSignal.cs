

public static class GameSignal 
{
    public class OnGridColorChanged { }
    public class OnGridChanged { }
    public class OnPlayerGridStatus
    {
        public GridStatus GridStatus;
        public OnPlayerGridStatus(GridStatus gridStatus) => GridStatus = gridStatus;
    }
    public class OnGameScoreChanged
    {
        public int CurrentScore;
        public OnGameScoreChanged(int currentScore) => CurrentScore = currentScore;
    }
    public class OnGameLevelPassed { }


    //Player Signals
    public class OnPlayerDead { }
}
