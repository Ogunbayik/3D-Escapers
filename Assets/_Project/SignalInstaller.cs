using UnityEngine;
using Zenject;

public class SignalInstaller : MonoInstaller
{
    [SerializeField] private BoardManager _boardManager;
    public override void InstallBindings()
    {
        Container.BindInstance(_boardManager).AsSingle();

        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<GameSignal.OnGridColorChanged>();

        Container.BindSignal<GameSignal.OnGridColorChanged>()
            .ToMethod<BoardManager>(x => x.Test)
            .FromResolve();
    }
}