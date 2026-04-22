using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        //Player Scripts
        Container.Bind<PlayerHealth>().FromComponentOnRoot().AsSingle();
        Container.Bind<PlayerBase>().FromComponentOnRoot().AsSingle();

        //PlayerRequirements
        Container.Bind<CharacterController>().FromComponentOnRoot().AsSingle();

        //States
        Container.Bind<IState>().To<PlayerIdleState>().AsSingle();
        Container.Bind<IState>().To<PlayerMovementState>().AsSingle();

        Container.Bind<PlayerStateMachine>().AsSingle().NonLazy();
    }
}