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
    private CollectibleItemFactory _collectibleFactory;

    private GridCellView.Pool _gridPool;

    private VFXManager _VFXManager;
    private ScoreManager _scoreManager;
    private LevelManager _levelManager;
    private SignalBus _signalBus;

    public List<GridCell> _lethalGrids = new List<GridCell>();
    public List<GridCell> _allGridList = new List<GridCell>();
    public List<GridCellView> _allGridViews = new List<GridCellView>();

    [Header("Visual Settings")]
    [SerializeField] private GridCellView _gridPrefab;
    [Header("Data Settings")]
    [SerializeField] private BoardData _data;
    [Header("Transform References")]
    [SerializeField] private Transform _spawnPosition;

    private LevelData _activeLevelData;

    private GridCell[,] _allGrid;

    private GridCell _goalGrid;

    //TODO Level için gereklilikler
    private int _groupIndex = 0;

    private bool _isSequenceActive = false;

    public BoardData Data => _data;

    [Inject]
    public void Construct(SignalBus signalBus,
        ScoreManager scoreManager,
        VFXManager VFXManager,
        LevelManager levelManager,
        GridCellView.Pool gridPool,
        CollectibleItemFactory collectibleItemFactory)
    {
        _signalBus = signalBus;
        _scoreManager = scoreManager;
        _VFXManager = VFXManager;
        _levelManager = levelManager;
        _gridPool = gridPool;
        _collectibleFactory = collectibleItemFactory;
    }
    private void OnEnable()
    {
        _signalBus.Subscribe<GameSignal.OnPlayerGridStatus>(OnPlayerGridStatusReached);
        _signalBus.Subscribe<GameSignal.OnLevelScoreReached>(CreateCollectibleItem);
    }
    private void OnDisable()
    {
        _signalBus.Unsubscribe<GameSignal.OnPlayerGridStatus>(OnPlayerGridStatusReached);
        _signalBus.Unsubscribe<GameSignal.OnLevelScoreReached>(CreateCollectibleItem);
    }
    private void OnPlayerGridStatusReached(GameSignal.OnPlayerGridStatus signal)
    {
        if (signal.GridStatus == GridStatus.Goal)
            ProcessGoalReachedSequence().Forget();
    }
    public async UniTask SetupBoard(LevelData levelData)
    {
        _activeLevelData = levelData;

        _allGrid = new GridCell[levelData.Width, levelData.Height];

        for (int i = 0; i < levelData.Height; i++)
        {
            for (int j = 0; j < levelData.Width; j++)
            {
                int id = i * levelData.Width + j;
                GridCell cell = new GridCell(id, j, i, GridType.Switchable, GridStatus.Safe);
                cell.SetGridStatus(GridStatus.Safe);

                _allGrid[j, i] = cell;
                _allGridList.Add(cell);

                var grid = _gridPool.Spawn(_gridPool);
                _allGridViews.Add(grid);
                
                var spawnPosition = new Vector3(j, _data.SpawnY, i);

                grid.name = $"Grid[{j},{i}]";
                grid.Configure(cell, spawnPosition);
                grid.AnimateMove(_data.TargetY, _data.SpawnPerDuration);

                await UniTask.Delay(TimeSpan.FromSeconds(_data.SpawnPerDuration));
            }
        }
    }
    public void StartLethalSequence()
    {
        _isSequenceActive = true;
        LethalSequence().Forget();
    }
    public void StopLethalSequence() => _isSequenceActive = false;
    public void ReturnAllCells()
    {
        foreach (var view in _allGridViews)
            view.ReturnToPool();
    }
    public void ClearBoard()
    {
        foreach (var gridView in _allGridViews)
        {
            _gridPool.Despawn(gridView);
        }

        _allGridViews.Clear();
        _allGridList.Clear();
        _lethalGrids.Clear();

        _groupIndex = 0;
        _allGrid = null;
    }
    private async UniTask LethalSequence()
    {
        var token = this.GetCancellationTokenOnDestroy();

        while (_isSequenceActive)
        {
            ClearLethalCells();
            SetNextLethalGroup();
            await UniTask.Delay(TimeSpan.FromSeconds(_activeLevelData.LethalDuration), cancellationToken: token);
        }
    }
    private async UniTask ProcessGoalReachedSequence()
    {
        ResetGoalGrid();

        var spawnPosition = new Vector3(_goalGrid.Width, 0f, _goalGrid.Height);
        _VFXManager.PlayGoalEffect(spawnPosition);
        _scoreManager.AddScore(_activeLevelData.ScorePerGoal);

        await UniTask.Delay(TimeSpan.FromSeconds(_data.GoalEffectDelay));

        if (!_scoreManager.IsReachedScore())
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
    private void CreateCollectibleItem()
    {
        var collectibleItem = _activeLevelData.CollectableItem;

        var item = _collectibleFactory.Create(collectibleItem, _spawnPosition.position);
        item.PlayIdleAnimation();
    }
    public void SetNextLethalGroup()
    {
        _lethalGrids.Clear();

        //TODO System changing for level
        foreach (var cell in _activeLevelData.LethalGroups)
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

        if (_groupIndex >= _activeLevelData.LethalGroups.Count)
            _groupIndex = 0;
    }
    public void InitialGoalGrid()
    {
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
        var randomWidth = UnityEngine.Random.Range(0, _activeLevelData.Width);
        var randomHeight = UnityEngine.Random.Range(0, _activeLevelData.Height);

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
