using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

namespace Pathing
{
    [RequireComponent(typeof(Tilemap))]
    public class GameGrid : MonoBehaviour
    {
        private Tilemap tilemap;

        public List<Node> Grid { get; private set; }

        public Bounds Bounds => tilemap.localBounds;

        /**
         * Do not have this enabled while drawing
         * on the tilemap. Exception out of bound
         * errors go wild! Also, do not forget to
         * compress the tile map!
         */
        [SerializeField]
        private bool enableDebugging = false;

        private void Start()
        {
            tilemap = GetComponent<Tilemap>();
            Grid = new List<Node>(tilemap.size.x * tilemap.size.y);

            // Populate grid
            for (int y = 0; y < tilemap.size.y; y++)
            {
                for (int x = 0; x < tilemap.size.x; x++)
                {
                    Node n = new Node(new Vector3()
                    {
                        x = x + (tilemap.localBounds.center.x - tilemap.size.x * tilemap.tileAnchor.x) + (tilemap.cellSize.x / 2),
                        y = y + (tilemap.localBounds.center.y - tilemap.size.y * tilemap.tileAnchor.y) + (tilemap.cellSize.y / 2),
                    }, 0, 0);

                    Grid.Add(n);
                }
            }
        }

        private void Update()
        {
            LocateObstructables();
        }

        private void LocateObstructables()
        {
            for (int y = 0; y < tilemap.size.y; y++)
            {
                for (int x = 0; x < tilemap.size.x; x++)
                {
                    Node n = Grid[y * tilemap.size.x + x];
                    Collider2D collision = Physics2D.OverlapBox(n.Position, tilemap.cellSize, 45, Layers.Collidables);
                    n.Obstructed = collision != null;
                }
            }
        }

        private void OnValidate()
        {
            Start();
        }

        private void OnDrawGizmos()
        {
            if (!enableDebugging)
            {
                return;
            }   

            if (tilemap == null || Grid.Count <= 0)
            {
                return;
            }

            LocateObstructables();

            for (int y = 0; y < tilemap.size.y; y++)
            {
                for (int x = 0; x < tilemap.size.x; x++)
                {
                    Node n = Grid[y * tilemap.size.x + x];
                    Gizmos.DrawWireCube(n.Position, tilemap.cellSize);

                    if (n.Obstructed)
                    {
                        Gizmos.DrawCube(n.Position, tilemap.cellSize);
                    }
                }
            }
        }
    }
}
