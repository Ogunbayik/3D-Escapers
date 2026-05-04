using UnityEngine;
using Zenject;

public class SignalInstaller : MonoInstaller
{
    [SerializeField] private BoardManager _boardManager;
    [SerializeField] private ScoreUIController _scoreUIController;
    public override void InstallBindings()
    {
        Container.BindInstance(_boardManager).AsSingle();
        Container.BindInstance(_scoreUIController).AsSingle();
        Container.Bind<ScoreManager>().AsSingle();

        SignalBusInstaller.Install(Container);
        //Check Signals
        Container.DeclareSignal<GameSignal.OnGridChanged>();
        Container.DeclareSignal<GameSignal.OnGridColorChanged>();
        Container.DeclareSignal<GameSignal.OnPlayerLanded>();
        Container.DeclareSignal<GameSignal.OnPlayerGridStatus>();
        //Health Changed Signals
        Container.DeclareSignal<GameSignal.OnPlayerDead>();

        Container.BindSignal<GameSignal.OnPlayerDead>()
            .ToMethod<CameraManager>(x => x.OnPlayerDead)
            .FromResolve();

        //Level Signals
        Container.DeclareSignal<GameSignal.OnGameLevelPassed>();
        Container.DeclareSignal<GameSignal.OnLevelStarted>();

        //Score Signals
        Container.DeclareSignal<GameSignal.OnGameScoreChanged>();
        Container.BindSignal<GameSignal.OnGameScoreChanged>()
            .ToMethod<ScoreUIController>(x => x.UpdateScoreText)
            .FromResolve();
    }
}