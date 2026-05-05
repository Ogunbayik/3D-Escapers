using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MenuManager : MonoBehaviour
{
    private LevelManager _levelManager;
    private SignalBus _signalBus;

    [Header("Canvas References")]
    [SerializeField] private Canvas _menuCanvas;
    [SerializeField] private Canvas _gameCanvas;
    [SerializeField] private Canvas _playerCanvas;
    [Header("Button References")]
    [SerializeField] private Button _playButton;

    [Inject]
    public void Construct(LevelManager levelManager, SignalBus signalBus)
    {
        _levelManager = levelManager;
        _signalBus = signalBus;
    }
    private void Start() => _playButton.onClick.AddListener(OnLevelStarted);
    private void OnLevelStarted() => _signalBus.Fire(new GameSignal.OnLevelStarted(_levelManager.CurrentLevelData));
}
