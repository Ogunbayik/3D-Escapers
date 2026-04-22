using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    protected PlayerIdleState(PlayerBase player) : base(player) { }
    public override void EnterState()
    {
        base.EnterState();
    }
    public override void Tick()
    {
        base.Tick();

        if (Player.IsMoving())
            StateMachine.SwitchState<PlayerMovementState>();
    }
}
