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
    private PlayerFacade _player;

    public FlowManager(MenuManager menuManager,
        CameraManager cameraManager,
        BoardManager boardManager,
        LevelManager levelManager,
        ScoreManager scoreManager,
        SignalBus signalBus,
        PlayerFacade player)
    {
        _menuManager = menuManager;
        _cameraManager = cameraManager;
        _boardManager = boardManager;
        _levelManager = levelManager;
        _scoreManager = scoreManager;
        _signalBus = signalBus;
        _player = player;
    }
    public void OnLevelInitializing() => LevelStartSequence().Forget();
    public void OnLevelCompleting() => LevelCompleteSequence().Forget();
    public async UniTask LevelStartSequence()
    {
        _menuManager.ToggleMenuCanvas(false);
        _scoreManager.SetReachScore(_levelManager.ActiveLevelData.ReachScore);

        await _player.Visual.Disappear(2f);

        _cameraManager.SwitchCamera(CameraType.Transition);

        await _cameraManager.PlayPathTransition(true);

        _player.Hud.DisplayHealthBar(2f);
        _player.Base.SetPosition(_boardManager.Data.GameStartPosition);
        _cameraManager.SwitchCamera(CameraType.Game);

        await _player.Visual.Appear(2f);

        _player.Hud.FillHealthBarEffect(2f);
        _menuManager.ToggleGameCanvas(true);

        await _boardManager.SetupBoard(_levelManager.ActiveLevelData);

        _boardManager.InitialGoalGrid();
        _boardManager.StartLethalSequence();

        _signalBus.Fire(new GameSignal.OnLevelStarted());
        //Camera Game Camera oldu -> Player Appear -> Board kurulacak -> Can görünecek -> Can fullenecek ve GameCanvas açýlacak -> Board için goal grid ve lethal
    }

    public async UniTask LevelCompleteSequence()
    {
        _cameraManager.SwitchCamera(CameraType.Victory);

        _menuManager.TogglePlayerCanvas(false);
        _menuManager.ToggleGameCanvas(false);

        _boardManager.StopLethalSequence();

        await UniTask.Delay(TimeSpan.FromSeconds(1f));

        _levelManager.OnLevelComplete();

        await _player.Visual.Disappear(3f);

        _cameraManager.SwitchCamera(CameraType.Transition);

        await _cameraManager.PlayPathTransition(false);

        _player.Base.SetPosition(_boardManager.Data.MenuPosition);
        _cameraManager.SwitchCamera(CameraType.Menu);

        _boardManager.ReturnAllCells();

        await _player.Visual.Appear(3f);

        _menuManager.ToggleMenuCanvas(true);
        _menuManager.UpdateLevelText();
    }
}
