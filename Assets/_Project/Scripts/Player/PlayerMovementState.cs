using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementState : PlayerGroundState
{
    protected PlayerMovementState(PlayerBase player) : base(player) { }

    public override void EnterState()
    {
        Debug.Log("Player is moving");
        base.EnterState();
    }
    public override void ExitState()
    {
        base.ExitState();
    }
    public override void Tick()
    {
        base.Tick();
    }
}
