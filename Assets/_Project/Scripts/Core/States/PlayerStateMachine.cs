using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class PlayerStateMachine : IInitializable, ITickable
{
    protected List<IState> _states;
    protected IState _currentState;
    public PlayerStateMachine(List<IState> states) => _states = states;
    public void Initialize()
    {
        foreach (var state in _states)
            state.SetStateMachine(this);

        SwitchState<PlayerMenuIdleState>();
    }
    public void Tick() => _currentState?.Tick();
    public void SwitchState<T>() where T : IState
    {
        _currentState?.ExitState();

        _currentState = _states.OfType<T>().FirstOrDefault();
        _currentState?.EnterState();
    }
    public void OnPlayerHealthDepleted() => SwitchState<PlayerDeathState>();
}
