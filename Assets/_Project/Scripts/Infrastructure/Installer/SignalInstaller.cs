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
        Container.DeclareSignal<GameSignal.OnGridColorChanged>();
        Container.DeclareSignal<GameSignal.OnGridChanged>();
        Container.DeclareSignal<GameSignal.OnPlayerGridStatus>();
        Container.DeclareSignal<GameSignal.OnGameScoreChanged>();
        Container.DeclareSignal<GameSignal.OnGameLevelPassed>();
        Container.DeclareSignal<GameSignal.OnPlayerDead>();

        Container.BindSignal<GameSignal.OnGameScoreChanged>()
            .ToMethod<ScoreUIController>(x => x.UpdateScoreText)
            .FromResolve();
    }
}