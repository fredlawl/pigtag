using Pathing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DebugUnitToGridTranslation : MonoBehaviour
{
    private GameGrid grid;
    private Node node;

    private void OnValidate()
    {        
        var tilemap = GameObject.Find("World").GetComponent<Tilemap>();
        grid = new GameGrid(tilemap);
        node = grid.GetNodeFromWorldPosition(transform.position);
    }

    private void OnDrawGizmos()
    {
        if (grid == null || node == null)
        {
            return;
        }

        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position, new Vector3(1, 1));

        Gizmos.color = Color.red;
        Gizmos.DrawCube(node.mapWorldPosition, new Vector3(1, 1));

        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(node.gridPosition, new Vector3(1, 1));
    }
}
