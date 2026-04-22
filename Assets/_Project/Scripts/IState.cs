using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void EnterState();
    void ExitState();
    void Tick();
    void SetStateMachine(PlayerStateMachine playerStateMachine);
}
