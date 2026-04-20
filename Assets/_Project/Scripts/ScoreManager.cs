using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ScoreManager
{
    private SignalBus _signalBus;

    private int _currentScore;
    private int _reachScore = 50;

    public ScoreManager(SignalBus signalBus) => _signalBus = signalBus;
    public void IncreaseScore(int score)
    {
        _currentScore += score;
        _signalBus.Fire(new GameSignal.OnGameScoreChanged(_currentScore));

        if (IsReachScore())
        {
            Debug.Log("Player pass the game..");
            _signalBus.Fire(new GameSignal.OnGameLevelPassed());
        }
    }
    public bool IsReachScore() => _currentScore >= _reachScore;
}
