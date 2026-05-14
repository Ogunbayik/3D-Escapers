using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Zenject;
using System;

public class FlowManager
{
    private MenuManager _menuManager;
    private CameraManager _cameraManager;
    private BoardManager _boardManager;
    private LevelManager _levelManager;
    private ScoreManager _scoreManager;
    private SignalBus _signalBus;

    public FlowManager(MenuManager menuManager, CameraManager cameraManager, BoardManager boardManager, LevelManager levelManager, ScoreManager scoreManager, SignalBus signalBus)
    {
        _menuManager = menuManager;
        _cameraManager = cameraManager;
        _boardManager = boardManager;
        _levelManager = levelManager;
        _scoreManager = scoreManager;
        _signalBus = signalBus;
    }
    public void OnLevelInitializing() => LevelStartSequence().Forget();
    public void OnLevelCompleting() => LevelCompleteSequence().Forget();
    public async UniTask LevelStartSequence()
    {
        _menuManager.ToggleMenuCanvas(false);
        _scoreManager.SetReachScore(_levelManager.ActiveLevelData.ReachScore);
        _signalBus.Fire(new GameSignal.OnPlayerTeleportRequested(_boardManager.Data.GameStartPosition));

        await UniTask.Delay(TimeSpan.FromSeconds(GameConst.Durations.INITIAL_DELAY));

        _cameraManager.SwitchCamera(CameraType.Transition);
        _cameraManager.OnTransitionStart();

        await UniTask.Delay(TimeSpan.FromSeconds(GameConst.Durations.CAMERA_TRANSITION_TIME));

        _boardManager.SetupBoard(_levelManager.ActiveLevelData).Forget();

        await UniTask.Delay(TimeSpan.FromSeconds(GameConst.Durations.BOARD_SETUP_DELAY));

        _menuManager.DisplayHealthBar();
        _cameraManager.SwitchCamera(CameraType.Game);

        await UniTask.Delay(TimeSpan.FromSeconds(GameConst.Durations.DISPLAY_HEALTH_DELAY));

        _menuManager.AnimateHealthRefill();
        _menuManager.ToggleGameCanvas(true);

        await UniTask.Delay(TimeSpan.FromSeconds(GameConst.Durations.GAMEPLAY_START_DELAY));

        _boardManager.InitialGoalGrid();
        _boardManager.StartLethalSequence();

        _signalBus.Fire(new GameSignal.OnLevelStarted());
    }

    public async UniTask LevelCompleteSequence()
    {
        _cameraManager.SwitchCamera(CameraType.Victory);
    }
}
