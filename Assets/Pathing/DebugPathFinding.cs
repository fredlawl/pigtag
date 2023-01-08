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
