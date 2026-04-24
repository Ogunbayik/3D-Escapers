using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : IState
{
    protected PlayerBase Player;

    protected AnimationController AnimationController;

    protected PlayerStateMachine StateMachine;
    public PlayerBaseState(PlayerBase player, AnimationController animationController)
    {
        Player = player;
        AnimationController = animationController;
    }
    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public abstract void Tick();
    public void SetStateMachine(PlayerStateMachine stateMachine) => StateMachine = stateMachine;

}
