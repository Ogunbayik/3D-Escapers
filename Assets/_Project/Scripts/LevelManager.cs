using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelManager : MonoBehaviour
{
    private SignalBus _signalBus;

    private ISaveService _saveService;

    [SerializeField] private List<LevelData> _allLevels = new List<LevelData>();

    private LevelData _activeLevelData;
    public LevelData ActiveLevelData => _activeLevelData;

    private int _levelIndex;

    [Inject]
    public void Construct(SignalBus signalBus, ISaveService saveService)
    {
        _signalBus = signalBus;
        _saveService = saveService;
    }
    private void Start() => Initialize();
    private void Initialize()
    {
        _levelIndex = _saveService.GetSavedLevelIndex();
        _activeLevelData = _allLevels[_levelIndex];
    }
    public void OnLevelComplete()
    {
        _levelIndex++;

        _saveService.SaveLevelIndex(_levelIndex);
        SetLevelData(_allLevels[_levelIndex]);
    }
    public void SetLevelData(LevelData newLevelData)
    {
        if (newLevelData == _activeLevelData)
            return;

        _activeLevelData = newLevelData;
    }
}
