using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerAirborneState : PlayerBaseState
{
    private PlayerHealth _health;
    public PlayerAirborneState(PlayerBase player, AnimationController animationController, PlayerHealth health) : base(player, animationController)
    {
        _health = health;
    }
    public override void EnterState()
    {
        base.EnterState();
        _health.SetLifeStatus(LifeStatus.Invulnerable);

        Player.HandleJump();
        Player.SetGrid(null);

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

        if (Player.VelocityY < 0f)
            StateMachine.SwitchState<PlayerFallState>();
    }
}
