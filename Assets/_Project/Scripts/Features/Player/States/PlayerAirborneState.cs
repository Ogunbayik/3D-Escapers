using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerAirborneState : PlayerBaseState
{
    private PlayerHealth _health;

    private SignalBus _signalBus;
    public PlayerAirborneState(PlayerBase player, AnimationController animationController, PlayerHealth health, SignalBus signalBus) : base(player, animationController)
    {
        _health = health;
        _signalBus = signalBus;
    }
    public override void EnterState()
    {
        base.EnterState();
        _health.SetInvulnerableStatus(true);

        Player.HandleJump();

        AnimationController.PlayAnimation(GameConst.PlayerAnimation.JUMP_HASH, GameConst.AnimationTransition.QUICK_TRANSITION);
    }
    public override void ExitState()
    {
        base.ExitState();
    }
    public override void Tick()
    {
        var direction = Player.GetMovementDirection();

        Player.ApplyGravity();
        Player.Move(direction);

        if (Player.IsGrounded() && Player.VelocityY < 0)
        {
            StateMachine.SwitchState<PlayerIdleState>();
            _health.SetInvulnerableStatus(false);
            _signalBus.Fire(new GameSignal.OnPlayerLanded());
        }
    }
}
