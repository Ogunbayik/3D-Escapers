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
        Container.Bind<Animator>().FromComponentInHierarchy().AsSingle();

        //States
        Container.Bind<IState>().To<PlayerIdleState>().AsSingle();
        Container.Bind<IState>().To<PlayerMovementState>().AsSingle();
        Container.Bind<IState>().To<PlayerAirborneState>().AsSingle();
        Container.Bind<IState>().To<PlayerDeathState>().AsSingle();

        Container.Bind<AnimationController>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerStateMachine>().AsSingle().NonLazy();
    }
}