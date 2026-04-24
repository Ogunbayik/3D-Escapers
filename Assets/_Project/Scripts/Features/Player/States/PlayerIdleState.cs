using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    protected PlayerIdleState(PlayerBase player, AnimationController animationController) : base(player, animationController) { }
    public override void EnterState()
    {
        base.EnterState();

        AnimationController.PlayAnimation(GameConst.PlayerAnimation.IDLE_HASH, GameConst.AnimationDuration.QUICK_TRANSITION);
    }
    public override void Tick()
    {
        base.Tick();

        if (Player.IsMoving())
            StateMachine.SwitchState<PlayerMovementState>();
    }
}
