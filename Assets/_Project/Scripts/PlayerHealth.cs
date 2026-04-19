using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerHealth : MonoBehaviour
{
    private SignalBus _signalBus;

    [SerializeField] private int _maximumHealth;

    private int _currentHealth;

    [Inject]
    public void Construct(SignalBus signalBus) => _signalBus = signalBus;
    void Start()
    {
        _currentHealth = _maximumHealth;
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
            Debug.Log("Player is dead");
    }
}
