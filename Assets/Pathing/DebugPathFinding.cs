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

    private Pathfinder pather;
    private GameGrid grid;

    private void OnValidate()
    {
        if (tilemap == null || target == null)
        {
            return;
        }

        grid = new GameGrid(tilemap);
        pather = new Pathfinder(grid);
        grid.MarkObstructables();
    }

    private void OnDrawGizmos()
    {
        var path = pather.FindPath(pather.GetNode(transform.position), pather.GetNode(target.position));
        Gizmos.color = Color.black;
        foreach (Node node in path)
        {
            Gizmos.DrawCube(node.mapWorldPosition, new Vector3(1, 1, -1));
            Gizmos.DrawCube(node.gridPosition, new Vector3(1, 1, -1));
        }
    }
}
