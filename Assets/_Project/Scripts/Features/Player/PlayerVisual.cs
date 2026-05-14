using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class PlayerVisual : MonoBehaviour
{
    private static readonly int DissolveAmountProp = Shader.PropertyToID(GameConst.ShaderProperties.DISSOLVE_AMOUNT);
    private static readonly int PlayerBaseColorProp = Shader.PropertyToID(GameConst.ShaderProperties.PLAYER_BASE_COLOR);

    private CameraManager _cameraManager;
    private PlayerHealth _health;

    private SkinnedMeshRenderer _meshRenderer;

    [Header("Transform References")]
    [SerializeField] private Transform _body;
    [SerializeField] private Transform _startPosition;
    [Header("Color Settings")]
    [SerializeField] private Color _originalColor;
    [SerializeField] private Color _hitColor;
    [Header("Duration Settings")]
    [SerializeField] private float _changeDuration;
    [SerializeField] private float _changeMultiply;
    [Header("Dissolve Settings")]
    [SerializeField] private float _dissolveDuration;
    [SerializeField] private float _disapperDelay;
    [SerializeField] private float _teleportDelay;

    public Transform Body => _body;

    [Inject]
    public void Construct(SkinnedMeshRenderer meshRenderer, PlayerHealth health,CameraManager cameraManager)
    {
        _meshRenderer = meshRenderer;
        _health = health;
        _cameraManager = cameraManager;
    }
    private void OnEnable() => _health.OnHealthChanged += OnHealthChanged;
    private void OnDisable() => _health.OnHealthChanged -= OnHealthChanged;
    private void OnHealthChanged(float percentage, HealthState healthState)
    {
        _cameraManager.GenerateHitShake();
        OnPlayerColorChanged();
    }
    private void OnPlayerColorChanged()
    {
        _meshRenderer.material.DOKill();

        Sequence hitSequence = DOTween.Sequence();
        
        hitSequence.Append(_meshRenderer.material.DOColor(_hitColor, PlayerBaseColorProp, _changeDuration * _changeMultiply));
        hitSequence.Append(_meshRenderer.material.DOColor(_originalColor, PlayerBaseColorProp, _changeDuration));
    }
    public void PlayTeleportSequence(GameSignal.OnPlayerTeleportRequested signal) => ExecuteTeleportSequence(signal).Forget();
    public async UniTask ExecuteTeleportSequence(GameSignal.OnPlayerTeleportRequested signal)
    {
        AnimateDissolve(GameConst.ShaderProperties.DISSOLVE_DISAPPEAR_VALUE, _dissolveDuration);

        await UniTask.Delay(System.TimeSpan.FromSeconds(_disapperDelay));

        transform.position = signal.TeleportPosition;

        await UniTask.Delay(System.TimeSpan.FromSeconds(_teleportDelay));

        AnimateDissolve(GameConst.ShaderProperties.DISSOLVE_APPEAR_VALUE, _dissolveDuration);
    }
    public void AnimateDissolve(float targetValue, float duration) => _meshRenderer.material.DOFloat(targetValue, DissolveAmountProp, duration);

}
