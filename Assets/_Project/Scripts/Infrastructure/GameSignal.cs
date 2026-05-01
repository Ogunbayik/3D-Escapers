

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

    //Level Signals
    public class OnGameLevelPassed { }
    public class OnLevelStarted
    {
        public LevelData CurrentLevel;
        public OnLevelStarted(LevelData levelData) => CurrentLevel = levelData;
    }

    //Player Signals
    public class OnPlayerDead { }
}
