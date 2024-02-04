using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameProject.Core
{
    public class WorldGridExample : MonoBehaviour
    {
        [SerializeField] private Vector3Int _gridSize;
        [SerializeField] private Vector3 _gridPosition;
        [SerializeField] private Vector3 _tileScale;

        private WorldGrid<GameObject> _worldGrid;

        private void Start()
        {
            _worldGrid = new WorldGrid<GameObject>(_gridSize, _gridPosition, _tileScale);
            _worldGrid.GenerateGrid();

            /*_worldGrid.ForEachTile((gridPosition) =>
            {
                Vector3 worldPosition = _worldGrid.GridToWorldPosition(gridPosition);
                GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
                _worldGrid.GetTile(gridPosition).OccupiedObject = tile;
                
                tile.transform.position = worldPosition;
                tile.transform.localScale = _tileScale;
            });*/

            Vector3Int depth = new Vector3Int(_gridSize.x, _gridSize.y, 1);
            _worldGrid.ForTiles(depth, (gridPosition) =>
            {
                Vector3 worldPosition = _worldGrid.GridToWorldPosition(gridPosition);
                GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
                _worldGrid.GetTile(gridPosition).OccupiedObject = tile;
                
                tile.transform.position = worldPosition;
                tile.transform.localScale = _tileScale;
            });


        }

        private void OnValidate()
        {
            _worldGrid = new WorldGrid<GameObject>(_gridSize, _gridPosition, _tileScale);
            _worldGrid.GenerateGrid();
        }

        private void OnDrawGizmosSelected()
        {
            _worldGrid?.ForEachTile((gridPosition) =>
            {
                Gizmos.color = Color.white;
                Vector3 worldPosition = _worldGrid.GridToWorldPosition(gridPosition);
                Gizmos.DrawWireCube(worldPosition, _tileScale);
            });
        }
    }
}
