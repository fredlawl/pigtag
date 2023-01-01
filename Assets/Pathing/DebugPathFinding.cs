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
    private Queue<Node> path;

    private void OnValidate()
    {
        if (tilemap == null || target == null)
        {
            return;
        }

        GameGrid grid = new GameGrid(tilemap);
        pather = new Pathfinder(grid);
        grid.MarkObstructables();
        path = pather.FindPath(pather.GetNode(transform.position), pather.GetNode(target.position));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        foreach (Node node in path)
        {
            Gizmos.DrawCube(node.mapWorldPosition, new Vector3(1, 1, -1));
            Gizmos.DrawCube(node.gridPosition, new Vector3(1, 1, -1));
        }
    }
}
