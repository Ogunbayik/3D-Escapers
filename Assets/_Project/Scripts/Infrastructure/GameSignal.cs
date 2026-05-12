

public static class GameSignal 
{
    //Color Check Signals
    public class OnGridColorChanged { }
    public class OnGridChanged { }
    public class OnPlayerLanded { }

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
    public class OnLevelCompleted { }
    public class OnLevelScoreReached { }
    public class OnLevelDataChanged
    {
        public LevelData LevelData;
        public int LevelIndex;

        public OnLevelDataChanged(LevelData levelData, int levelIndex)
        {
            LevelData = levelData;
            LevelIndex = levelIndex;
        }
    }
    public class OnLevelInitializing
    {
        public LevelData CurrentLevel;
        public OnLevelInitializing(LevelData levelData) => CurrentLevel = levelData;
    }
    public class OnLevelStarted { }
    //Player Signals
    public class OnPlayerDead { }
}
