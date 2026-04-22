using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerBaseState
{
    protected PlayerGroundState(PlayerBase player) : base(player) { }
    public override void EnterState() => base.EnterState();
    public override void ExitState() => base.ExitState();
    public override void Tick()
    {
        //TODO Player can pressed jump and change moveState
    }
}
