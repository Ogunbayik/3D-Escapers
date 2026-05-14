using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerVictoryState : PlayerBaseState
{
    private bool _hasVictorySequenceStarted = false;

    private SignalBus _signalBus;
    public PlayerVictoryState(PlayerBase player, AnimationController animationController,SignalBus signalBus) : base(player, animationController)  => _signalBus = signalBus;

    public override void EnterState()
    {
        AnimationController.PlayAnimation(GameConst.PlayerAnimation.IDLE_HASH, GameConst.AnimationTransition.QUICK_TRANSITION);
    }
    public override void Tick()
    {
        Player.ApplyGravity();

        Player.ResetMovement();

        if (Player.IsGrounded() && !_hasVictorySequenceStarted)
            VictorySequence().Forget();
    }
    public override void ExitState()
    {
        
    }
    public async UniTask VictorySequence()
    {
        if (_hasVictorySequenceStarted) return;

        SetVictorySequenceStatus(true);

        Player.AlignToVictoryPose();

        //3f Camera TransitionTime
        await UniTask.Delay(System.TimeSpan.FromSeconds(3f));

        //TODO Victory Animation deðiþtirilecek
        AnimationController.PlayAnimation(GameConst.PlayerAnimation.VICTORY_HASH, GameConst.AnimationTransition.QUICK_TRANSITION);

        await UniTask.WaitUntil(() => AnimationController.IsAnimationFinished(GameConst.PlayerAnimation.VICTORY_HASH));

        AnimationController.PlayAnimation(GameConst.PlayerAnimation.IDLE_HASH, GameConst.AnimationTransition.QUICK_TRANSITION);

        //TODO 1F delay for dissolve animation
        await UniTask.Delay(TimeSpan.FromSeconds(1f));

    }
    private void SetVictorySequenceStatus(bool isActive) => _hasVictorySequenceStarted = isActive;

    
}
