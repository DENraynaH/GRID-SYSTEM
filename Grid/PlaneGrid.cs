using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneGrid<T> : IGridEnumerableTiles<T>
{
    protected readonly Vector2 _gridPosition;
    protected Vector2Int _gridSize;
    protected readonly Vector2 _tileScale;
    protected GridTile<T>[,] _gridTiles;

    public PlaneGrid(Vector2Int gridSize, Vector2 gridPosition, Vector2 tileSize)
    {
        _gridSize = gridSize;
        _gridPosition = gridPosition;
        _tileScale = tileSize;
    }
    
    public void GenerateGrid()
    {
        _gridTiles = new GridTile<T>[_gridSize.x, _gridSize.y];
        
        for (int x = 0; x < _gridTiles.GetLength(0); x++)
        {
            for (int y = 0; y < _gridTiles.GetLength(1); y++)
            {
                _gridTiles[x, y] = new GridTile<T>();
            }
        }
    }

    public void ForEachTilePosition(Action<Vector3Int> gridPosition)
    {
        for (int x = 0; x < _gridTiles.GetLength(0); x++)
        {
            for (int y = 0; y < _gridTiles.GetLength(1); y++)
            {
                gridPosition.Invoke(new Vector3Int(x, y));
            }
        }
    }

    public void ForEachTilePositionAndScale(Action<Vector3Int, Vector3> gridPositionAndScale)
    {
        for (int x = 0; x < _gridTiles.GetLength(0); x++)
        {
            for (int y = 0; y < _gridTiles.GetLength(1); y++)
            {
                gridPositionAndScale.Invoke(new Vector3Int(x, y), _tileScale);
            }
        }
    }
    
    public void ForEachTile(Action<GridTile<T>> gridTile)
    {
        for (int x = 0; x < _gridTiles.GetLength(0); x++)
        {
            for (int y = 0; y < _gridTiles.GetLength(1); y++)
            {
                gridTile.Invoke(_gridTiles[x, y]);
            }
        }
    }

    public bool SetTileObject(Vector2Int gridPosition, T updatedObject)
    {
        bool gotTile = TryGetTile(gridPosition, out GridTile<T> gridTile);
        
        if (gotTile)
        {
            gridTile.OccupiedObject = updatedObject;
            return true;
        }

        return false;
    }
    
    public bool TryGetTile(Vector2Int gridPosition, out GridTile<T> gridTile)
    {
        bool withinBounds = Contains(gridPosition);

        if (withinBounds)
        {
            gridTile = _gridTiles[gridPosition.x, gridPosition.y];
            return true;
        }
        
        gridTile = default;
        return false;
    }
    
    public GridTile<T> GetTile(Vector2Int gridPosition)
    {
        bool withinBounds = Contains(gridPosition);

        if (withinBounds)
            return _gridTiles[gridPosition.x, gridPosition.y];
        
        return null;
    }
    
    public bool Contains(Vector2Int gridPosition)
    {
        return (gridPosition.x <= _gridSize.x)
               && (gridPosition.y <= _gridSize.y);
    }
    
    public Vector2 GridToWorldPosition(Vector2Int gridPosition)
    {
        return new Vector2(
            gridPosition.x * _tileScale.x + _gridPosition.x, 
            gridPosition.y * _tileScale.y + _gridPosition.y) 
               + _tileScale / 2f;
    }
    
    public Vector2Int WorldToGridPosition(Vector2 worldPosition)
    {
        return new Vector2Int(
            Mathf.FloorToInt((worldPosition.x - _gridPosition.x) / _tileScale.x), 
            Mathf.FloorToInt((worldPosition.y - _gridPosition.y) / _tileScale.y));
    }
}
