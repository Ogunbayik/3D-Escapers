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
    private LifeStatus _lifeStatus;
    private PlayerStateMachine _stateMachine;

    private int _currentHealth;

    private bool _isInvulnerable;

    public bool IsDead => _lifeStatus == LifeStatus.Dead;
    public bool IsInvulnerable => _isInvulnerable;

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
        _lifeStatus = LifeStatus.Alive;
    }
    private void OnEnable()
    {
        _signalBus.Subscribe<GameSignal.OnPlayerGridStatus>(OnPlayerGridStatusChanged);
        _signalBus.Subscribe<GameSignal.OnPlayerDead>(OnPlayerDead);
    }
    private void OnDisable()
    {
        _signalBus.Unsubscribe<GameSignal.OnPlayerGridStatus>(OnPlayerGridStatusChanged);
        _signalBus.Unsubscribe<GameSignal.OnPlayerDead>(OnPlayerDead);
    }
    public void DecreaseHealth()
    {
        if (_isInvulnerable) return;

        _currentHealth--;

        UpdateInvulnerableStatus().Forget();
        UpdateHealthState();
        NotifyHealthChanged();

        if (_currentHealth <= 0)
        {
            _stateMachine.OnPlayerHealthDepleted();
            _signalBus.Fire(new GameSignal.OnPlayerDead());
        }
    }
    public void SetLifeStatus(LifeStatus newStatus)
    {
        if (_lifeStatus == newStatus) return;

        _lifeStatus = newStatus;
    }
    public void SetInvulnerableStatus(bool status) => _isInvulnerable = status;
    private void OnPlayerGridStatusChanged(GameSignal.OnPlayerGridStatus signal)
    {
        if (signal.GridStatus == GridStatus.Lethal)
        {
            DecreaseHealth();
            SetInvulnerableStatus(true);
        }
    }
    private void OnPlayerDead() => SetLifeStatus(LifeStatus.Dead);
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
    private void NotifyHealthChanged()
    {
        var percentage = (float)_currentHealth / _data.MaximumHealth;
        OnHealthChanged?.Invoke(percentage, _healthState);
    }
}
