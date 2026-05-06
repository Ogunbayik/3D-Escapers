using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using DG.Tweening;

public class PlayerVisual : MonoBehaviour
{
    private CameraManager _cameraManager;
    private PlayerHealth _health;

    private SkinnedMeshRenderer _meshRenderer;

    [Header("Visual References")]
    [SerializeField] private Transform _body;
    [Header("Color Settings")]
    [SerializeField] private Color _originalColor;
    [SerializeField] private Color _hitColor;
    [Header("Duration Settings")]
    [SerializeField] private float _changeDuration;

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

        hitSequence.Append(_meshRenderer.material.DOColor(_hitColor, _changeDuration * 0.25f));
        hitSequence.Append(_meshRenderer.material.DOColor(_originalColor, _changeDuration));
    }

}
