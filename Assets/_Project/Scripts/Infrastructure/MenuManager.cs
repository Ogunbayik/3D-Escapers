using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [Header("Text References")]
    [SerializeField] private TextMeshProUGUI _levelDataText;
    [Header("Duration Time")]
    [SerializeField] private float _displayDuration;
    [SerializeField] private float _heartFullDuration;

    private LevelData _levelData;

    [Inject]
    public void Construct(LevelManager levelManager, SignalBus signalBus)
    {
        _levelManager = levelManager;
        _signalBus = signalBus;
    }
    private void Start()
    {
        _playButton.onClick.AddListener(OnLevelPreperation);
        Initialize();
    }
    private void Initialize()
    {
        var level = _levelManager.LevelIndex + 1;
        _levelDataText.text = level.ToString();

        ToggleMenuCanvas(true);
        ToggleGameCanvas(false);
    }
    private void OnEnable() => _signalBus.Subscribe<GameSignal.OnLevelDataChanged>(UpdateLevelData);
    private void OnDisable() => _signalBus.Unsubscribe<GameSignal.OnLevelDataChanged>(UpdateLevelData);
    private void OnLevelPreperation() => _signalBus.Fire(new GameSignal.OnLevelInitializing(_levelManager.ActiveLevelData));
    public void ToggleMenuCanvas(bool isActive) => _menuCanvas.gameObject.SetActive(isActive);
    public void ToggleGameCanvas(bool isActive) => _gameCanvas.gameObject.SetActive(isActive);
    public void TogglePlayerCanvas(bool isActive) => _playerCanvas.gameObject.SetActive(isActive);
    public void UpdateLevelData(GameSignal.OnLevelDataChanged signal) => _levelData = signal.LevelData;
    public void UpdateLevelText() => _levelDataText.text = _levelData.ID.ToString();
}
