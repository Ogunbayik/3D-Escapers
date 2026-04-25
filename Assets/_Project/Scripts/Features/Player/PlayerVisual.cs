using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerVisual : MonoBehaviour
{
    private PlayerHealth _health;

    private SkinnedMeshRenderer _meshRenderer;

    [Header("Visual References")]
    [SerializeField] private Transform _body;

    public Transform Body => _body;

    [Inject]
    public void Construct(SkinnedMeshRenderer meshRenderer, PlayerHealth health)
    {
        _meshRenderer = meshRenderer;
        _health = health;
    }
    private void OnEnable() => _health.OnHealthChanged += OnHealthChanged;
    private void OnDisable() => _health.OnHealthChanged -= OnHealthChanged;
    private void OnHealthChanged(int currentHealth, int maximumHealth) => _meshRenderer.material.color = Color.red;


}
