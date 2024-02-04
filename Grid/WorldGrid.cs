using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WorldGrid<T>
{
    public Vector3Int GridSize => _gridSize;
    public Vector3 GridPosition => _gridPosition;
    public Vector3 TileScale => _tileScale;
    
    protected readonly Vector3 _gridPosition;
    protected Vector3Int _gridSize;
    protected readonly Vector3 _tileScale;
    protected GridTile<T>[,,] _gridTiles;
    
    public WorldGrid(Vector3Int gridSize, Vector3 gridPosition, Vector3 tileSize)
    {
        _gridSize = gridSize;
        _gridPosition = gridPosition;
        _tileScale = tileSize;
    }
    
    public void GenerateGrid()
    {
        _gridTiles = new GridTile<T>[_gridSize.x, _gridSize.y, _gridSize.z];
        
        for (int x = 0; x < _gridTiles.GetLength(0); x++)
        {
            for (int y = 0; y < _gridTiles.GetLength(1); y++)
            {
                for (int z = 0; z < _gridTiles.GetLength(2); z++)
                {
                    _gridTiles[x, y, z] = new GridTile<T>() 
                        { GridPosition = new Vector3Int(x, y, z) };
                }
            }
        }
    }
    
    public void ForEachTile(Action<Vector3Int> gridPosition)
    {
        for (int x = 0; x < _gridTiles.GetLength(0); x++)
        {
            for (int y = 0; y < _gridTiles.GetLength(1); y++)
            {
                for (int z = 0; z < _gridTiles.GetLength(2); z++)
                {
                    gridPosition.Invoke(new Vector3Int(x, y, z));
                }
            }
        }
    }
    
    public void ForEachTile(Action<GridTile<T>> gridTile)
    {
        for (int x = 0; x < _gridTiles.GetLength(0); x++)
        {
            for (int y = 0; y < _gridTiles.GetLength(1); y++)
            {
                for (int z = 0; z < _gridTiles.GetLength(2); z++)
                {
                    gridTile.Invoke(_gridTiles[x, y, z]);
                }
            }
        }
    }
    
    public void ForTilesAround(Vector3Int gridPosition, int radius, Action<Vector3Int> position)
    {
        for (int x = gridPosition.x - radius; x <= gridPosition.x + radius; x++)
        {
            for (int y = gridPosition.y - radius; y <= gridPosition.y + radius; y++)
            {
                for (int z = gridPosition.z - radius; z <= gridPosition.z + radius; z++)
                {
                    if (Contains(new Vector3Int(x, y, z)))
                        position.Invoke(new Vector3Int(x, y, z));
                }
            }
        }
    }
    
    public void ForTilesAround(Vector3Int gridPosition, int radius, Action<GridTile<T>> tile)
    {
        for (int x = gridPosition.x - radius; x <= gridPosition.x + radius; x++)
        {
            for (int y = gridPosition.y - radius; y <= gridPosition.y + radius; y++)
            {
                for (int z = gridPosition.z - radius; z <= gridPosition.z + radius; z++)
                {
                  if(Contains(new Vector3Int(x, y, z)))
                        tile.Invoke(_gridTiles[x, y, z]);
                }
            }
        }
    }

    public void ForTiles(Vector3Int gridDepth, Action<Vector3Int> tilePosition)
    {
        for (int x = 0; x < gridDepth.x; x++)
        {
            for (int y = 0; y < gridDepth.y; y++)
            {
                for (int z = 0; z < gridDepth.z; z++)
                {
                    Vector3Int gridPosition = new Vector3Int(x, y, z);
                    if (Contains(gridPosition))
                        tilePosition.Invoke(gridPosition);
                }
            }
        }
    }
    
    public void ForTiles(Vector3Int gridDepth, Action<GridTile<T>> tile)
    {
        for (int x = 0; x < gridDepth.x; x++)
        {
            for (int y = 0; y < gridDepth.y; y++)
            {
                for (int z = 0; z < gridDepth.z; z++)
                {
                    Vector3Int gridPosition = new Vector3Int(x, y, z);
                    if (Contains(gridPosition))
                        tile.Invoke(_gridTiles[x, y, z]);
                }
            }
        }
    }
    
    public bool TryGetTile(Vector3Int gridPosition, out GridTile<T> gridTile)
    {
        bool withinBounds = Contains(gridPosition);

        if (withinBounds)
        {
            gridTile = _gridTiles[gridPosition.x, gridPosition.y, gridPosition.z];
            return true;
        }

        gridTile = null;
        return false;
    }
    
    public GridTile<T> GetTile(Vector3Int gridPosition)
    {
        bool withinBounds = Contains(gridPosition);

        if (withinBounds)
            return _gridTiles[gridPosition.x, gridPosition.y, gridPosition.z];
        
        return null;
    }
    
    public bool Contains(Vector3Int gridPosition)
    {
        bool withinBounds = gridPosition.x >= 0 && gridPosition.x < _gridSize.x &&
                            gridPosition.y >= 0 && gridPosition.y < _gridSize.y &&
                            gridPosition.z >= 0 && gridPosition.z < _gridSize.z;

        return withinBounds;
    }

    public Vector3 GridToWorldPosition(Vector3Int gridPosition)
    {
        return new Vector3(
                   gridPosition.x * _tileScale.x + _gridPosition.x, 
                   gridPosition.y * _tileScale.y + _gridPosition.y,
                   gridPosition.z * _tileScale.z + _gridPosition.z) 
               + _tileScale / 2f;
    }
    
    public Vector3Int WorldToGridPosition(Vector3 worldPosition)
    {
        return new Vector3Int(
            Mathf.FloorToInt((worldPosition.x - _gridPosition.x) / _tileScale.x),
            Mathf.FloorToInt((worldPosition.y - _gridPosition.y) / _tileScale.y),
            Mathf.FloorToInt((worldPosition.z - _gridPosition.z) / _tileScale.z));
    }
    
    public Vector3 RandomWorldToGridWorldPosition(Vector3 worldPosition)
    {
        Vector3Int gridPosition = WorldToGridPosition(worldPosition);
        return GridToWorldPosition(gridPosition);
    }
    
    public Vector3Int GetTileAbove(Vector3Int gridPosition, int distance = 1)
    {
        return new Vector3Int(gridPosition.x, gridPosition.y + distance, gridPosition.z);
    }

    public Vector3Int GetTileBelow(Vector3Int gridPosition, int distance = 1)
    {
        return new Vector3Int(gridPosition.x, gridPosition.y - distance, gridPosition.z);
    }
    
    public Vector3Int GetTileForward(Vector3Int gridPosition, int distance = 1)
    {
        return new Vector3Int(gridPosition.x, gridPosition.y, gridPosition.z + distance);
    }
    
    public Vector3Int GetTileRight(Vector3Int gridPosition, int distance = 1)
    {
        return new Vector3Int(gridPosition.x + distance, gridPosition.y, gridPosition.z);
    }
}