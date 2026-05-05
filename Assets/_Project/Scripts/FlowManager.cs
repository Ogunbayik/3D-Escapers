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

    public FlowManager(MenuManager menuManager, CameraManager cameraManager, BoardManager boardManager, LevelManager levelManager)
    {
        _menuManager = menuManager;
        _cameraManager = cameraManager;
        _boardManager = boardManager;
        _levelManager = levelManager;
    }
    public void OnLevelStarted() => LevelStartSequence().Forget();
    public async UniTask LevelStartSequence()
    {
        //TODO Create animation for player Teleport
        _menuManager.ToggleMenuCanvas(false);

        await UniTask.Delay(TimeSpan.FromSeconds(GameConst.Durations.INITIAL_DELAY));

        _cameraManager.SwitchCamera(CameraType.Transition);
        _cameraManager.OnTransitionStart();

        await UniTask.Delay(TimeSpan.FromSeconds(GameConst.Durations.CAMERA_TRANSITION_TIME));

        _boardManager.SetupBoard(_levelManager.CurrentLevelData);
        _menuManager.DisplayHealthBar();

        await UniTask.Delay(TimeSpan.FromSeconds(GameConst.Durations.BOARD_SETUP_DELAY));

        _cameraManager.Player.transform.position = new Vector3(-2f, 0f, -2f);
        _cameraManager.SwitchCamera(CameraType.Game);

        _menuManager.FillHealtBarEffect();
        _menuManager.ToggleGameCanvas(true);
    }
}
