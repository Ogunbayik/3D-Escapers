using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirborneState : PlayerBaseState
{
    public PlayerAirborneState(PlayerBase player) : base(player) { }
    public override void EnterState()
    {
        base.EnterState();

        Player.HandleJump();
        Player.SetGrid(null);
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
            Debug.Log("Player is Idle");
        }
    }
}
