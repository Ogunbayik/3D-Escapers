using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCellView : MonoBehaviour
{
    private GridCell _cell;

    private MeshRenderer _meshRenderer;

    [SerializeField] private GameObject _cellVisual;

    [SerializeField] private Color _safeColor;
    [SerializeField] private Color _lethalColor;
    [SerializeField] private Color _goalColor;
    private void Awake() => _meshRenderer = GetComponentInChildren<MeshRenderer>();
    private void OnDisable()
    {
        if (_cell != null)
            _cell.OnColorChanged -= HandleColorChanged;
    }
    public void Initialize(GridCell cell, Vector3 scale, Vector3 spawnPosition)
    {
        if (_cell != null) _cell.OnColorChanged -= HandleColorChanged;

        _cell = cell;
        _cellVisual.transform.localScale = scale;
        transform.position = spawnPosition;

        _cell.OnColorChanged += HandleColorChanged;

        HandleColorChanged(_cell.ColorType);
    }
    public void HandleColorChanged(ColorType colortype)
    {
        switch(colortype)
        {
            case ColorType.Safe: SetColor(_safeColor);
                break;
            case ColorType.Lethal: SetColor(_lethalColor);
                break;
            case ColorType.Goal: SetColor(_goalColor);
                break;
            default:
                SetColor(Color.black);
                break;
        }
    }
    private void SetColor(Color color) => _meshRenderer.material.color = color;
}
