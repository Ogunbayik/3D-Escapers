using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCellView : MonoBehaviour
{
    private GridCell _grid;

    private MeshRenderer _meshRenderer;
    private BoxCollider _collider;

    [SerializeField] private GameObject _cellVisual;
    [SerializeField] private Vector3 _gridScale;

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
    public void Configure(GridCell grid, Vector3 scale, Vector3 spawnPosition)
    {
        if (_grid != null) _grid.OnGridStatusChanged += HandleGridStatueChanged;

        _grid = grid;
        _cellVisual.transform.localScale = scale;
        _collider.size = _gridScale;
        transform.position = spawnPosition;

        _grid.OnGridStatusChanged += HandleGridStatueChanged;

        HandleGridStatueChanged(_grid.GridStatus);
    }
    public void HandleGridStatueChanged(GridStatus gridStatus)
    {
        switch(gridStatus)
        {
            case GridStatus.Safe: SetColor(_safeColor);
                break;
            case GridStatus.Lethal: SetColor(_lethalColor);
                break;
            case GridStatus.Goal: SetColor(_goalColor);
                break;
        }
    }
    private void SetColor(Color color) => _meshRenderer.material.color = color;
}
