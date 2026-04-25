using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerHealth : MonoBehaviour
{
    private SignalBus _signalBus;

    [Header("Data References")]
    [SerializeField] private PlayerData _data;

    private PlayerStateMachine _stateMachine;

    private int _currentHealth;

    [Inject]
    public void Construct(SignalBus signalBus, PlayerStateMachine stateMachine)
    { 
        _signalBus = signalBus;
        _stateMachine = stateMachine;
    }
    void Start()
    {
        _currentHealth = _data.MaximumHealth;
    }
    private void OnEnable() => _signalBus.Subscribe<GameSignal.OnPlayerGridStatus>(OnPlayerGridStatusChanged);
    private void OnDisable() => _signalBus.Unsubscribe<GameSignal.OnPlayerGridStatus>(OnPlayerGridStatusChanged);
    private void OnPlayerGridStatusChanged(GameSignal.OnPlayerGridStatus signal)
    {
        if (signal.GridStatus == GridStatus.Lethal)
            DecreaseHealth();
    }
    public void DecreaseHealth()
    {
        _currentHealth--;

        Debug.Log($"Player Health: {_currentHealth}");

        if (_currentHealth <= 0)
        {
            _stateMachine.OnPlayerHealthDepleted();

        }
    }
}
