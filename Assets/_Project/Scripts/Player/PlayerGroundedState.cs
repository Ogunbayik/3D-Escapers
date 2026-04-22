using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    protected PlayerGroundedState(PlayerBase player) : base(player) { }
    public override void EnterState() => base.EnterState();
    public override void ExitState() => base.ExitState();
    public override void Tick()
    {
        if (Player.IsGrounded() && Player.PressedJump())
            StateMachine.SwitchState<PlayerAirborneState>();

        Player.CheckGrid();
    }
}
