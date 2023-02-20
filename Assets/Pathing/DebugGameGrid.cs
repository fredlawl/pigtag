using Pathing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class DebugGameGrid : MonoBehaviour
{
    /**
    * Do not have this enabled while drawing
    * on the tilemap. Exception out of bound
    * errors go wild! Also, do not forget to
    * compress the tile map!
    */
    public bool enableDebugging = false;

    private GameGrid grid;

    Tilemap difficultTerrain;

    // Start is called before the first frame update
    private void OnValidate()
    {
        grid = new GameGrid(GetComponent<Tilemap>());
        difficultTerrain = GameObject.Find("DifficultTerrain").GetComponent<Tilemap>();

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
        if (!enableDebugging)
        {
            return;
        }

        if (grid == null)
        {
            return;
        }

        for (int y = 0; y < grid.size.y; y++)
        {
            for (int x = 0; x < grid.size.x; x++)
            {
                Node n = grid.Cell(x, y);
                Gizmos.DrawWireCube(n.worldPosition, grid.cellSize);
            }
        }

        foreach (Node n in grid.Obstacles())
        {
            Gizmos.DrawCube(n.worldPosition, grid.cellSize);
            Gizmos.DrawCube(n.gridPosition, grid.cellSize);
        }
    }
}
