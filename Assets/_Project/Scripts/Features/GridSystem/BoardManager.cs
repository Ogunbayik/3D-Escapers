using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private GridCellView _gridPrefab;

    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private float distanceToGrid;

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
                GridCell cell = new GridCell(i, j, GridType.Switchable);
                _allCells[i, j] = cell;

                var grid = Instantiate(_gridPrefab);
                var spawnPosition = new Vector3(i * distanceToGrid, 0f, j * distanceToGrid);
                grid.transform.position = spawnPosition;
                grid.SetColor(Color.green);
            }
        }
    }
    void Update()
    {
        
    }
}
