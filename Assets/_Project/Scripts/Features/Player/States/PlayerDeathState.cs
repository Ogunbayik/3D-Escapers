using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerGroundedState
{
    protected PlayerDeathState(PlayerBase player, AnimationController animationController) : base(player, animationController) { }
    public override void EnterState()
    {
        base.EnterState();

        Debug.Log("Player is Dead");
        AnimationController.PlayAnimation(GameConst.PlayerAnimation.DEATH_HASH, GameConst.AnimationDuration.QUICK_TRANSITION);
    }
    public override void ExitState()
    {

    }
    public override void Tick()
    {

    }
}
