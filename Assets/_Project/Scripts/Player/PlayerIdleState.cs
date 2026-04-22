using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    protected PlayerIdleState(PlayerBase player) : base(player) { }
    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Idle State");
    }
    public override void Tick()
    {
        base.Tick();
    }
}
