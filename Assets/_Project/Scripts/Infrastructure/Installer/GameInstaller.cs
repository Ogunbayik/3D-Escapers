using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [Header("Manager References")]
    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private MenuManager _menuManager;
    public override void InstallBindings()
    {
        Container.Bind<IInputService>().To<InputService>().AsSingle();

        Container.BindInstance(_cameraManager).AsSingle();
        Container.BindInstance(_levelManager).AsSingle();
        Container.BindInstance(_menuManager).AsSingle();
    }
}