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

    public LevelData CurrentLevelData => _currentLevelData;

    [Inject]
    public void Construct(SignalBus signalBus) => _signalBus = signalBus;
    public void SetLevelData(LevelData newLevelData)
    {
        if (newLevelData == _currentLevelData)
            return;

        _currentLevelData = newLevelData;
    }
}
