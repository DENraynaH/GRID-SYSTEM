using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile<T>
{
    public bool IsOccupied => OccupiedObject != null;
    public T OccupiedObject { get; set; }
    public Vector3Int GridPosition { get; set; }
}
