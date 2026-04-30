using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerHUD : MonoBehaviour
{
    private SignalBus _signalBus;

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
    [Header("Animation Settings")]
    [SerializeField] private float _fadeDuration;
    [SerializeField] private float _decreaseScale;
    [SerializeField] private HealthAnimationData _unstableAnimationData;
    [SerializeField] private HealthAnimationData _criticalAnimationData;

    private Sequence _heartAnimationSequence;

    private float _originalScale;

    [Inject]
    public void Construct(PlayerHealth health, SignalBus signalBus)
    {
        _health = health;
        _signalBus = signalBus;

        _originalScale = _iconImage.transform.localScale.x;
    }
    private void OnEnable()
    {
        _health.OnHealthChanged += Health_OnHealthChanged;

        _signalBus.Subscribe<GameSignal.OnPlayerDead>(PlayDeadAnimation);
    }
    private void OnDisable()
    {
        _health.OnHealthChanged -= Health_OnHealthChanged;

        _signalBus.Unsubscribe<GameSignal.OnPlayerDead>(PlayDeadAnimation);
    }
    private void Health_OnHealthChanged(float percentage, HealthState healthState)
    {
        UpdateFillAmount(percentage);
        UpdateHUD(healthState);

        PlayHeartAnimationSequence(healthState);
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
    private void PlayHeartAnimationSequence(HealthState healthState)
    {
        _heartAnimationSequence.Kill();

        if (healthState != HealthState.Critical && healthState != HealthState.Unstable) return;

        switch(healthState)
        {
            case HealthState.Unstable: HeartSequence(_unstableAnimationData);
                break;
            case HealthState.Critical: HeartSequence(_criticalAnimationData);
                break;
        }
    }
    private void HeartSequence(HealthAnimationData animationData)
    {
        _heartAnimationSequence = DOTween.Sequence();

        _heartAnimationSequence.Append(_iconImage.transform.DOScale(_decreaseScale, animationData.DecreaseDuration));
        _heartAnimationSequence.Append(_iconImage.transform.DOScale(_originalScale, animationData.IncreaseDuration));
        _heartAnimationSequence.AppendInterval(animationData.RepeatDuration);
        _heartAnimationSequence.SetLoops(-1);
    }
    private void PlayDeadAnimation()
    {
        if (_heartAnimationSequence != null) _heartAnimationSequence.Kill();

        _iconImage.DOFade(0f, _fadeDuration);
    }
}

[Serializable]
public struct HealthVisualData
{
    public Sprite Fill;
    public Sprite Base;
    public Sprite Icon;
}
[Serializable] 
public struct HealthAnimationData
{
    public float DecreaseDuration;
    public float IncreaseDuration;
    public float RepeatDuration;
}
