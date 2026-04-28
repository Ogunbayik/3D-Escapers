using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [Header("Manager References")]
    [SerializeField] private CameraManager _cameraManager;
    public override void InstallBindings()
    {
        Container.Bind<IInputService>().To<InputService>().AsSingle();

        Container.BindInstance(_cameraManager).AsSingle();
    }
}