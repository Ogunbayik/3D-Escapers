using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCellView : MonoBehaviour
{
    private MeshRenderer _meshRenderer;

    [SerializeField] private GameObject _gridVisual;

    private void Awake() => _meshRenderer = GetComponentInChildren<MeshRenderer>();
    public void SetColor(Color color) => _meshRenderer.material.color = color;
}
