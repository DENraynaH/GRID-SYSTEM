using System;
using UnityEngine;

public interface IGridEnumerableTiles<T>
{
    void ForEachTilePosition(Action<Vector3Int> gridPosition);
    void ForEachTilePositionAndScale(Action<Vector3Int, Vector3> gridPositionAndScale);
    void ForEachTile(Action<GridTile<T>> gridTile);
}