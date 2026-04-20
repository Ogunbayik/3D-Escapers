using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    public void UpdateScoreText(GameSignal.OnGameScoreChanged signal) => _scoreText.text = $"Score: {signal.CurrentScore}";
}
