using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerFallState : PlayerAirborneState
{
    private PlayerHealth _health;
    private SignalBus _signalBus;
    public PlayerFallState(PlayerBase player, AnimationController animationController, PlayerHealth health, SignalBus signalBus) : base(player, animationController, health)
    {
        _health = health;
        _signalBus = signalBus;
    }

    public override void EnterState()
    {
        AnimationController.PlayAnimation(GameConst.PlayerAnimation.FALL_HASH, GameConst.AnimationTransition.QUICK_TRANSITION);
    }
    public override void Tick()
    {
        base.Tick();

        if (Player.IsGrounded())
        {
            Player.CheckGrid();

            StateMachine.SwitchState<PlayerIdleState>();
            _signalBus.Fire(new GameSignal.OnPlayerLanded());
            _health.SetLifeStatus(LifeStatus.Alive);
        }
    }
    public override void ExitState()
    {
      
    }
}
