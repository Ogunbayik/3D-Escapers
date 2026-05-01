using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using Zenject;

public class BoardManager : MonoBehaviour
{
    private VFXManager _VFXManager;
    private ScoreManager _scoreManager;
    private SignalBus _signalBus;

    public List<CellGroup> _lethalGroups = new List<CellGroup>();
    public List<GridCell> _lethalGrids = new List<GridCell>();
    public List<GridCell> _allGridList = new List<GridCell>();

    [Header("Visual Settings")]
    [SerializeField] private GridCellView _gridPrefab;
    [Header("Data Settings")]
    [SerializeField] private BoardData _data;

    private GridCell[,] _allGrid;

    private GridCell _goalGrid;

    //TODO Level için gereklilikler
    private int _maxGroupIndex = 7;
    private int _groupIndex = 0;

    private bool _isSequenceActive = false;

    [Inject]
    public void Construct(SignalBus signalBus, ScoreManager scoreManager, VFXManager VFXManager)
    {
        _signalBus = signalBus;
        _scoreManager = scoreManager;
        _VFXManager = VFXManager;
    }
    void Start() => SetupBoard();
    private void OnEnable()
    {
        _signalBus.Subscribe<GameSignal.OnPlayerGridStatus>(OnPlayerGridStatusReached);
    }
    private void OnDisable()
    {
        _signalBus.Unsubscribe<GameSignal.OnPlayerGridStatus>(OnPlayerGridStatusReached);
    }
    private void OnPlayerGridStatusReached(GameSignal.OnPlayerGridStatus signal)
    {
        if (signal.GridStatus == GridStatus.Goal)
            StartGoalSequence().Forget();
    }
    private void SetupBoard()
    {
        _allGrid = new GridCell[_data.Width, _data.Height];

        for (int i = 0; i < _data.Height; i++)
        {
            for (int j = 0; j < _data.Width; j++)
            {
                int id = i * _data.Width + j;
                GridCell cell = new GridCell(id, j, i, GridType.Switchable, GridStatus.Safe);
                cell.SetGridStatus(GridStatus.Safe);

                _allGrid[j,i] = cell;
                _allGridList.Add(cell);

                var grid = Instantiate(_gridPrefab);
                var scale = new Vector3(_data.Scale, _data.Scale, _data.Scale);
                var spawnPosition = new Vector3(j, 0f, i);

                grid.name = $"Grid[{j},{i}]";
                grid.Configure(cell, scale, spawnPosition);
            }
        }

        InitialGoalGrid();
    }
    private async UniTask StartLethalSequence()
    {
        var token = this.GetCancellationTokenOnDestroy();

        while (_isSequenceActive)
        {
            ClearLethalCells();
            SetNextLethalGroup();
            await UniTask.Delay(TimeSpan.FromSeconds(_data.LethalDuration), cancellationToken: token);
        }
    }
    private async UniTask StartGoalSequence()
    {
        float reachDuration = 0.4f;
        float goalDuration = 0.5f;
        int testScore = 10;

        ResetGoalGrid();
        //TODO Some effect for reach
        var spawnPosition = new Vector3(_goalGrid.Width, 0f, _goalGrid.Height);
        _VFXManager.PlayGoalEffect(spawnPosition);
        await UniTask.Delay(TimeSpan.FromSeconds(reachDuration));
        _scoreManager.IncreaseScore(testScore);
        CreateNewGoalGrid();
        await UniTask.Delay(TimeSpan.FromSeconds(goalDuration));
    }
    public void ClearLethalCells()
    {
        if (_lethalGrids.Count == 0)
            return;

        foreach (var cell in _lethalGrids)
        {
            if (cell.CellType == GridType.Switchable)
                cell.SetGridStatus(GridStatus.Safe);
        }
    }
    public void SetNextLethalGroup()
    {
        _lethalGrids.Clear();

        //TODO System changing for level
        foreach (var cell in _lethalGroups)
        {
            /*
            if (cell.GroupID == _groupIndex)
            {
                foreach (var coordinate in cell.Coordinates)
                    _lethalGrids.Add(_allGrid[coordinate.Width, coordinate.Height]);
            }
            */

            if(cell.GroupID == _groupIndex)
            {
                foreach (var id in cell.CellIDs)
                    _lethalGrids.Add(_allGridList[id]);
            }
        }

        ChangeLethalCellColor();
        IncreaseGroupIndex();
    }
    private void ChangeLethalCellColor()
    {
        foreach (var cell in _lethalGrids)
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
    public void InitialGoalGrid()
    {
        //TODO Using this method when the game started!
        _goalGrid = GetRandomGrid();
        ActivateGoalGrid();
    }
    public void CreateNewGoalGrid()
    {
        _goalGrid = GetDifferentGoalGrid();
        ActivateGoalGrid();
    }
    public void ActivateGoalGrid()
    {
        _goalGrid.SetCellType(GridType.Locked);
        _goalGrid.SetGridStatus(GridStatus.Goal);
    }
    public void ResetGoalGrid()
    {
        _goalGrid.SetCellType(GridType.Switchable);
        _goalGrid.SetGridStatus(GridStatus.Safe);
    }
    public GridCell GetDifferentGoalGrid()
    {
        GridCell newGrid;
        int totalAttempt = 0;
        int maxAttempt = 100;

        do
        {
            newGrid = GetRandomGrid();
            totalAttempt++;
        }
        while (newGrid.Height == _goalGrid.Height && newGrid.Width == _goalGrid.Width && totalAttempt < maxAttempt);

        return newGrid;
    }
    public GridCell GetRandomGrid()
    {
        //This is for Goal Cell
        var randomWidth = UnityEngine.Random.Range(0, _data.Width);
        var randomHeight = UnityEngine.Random.Range(0, _data.Height);

        var grid = _allGrid[randomWidth, randomHeight];
        return grid;
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
    public List<int> CellIDs;
}
