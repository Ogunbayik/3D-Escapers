using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zenject;

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

    public bool IsDead => _lifeStatus == LifeStatus.Dead;
    public bool IsInvulnerable => _lifeStatus == LifeStatus.Invulnerable;

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
        _lifeStatus = LifeStatus.Alive;
    }
    private void OnEnable()
    {
        _signalBus.Subscribe<GameSignal.OnPlayerGridStatus>(OnPlayerGridStatusChanged);
    }
    private void OnDisable()
    {
        _signalBus.Unsubscribe<GameSignal.OnPlayerGridStatus>(OnPlayerGridStatusChanged);
    }
    public void DecreaseHealth()
    {
        if (IsInvulnerable || IsDead) return;

        _currentHealth--;

        UpdateHealthState();
        NotifyHealthChanged();

        if (_currentHealth <= 0)
        {
            SetLifeStatus(LifeStatus.Dead);
            _stateMachine.OnPlayerHealthDepleted();
            _signalBus.Fire(new GameSignal.OnPlayerDead());
        }
        else
        {
            UpdateInvulnerableStatus().Forget();
        }
    }
    public void SetLifeStatus(LifeStatus newStatus)
    {
        if (_lifeStatus == newStatus) return;

        _lifeStatus = newStatus;
    }
    private void OnPlayerGridStatusChanged(GameSignal.OnPlayerGridStatus signal)
    {
        Debug.Log($"DecreaseHealth Tetiklendi! Invulnerable: {IsInvulnerable}, GridStatus: {signal.GridStatus}");

        if (signal.GridStatus == GridStatus.Lethal)
            DecreaseHealth();
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
        SetLifeStatus(LifeStatus.Invulnerable);
        var token = this.GetCancellationTokenOnDestroy();

        await UniTask.Delay(TimeSpan.FromSeconds(_data.InvulnerableDuration), cancellationToken: token);

        if (_lifeStatus != LifeStatus.Dead)
            SetLifeStatus(LifeStatus.Alive);
    }
    private void NotifyHealthChanged()
    {
        var percentage = (float)_currentHealth / _data.MaximumHealth;
        OnHealthChanged?.Invoke(percentage, _healthState);
    }
}
