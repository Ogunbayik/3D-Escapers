using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Header("Visual Settings")]
    [SerializeField] private GridCellView _gridPrefab;
    [Header("Data Settings")]
    [SerializeField] private BoardData _data;
    private GridCell[,] _allCells;
    void Start()
    {
        SetupBoard();
    }
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
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            var cell = GetRandomCell();
            cell.SetCellType(CellType.Locked);
            cell.SetCellColor(ColorType.Goal);
        }
    }
    public GridCell GetRandomCell()
    {
        var randomWidth = Random.Range(0, _data.Width);
        var randomHeight = Random.Range(0, _data.Height);

        var testCell = _allCells[randomWidth, randomHeight];
        return testCell;
    }
}
