using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class BoardManager : MonoBehaviour
{
    public List<CellGroup> _lethalCells = new List<CellGroup>();
    public List<GridCell> _cells = new List<GridCell>();

    [Header("Visual Settings")]
    [SerializeField] private GridCellView _gridPrefab;
    [Header("Data Settings")]
    [SerializeField] private BoardData _data;
    private GridCell[,] _allCells;

    private float _nextLethalDuration = 0.5f;
    private int _maxGroupIndex = 7;
    private int _groupIndex = 0;
    void Start() => SetupBoard();
    private void SetupBoard()
    {
        _allCells = new GridCell[_data.Width, _data.Height];

        for (int i = 0; i < _data.Width; i++)
        {
            for (int j = 0; j < _data.Height; j++)
            {
                GridCell cell = new GridCell(i, j, CellType.Switchable);
                cell.SetCellColor(ColorType.Safe);

                _allCells[i, j] = cell;

                var grid = Instantiate(_gridPrefab);
                var scale = new Vector3(_data.Scale, _data.Scale, _data.Scale);
                var spawnPosition = new Vector3(i, 0f, j);

                grid.Initialize(cell, scale, spawnPosition);
            }
        }
    }
    private async UniTask LethalCellSequence()
    {
        ClearLethalCells();

        await UniTask.Delay(TimeSpan.FromSeconds(_nextLethalDuration));

        SetNextLethalGroup();
    }
    public void ClearLethalCells()
    {
        if (_cells.Count == 0)
            return;

        foreach (var cell in _cells)
            cell.SetCellColor(ColorType.Safe);
    }
    public void SetNextLethalGroup()
    {
        _cells.Clear();

        //TODO System changing for level
        foreach (var cell in _lethalCells)
        {
            if (cell.GroupID == _groupIndex)
            {
                foreach (var coordinate in cell.Coordinates)
                    _cells.Add(_allCells[coordinate.Width, coordinate.Height]);
            }
        }

        ChangeLethalCellColor();
        IncreaseGroupIndex();
    }
    private void ChangeLethalCellColor()
    {
        foreach (var cell in _cells)
            cell.SetCellColor(ColorType.Lethal);
    }
    private void IncreaseGroupIndex()
    {
        _groupIndex++;

        if (_groupIndex > _maxGroupIndex)
            _groupIndex = 0;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            LethalCellSequence().Forget();
        }
    }
    public GridCell GetRandomCell()
    {
        var randomWidth = UnityEngine.Random.Range(0, _data.Width);
        var randomHeight = UnityEngine.Random.Range(0, _data.Height);

        var testCell = _allCells[randomWidth, randomHeight];
        return testCell;
    }
}

[System.Serializable]
public struct CellCoordinate
{
    public int Width;
    public int Height;
}

[System.Serializable]
public struct CellGroup
{
    public int GroupID;
    public List<CellCoordinate> Coordinates;
}
