using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GridCellView : MonoBehaviour, IPoolable<IMemoryPool>
{
    private IMemoryPool _pool;

    private GridCell _grid;

    private MeshRenderer _meshRenderer;
    private BoxCollider _collider;

    [Header("Visual References")]
    [SerializeField] private GameObject _cellVisual;
    [Header("Color Settings")]
    [SerializeField] private Color _safeColor;
    [SerializeField] private Color _lethalColor;
    [SerializeField] private Color _goalColor;
    public GridCell Grid => _grid;
    private void Awake() => SetupReferences();
    private void SetupReferences()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _collider = GetComponent<BoxCollider>();
    }
    private void OnDisable()
    {
        if (_grid != null)
            _grid.OnGridStatusChanged -= HandleGridStatueChanged;
    }
    public void Configure(GridCell grid, Vector3 spawnPosition)
    {
        if (_grid != null) _grid.OnGridStatusChanged += HandleGridStatueChanged;

        _grid = grid;
        transform.position = spawnPosition;

        _grid.OnGridStatusChanged += HandleGridStatueChanged;

        HandleGridStatueChanged(_grid.GridStatus);
    }
    public void HandleGridStatueChanged(GridStatus gridStatus)
    {
        transform.DOKill();

        switch(gridStatus)
        {
            case GridStatus.Safe: SetColor(_safeColor);
                transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
                break;
            case GridStatus.Lethal: SetColor(_lethalColor);
                break;
            case GridStatus.Goal: SetColor(_goalColor);
                AnimateGoalGrid();
                break;
        }
    }
    public void IncreaseScale(Vector3 scale, float duration) => transform.DOScale(scale, duration).SetEase(Ease.OutBounce);
    public void AnimateMove(float target, float duration) => transform.DOMoveY(target, duration).SetEase(Ease.OutBack);
    public void AnimateGoalGrid() => transform.DOScale(0.8f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    private void SetColor(Color color) => _meshRenderer.material.color = color;
    public void OnSpawned(IMemoryPool pool)
    {
        _pool = pool;
    }
    public void OnDespawned()
    {

    }
    public void ReturnToPool()
    {
        _pool.Despawn(this);
    }

    public class Pool : MonoPoolableMemoryPool<IMemoryPool,GridCellView> { }
}
