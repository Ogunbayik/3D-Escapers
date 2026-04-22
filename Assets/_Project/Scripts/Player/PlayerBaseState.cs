using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : IState
{
    protected PlayerBase Player;

    protected PlayerStateMachine StateMachine;
    public PlayerBaseState(PlayerBase player) => Player = player;
    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public abstract void Tick();
    public void SetStateMachine(PlayerStateMachine stateMachine) => StateMachine = stateMachine;

}
