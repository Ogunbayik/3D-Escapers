using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        //Player Scripts
        Container.Bind<PlayerHealth>().FromComponentOnRoot().AsSingle();
        Container.Bind<PlayerBase>().FromComponentOnRoot().AsSingle();
        Container.Bind<PlayerVisual>().FromComponentOnRoot().AsSingle();
        Container.Bind<PlayerHUD>().FromComponentInHierarchy().AsSingle();

        //PlayerRequirements
        Container.Bind<CharacterController>().FromComponentOnRoot().AsSingle();
        Container.Bind<Animator>().FromComponentInHierarchy().AsSingle();
        Container.Bind<SkinnedMeshRenderer>().FromComponentsInHierarchy().AsSingle();

        //States
        Container.Bind<IState>().To<PlayerIdleState>().AsSingle();
        Container.Bind<IState>().To<PlayerMovementState>().AsSingle();
        Container.Bind<IState>().To<PlayerAirborneState>().AsSingle();
        Container.Bind<IState>().To<PlayerDeathState>().AsSingle();
        Container.Bind<IState>().To<PlayerMenuIdleState>().AsSingle();

        Container.Bind<AnimationController>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerStateMachine>().AsSingle().NonLazy();
    }
}