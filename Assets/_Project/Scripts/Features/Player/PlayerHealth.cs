using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;

public class PlayerHealth : MonoBehaviour
{
    private PlayerHUD _hud;

    public event Action<float, HealthState> OnHealthChanged;

    private SignalBus _signalBus;

    [Header("Data References")]
    [SerializeField] private PlayerData _data;

    private HealthState _healthState;
    private PlayerStateMachine _stateMachine;

    private int _currentHealth;

    private float _invulnerableTimer;

    private bool _isInvulnerable;

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
        {
            DecreaseHealth();
            SetInvulnerableStatus(true);
        }
    }
    public void DecreaseHealth()
    {
        if (_isInvulnerable) return;

        _currentHealth--;

        UpdateInvulnerableStatus().Forget();
        UpdateHealthState();

        var percentage = (float)_currentHealth / _data.MaximumHealth;
        OnHealthChanged?.Invoke(percentage, _healthState);

        if (_currentHealth <= 0)
        {
            _stateMachine.OnPlayerHealthDepleted();

            _signalBus.Fire(new GameSignal.OnPlayerDead());
        }
    }
    private void UpdateHealthState()
    {
        float healthPercentage = (float)_currentHealth / _data.MaximumHealth;

        if (healthPercentage < GameConst.HealthPercentage.CRITICAL_PERCENTAGE)
            _healthState = HealthState.Critical;
        else if (healthPercentage < GameConst.HealthPercentage.UNSTABLE_PERCENTAGE)
            _healthState = HealthState.Unstable;
        else if (healthPercentage < GameConst.HealthPercentage.STABLE_PERCENTAGE)
            _healthState = HealthState.Stable;
        else
            _healthState = HealthState.Optimal;
    }
    private async UniTask UpdateInvulnerableStatus()
    {
        SetInvulnerableStatus(true);

        await UniTask.Delay(TimeSpan.FromSeconds(_data.InvulnerableDuration));

        SetInvulnerableStatus(false);
    }
    private void SetInvulnerableStatus(bool status) => _isInvulnerable = status;
}
