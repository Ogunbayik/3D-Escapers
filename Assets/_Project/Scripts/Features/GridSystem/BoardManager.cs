using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private GridCellView _gridPrefab;

    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private float _gridScale;

    private GridCell[,] _allCells;
    void Start()
    {
        SetupBoard();
    }
    private void SetupBoard()
    {
        _allCells = new GridCell[_width, _height];

        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                GridCell cell = new GridCell(i, j, CellType.Switchable);
                cell.SetCellColor(ColorType.Lethal);

                _allCells[i, j] = cell;

                var grid = Instantiate(_gridPrefab);
                var scale = new Vector3(_gridScale, _gridScale,_gridScale);
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
        var randomWidth = Random.Range(0, _width);
        var randomHeight = Random.Range(0, _height);

        var testCell = _allCells[randomWidth, randomHeight];
        return testCell;
    }
}
