using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    public void UpdateScoreText(GameSignal.OnGameScoreChanged signal) => _scoreText.text = $"Score: {signal.CurrentScore}";
    public void ResetScore() => _scoreText.text = $"Score: {0}";
}
