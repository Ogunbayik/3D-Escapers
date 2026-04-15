using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using Zenject;

public class BoardManager : MonoBehaviour
{
    private SignalBus _signalBus;

    public List<CellGroup> _lethalCells = new List<CellGroup>();
    public List<GridCell> _cells = new List<GridCell>();

    [Header("Visual Settings")]
    [SerializeField] private GridCellView _gridPrefab;
    [Header("Data Settings")]
    [SerializeField] private BoardData _data;

    private GridCell[,] _allCells;

    //TODO Level için gereklilikler
    private float _nextLethalDuration = 1f;
    private int _maxGroupIndex = 7;
    private int _groupIndex = 0;

    private bool _isSequenceActive = false;

    [Inject]
    public void Construct(SignalBus signalBus) => _signalBus = signalBus;
    void Start() => SetupBoard();
    private void SetupBoard()
    {
        _allCells = new GridCell[_data.Width, _data.Height];

        for (int i = 0; i < _data.Width; i++)
        {
            for (int j = 0; j < _data.Height; j++)
            {
                GridCell cell = new GridCell(i, j, GridType.Switchable, GridStatus.Safe);
                //cell.SetCellColor(ColorType.Safe);
                cell.SetGridStatus(GridStatus.Safe);

                _allCells[i, j] = cell;

                var grid = Instantiate(_gridPrefab);
                var scale = new Vector3(_data.Scale, _data.Scale, _data.Scale);
                var spawnPosition = new Vector3(i, 0f, j);

                grid.name = $"Grid[{i},{j}]";
                grid.Configure(cell, scale, spawnPosition);
            }
        }

        CreateGoalCell();
    }
    private async UniTask StartLethalSequence()
    {
        while (_isSequenceActive)
        {
            ClearLethalCells();
            await UniTask.Delay(TimeSpan.FromSeconds(_nextLethalDuration));
            SetNextLethalGroup();
            await UniTask.Delay(TimeSpan.FromSeconds(_nextLethalDuration));
        }
    }
    public void ClearLethalCells()
    {
        if (_cells.Count == 0)
            return;

        foreach (var cell in _cells)
        {
            if (cell.CellType == GridType.Switchable)
                cell.SetGridStatus(GridStatus.Safe);
        }
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
        {
            if (cell.CellType != GridType.Locked)
                cell.SetGridStatus(GridStatus.Lethal);
        }

        _signalBus.Fire(new GameSignal.OnGridColorChanged());
    }
    private void IncreaseGroupIndex()
    {
        _groupIndex++;

        if (_groupIndex > _maxGroupIndex)
            _groupIndex = 0;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _isSequenceActive = true;
            StartLethalSequence().Forget();
        }
    }
    private void CreateGoalCell()
    {
        var cell = GetRandomCell();

        cell.SetCellType(GridType.Locked);
        cell.SetGridStatus(GridStatus.Goal);
    }
    public GridCell GetRandomCell()
    {
        //This is for Goal Cell
        var randomWidth = UnityEngine.Random.Range(0, _data.Width);
        var randomHeight = UnityEngine.Random.Range(0, _data.Height);

        var cell = _allCells[randomWidth, randomHeight];
        return cell;
    }

    public void Test() => Debug.Log("Changed!");
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
