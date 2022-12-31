using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DebugTilemap : MonoBehaviour
{
    Tilemap tilemap;

    void OnValidate()
    {
        if (tilemap == null)
            tilemap = GetComponent<Tilemap>();
    }

    void OnDrawGizmos()
    {
        Draw();
    }

    void Draw()
    {
        if (tilemap == null)
            return;

        // tilemap position
        var tp = tilemap.transform.position;

        // bounds + offset
        var tBounds = tilemap.localBounds;

        // corner points
        var c0 = new Vector3(tBounds.min.x, tBounds.min.y);
        var c1 = new Vector3(tBounds.min.x, tBounds.max.y);
        var c2 = new Vector3(tBounds.max.x, tBounds.max.y);
        var c3 = new Vector3(tBounds.max.x, tBounds.min.y);

        // draw borders
        Debug.DrawLine(c0, c1, Color.blue);
        Debug.DrawLine(c1, c2, Color.blue);
        Debug.DrawLine(c2, c3, Color.red);
        Debug.DrawLine(c3, c0, Color.red);
    }
}
