using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerHealth : MonoBehaviour
{
    private PlayerHUD _hud;

    public event Action<int, int> OnHealthChanged;

    private SignalBus _signalBus;

    [Header("Data References")]
    [SerializeField] private PlayerData _data;

    private PlayerStateMachine _stateMachine;

    private int _currentHealth;

    [Inject]
    public void Construct(SignalBus signalBus, PlayerStateMachine stateMachine, PlayerHUD hud)
    { 
        _signalBus = signalBus;
        _stateMachine = stateMachine;
        _hud = hud;
    }
    void Start() => Initialize();
    private void Initialize()
    {
        _currentHealth = _data.MaximumHealth;
        var percentage = (float)_currentHealth / _data.MaximumHealth;

        _hud.InitializeHUD(percentage);
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

        OnHealthChanged?.Invoke(_currentHealth, _data.MaximumHealth);
        Debug.Log($"Player Health: {_currentHealth}");

        if (_currentHealth <= 0)
        {
            _stateMachine.OnPlayerHealthDepleted();

            _signalBus.Fire(new GameSignal.OnPlayerDead());
        }
    }
}
