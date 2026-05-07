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
    [Header("Duration Time")]
    [SerializeField] private float _displayDuration;
    [SerializeField] private float _heartFullDuration;

    private PlayerHUD _playerHUD;

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
        _playerHUD = _playerCanvas.GetComponentInChildren<PlayerHUD>();

        ToggleMenuCanvas(true);
        ToggleGameCanvas(false);
    }
    private void OnLevelPreperation() => _signalBus.Fire(new GameSignal.OnLevelInitializing(_levelManager.CurrentLevelData));
    public void ToggleMenuCanvas(bool isActive) => _menuCanvas.gameObject.SetActive(isActive);
    public void ToggleGameCanvas(bool isActive) => _gameCanvas.gameObject.SetActive(isActive);
    public void TogglePlayerCanvas(bool isActive) => _playerCanvas.gameObject.SetActive(isActive);
    public void FillHealtBarEffect() => _playerHUD.FillHealthBarEffect(_heartFullDuration);
    public void DisplayHealthBar() => _playerHUD.DisplayHealthBar(_displayDuration);
}
