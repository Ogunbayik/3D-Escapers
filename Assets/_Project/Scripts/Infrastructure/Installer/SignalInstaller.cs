using UnityEngine;
using Zenject;

public class SignalInstaller : MonoInstaller
{
    
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);

        //Check Signals
        Container.DeclareSignal<GameSignal.OnGridChanged>();
        Container.DeclareSignal<GameSignal.OnGridColorChanged>();
        Container.DeclareSignal<GameSignal.OnPlayerLanded>();
        Container.DeclareSignal<GameSignal.OnPlayerGridStatus>();

        //Health Changed Signals
        Container.DeclareSignal<GameSignal.OnPlayerDead>();

        //Level Signals
        Container.DeclareSignal<GameSignal.OnLevelCompleted>();
        Container.DeclareSignal<GameSignal.OnLevelInitializing>();
        Container.DeclareSignal<GameSignal.OnLevelStarted>();
        Container.DeclareSignal<GameSignal.OnLevelDataChanged>();
        Container.DeclareSignal<GameSignal.OnLevelScoreReached>();

        //Score Signals
        Container.DeclareSignal<GameSignal.OnGameScoreChanged>();

        //CollectibleItem
        Container.DeclareSignal<GameSignal.OnCollectableItemCollected>();
        
    }
}