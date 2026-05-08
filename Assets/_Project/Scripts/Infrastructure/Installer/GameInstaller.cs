using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [Header("Manager References")]
    [SerializeField] private BoardManager _boardManager;
    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private MenuManager _menuManager;
    [SerializeField] private ScoreUIController _scoreUIController;
    public override void InstallBindings()
    {
        Container.Bind<IInputService>().To<InputService>().AsSingle();
        Container.Bind<ISaveService>().To<SaveService>().AsSingle();

        Container.Bind<FlowManager>().AsSingle();
        Container.Bind<ScoreManager>().AsSingle();

        Container.BindInstance(_cameraManager).AsSingle();
        Container.BindInstance(_levelManager).AsSingle();
        Container.BindInstance(_menuManager).AsSingle();
        Container.BindInstance(_boardManager).AsSingle();

        Container.BindInstance(_scoreUIController).AsSingle();

        //Camera Methods
        Container.BindSignal<GameSignal.OnPlayerDead>()
          .ToMethod<CameraManager>(x => x.OnPlayerDead)
          .FromResolve();

        //Flow Methods
        Container.BindSignal<GameSignal.OnLevelInitializing>()
          .ToMethod<FlowManager>(x => x.OnLevelInitializing)
          .FromResolve();

        Container.BindSignal<GameSignal.OnGameScoreChanged>()
            .ToMethod<ScoreUIController>(x => x.UpdateScoreText)
            .FromResolve();

       
    }
}