using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerHUD : MonoBehaviour
{
    private PlayerHealth _health;

    [Header("Image References")]
    [SerializeField] private Image _fillImage;
    [SerializeField] private Image _baseImage;
    [SerializeField] private Image _iconImage;
    [Header("Data Settings")]
    [SerializeField] private HealthVisualData _optimalData;
    [SerializeField] private HealthVisualData _stableData;
    [SerializeField] private HealthVisualData _unstableData;
    [SerializeField] private HealthVisualData _criticalData;
    [Header("Time Settings")]
    [SerializeField] private float _decreaseDuration;

    [Inject]
    public void Construct(PlayerHealth health) => _health = health;
    private void OnEnable() => _health.OnHealthChanged += Health_OnHealthChanged;
    private void OnDisable() => _health.OnHealthChanged -= Health_OnHealthChanged;
    private void Health_OnHealthChanged(float percentage, HealthState healthState)
    {
        UpdateFillAmount(percentage);
        UpdateHUD(healthState);
    }
    public void InitializeHUD(float percentage) => UpdateFillAmount(percentage);
    private void UpdateFillAmount(float percentage) => _fillImage.DOFillAmount(percentage, _decreaseDuration);
    private void UpdateHUD(HealthState healthState)
    {
        switch(healthState)
        {
            case HealthState.Critical: SetHealthVisuals(_criticalData); break;
            case HealthState.Unstable: SetHealthVisuals(_unstableData); break;
            case HealthState.Stable: SetHealthVisuals(_stableData); break;
            case HealthState.Optimal: SetHealthVisuals(_optimalData); break;
        }
    }
    private void SetHealthVisuals(HealthVisualData visualData)
    {
        _fillImage.sprite = visualData.Fill;
        _baseImage.sprite = visualData.Base;
        _iconImage.sprite = visualData.Icon;
    }
}

[Serializable]
public struct HealthVisualData
{
    public Sprite Fill;
    public Sprite Base;
    public Sprite Icon;
}
