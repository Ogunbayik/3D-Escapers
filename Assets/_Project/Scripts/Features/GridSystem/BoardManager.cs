using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using Zenject;
using DG.Tweening;

public class BoardManager : MonoBehaviour
{
    private GridCellView.Pool _gridPool;

    private VFXManager _VFXManager;
    private ScoreManager _scoreManager;
    private SignalBus _signalBus;

    public List<GridCell> _lethalGrids = new List<GridCell>();
    public List<GridCell> _allGridList = new List<GridCell>();

    [Header("Visual Settings")]
    [SerializeField] private GridCellView _gridPrefab;
    [Header("Data Settings")]
    [SerializeField] private BoardData _data;

    private GridCell[,] _allGrid;

    private GridCell _goalGrid;

    //TODO Level için gereklilikler
    private int _groupIndex = 0;

    private LevelData _currentLevel = null;

    private bool _isSequenceActive = false;

    [Inject]
    public void Construct(SignalBus signalBus, ScoreManager scoreManager, VFXManager VFXManager, GridCellView.Pool gridPool)
    {
        _signalBus = signalBus;
        _scoreManager = scoreManager;
        _VFXManager = VFXManager;
        _gridPool = gridPool;
    }
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
            ProcessGoalReachedSequence().Forget();
    }
    public void SetupBoard(LevelData levelData)
    {
        _currentLevel = levelData;

        _allGrid = new GridCell[_currentLevel.Width, _currentLevel.Height];

        for (int i = 0; i < _currentLevel.Height; i++)
        {
            for (int j = 0; j < _currentLevel.Width; j++)
            {
                int id = i * _currentLevel.Width + j;
                GridCell cell = new GridCell(id, j, i, GridType.Switchable, GridStatus.Safe);
                cell.SetGridStatus(GridStatus.Safe);

                _allGrid[j,i] = cell;
                _allGridList.Add(cell);

                var grid = _gridPool.Spawn(_gridPool);
                var spawnPosition = new Vector3(j, 0f, i);

                grid.name = $"Grid[{j},{i}]";
                grid.transform.localScale = Vector3.zero;
                grid.Configure(cell, spawnPosition);
                grid.IncreaseScale(Vector3.one, _data.IncreaseDuration);
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
            await UniTask.Delay(TimeSpan.FromSeconds(_currentLevel.LethalDuration), cancellationToken: token);
        }
    }
    private async UniTask ProcessGoalReachedSequence()
    {
        ResetGoalGrid();
        //TODO Some effect for reach
        var spawnPosition = new Vector3(_goalGrid.Width, 0f, _goalGrid.Height);
        _VFXManager.PlayGoalEffect(spawnPosition);

        await UniTask.Delay(TimeSpan.FromSeconds(_data.GoalEffectDelay));

        _scoreManager.IncreaseScore(_currentLevel.ScorePerGoal);
        CreateNewGoalGrid();

        await UniTask.Delay(TimeSpan.FromSeconds(_data.NextGoalDelay));
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
        foreach (var cell in _currentLevel.LethalGroups)
        {
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

        if (_groupIndex >= _currentLevel.LethalGroups.Count)
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
        var randomWidth = UnityEngine.Random.Range(0, _currentLevel.Width);
        var randomHeight = UnityEngine.Random.Range(0, _currentLevel.Height);

        var grid = _allGrid[randomWidth, randomHeight];
        return grid;
    }
}

[System.Serializable]
public struct CellGroup
{
    public int GroupID;
    public List<int> CellIDs;
}
