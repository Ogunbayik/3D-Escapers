using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerHUD : MonoBehaviour
{
    private PlayerHealth _health;

    [Header("Image References")]
    [SerializeField] private Image _healtFill;

    [SerializeField] private float _decreaseDuration;

    [Inject]
    public void Construct(PlayerHealth health) => _health = health;
    private void OnEnable() => _health.OnHealthChanged += Health_OnHealthChanged;
    private void OnDisable() => _health.OnHealthChanged -= Health_OnHealthChanged;
    private void Health_OnHealthChanged(int currentHealth, int maximumHealth)
    {
        var percentage = (float)currentHealth / maximumHealth;
        UpdateFillAmount(percentage);
    }
    public void InitializeHUD(float percentage) => UpdateFillAmount(percentage);
    private void UpdateFillAmount(float percentage) => _healtFill.DOFillAmount(percentage, _decreaseDuration);
}
