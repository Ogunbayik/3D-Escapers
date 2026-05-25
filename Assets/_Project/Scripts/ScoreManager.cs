using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ScoreManager
{
    private SignalBus _signalBus;

    private int _currentScore;
    private int _reachScore;

    public ScoreManager(SignalBus signalBus) => _signalBus = signalBus;
    public void AddScore(int score)
    {
        _currentScore += score;
        _signalBus.Fire(new GameSignal.OnGameScoreChanged(_currentScore));

        if (IsReachedScore())
        {
            Debug.Log("Player pass the game..");
            _signalBus.Fire(new GameSignal.OnLevelScoreReached());
        }
    }
    public void ResetScore() => _currentScore = 0;
    public void SetReachScore(int score) => _reachScore = score;
    public bool IsReachedScore() => _currentScore >= _reachScore;
}
