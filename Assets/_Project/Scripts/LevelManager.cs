using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelManager : MonoBehaviour
{
    private SignalBus _signalBus;

    [SerializeField] private List<LevelData> _allLevels = new List<LevelData>();

    [Header("Test Parts")]
    [SerializeField] private LevelData _currentLevelData;

    [SerializeField] private Button _startButton;

    [Inject]
    public void Construct(SignalBus signalBus) => _signalBus = signalBus;

    private void Start() => _startButton.onClick.AddListener(StartLevel);
    private void StartLevel() => _signalBus.Fire(new GameSignal.OnLevelStarted(_currentLevelData));
    public void SetLevelData(LevelData newLevelData)
    {
        if (newLevelData == _currentLevelData)
            return;

        _currentLevelData = newLevelData;
    }
}
