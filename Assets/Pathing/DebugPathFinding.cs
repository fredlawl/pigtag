using Pathing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class DebugPathFinding : MonoBehaviour
{
    public Tilemap tilemap;
    public Transform target;
    private GameGrid grid;

    private void OnValidate()
    {
        if (tilemap == null || target == null)
        {
            return;
        }

        grid = new GameGrid(tilemap);

        Tilemap difficultTerrain = GameObject.Find("DifficultTerrain").GetComponent<Tilemap>();
        // Necessary for loading in difficult terrain based on tile map data
        for (int y = difficultTerrain.cellBounds.y; y < difficultTerrain.cellBounds.yMax; y++)
        {
            for (int x = difficultTerrain.cellBounds.x; x < difficultTerrain.cellBounds.xMax; x++)
            {
                var pos = new Vector3Int(x, y, 0);
                if (difficultTerrain.HasTile(pos))
                {
                    grid.AddObstacle(grid.GetNodeFromWorldPosition(pos).gridPosition);
                }
            }
        }

        grid.HydrateObstacles();
    }

    private void OnDrawGizmos()
    {
        var path = grid.FindPath(transform.position, target.position);
        Gizmos.color = Color.black;
        foreach (Vector3 node in path)
        {
            Gizmos.DrawCube(node, new Vector3(1, 1, -1));
        }
    }
}
