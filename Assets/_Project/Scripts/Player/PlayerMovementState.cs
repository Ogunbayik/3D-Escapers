using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementState : PlayerGroundedState
{
    protected PlayerMovementState(PlayerBase player) : base(player) { }

    public override void EnterState()
    {
        base.EnterState();
    }
    public override void ExitState()
    {
        base.ExitState();
    }
    public override void Tick()
    {
        base.Tick();

        if (!Player.IsMoving())
            StateMachine.SwitchState<PlayerIdleState>();

        var direction = Player.GetMovementDirection();
        Player.Move(direction);
    }
}
