using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

namespace Pathing
{
    public class GameGrid
    {
        private Tilemap tilemap;
        private List<Node> grid = new List<Node>();
        public Vector2Int size => new Vector2Int(tilemap.size.x, tilemap.size.y);

        private GameGrid() { }

        public GameGrid(Tilemap map)
        {
            tilemap = map;
            grid = new List<Node>(tilemap.size.x * tilemap.size.y);

            // Populate grid
            for (int y = 0; y < tilemap.size.y; y++)
            {
                for (int x = 0; x < tilemap.size.x; x++)
                {
                    Node n = new Node(new Vector3()
                    {
                        x = x + (tilemap.localBounds.center.x - tilemap.size.x * tilemap.tileAnchor.x) + (tilemap.cellSize.x * tilemap.tileAnchor.x),
                        y = y + (tilemap.localBounds.center.y - tilemap.size.y * tilemap.tileAnchor.y) + (tilemap.cellSize.y * tilemap.tileAnchor.y),
                    }, new Vector3(x, y));

                    grid.Add(n);
                }
            }
        }

        public Node GetNodeFromWorldPosition(Vector3 position)
        {
            var pos = MapPositionToGridCell(position);
            return Cell(pos);
        }

        public Node GetNodeFromGridPosition(Vector3 position)
        {
            return Cell(position);
        }

        private Vector3 MapPositionToGridCell(Vector3 position)
        {
            return new Vector3()
            {
                x = position.x - (tilemap.localBounds.center.x - tilemap.size.x * tilemap.tileAnchor.x) + (tilemap.cellSize.x * tilemap.tileAnchor.x) - tilemap.tileAnchor.x,
                y = position.y - (tilemap.localBounds.center.y - tilemap.size.y * tilemap.tileAnchor.y) + (tilemap.cellSize.y * tilemap.tileAnchor.y) - tilemap.tileAnchor.y,
            };
        }

        public void MarkObstructables()
        {
            for (int y = 0; y < tilemap.size.y; y++)
            {
                for (int x = 0; x < tilemap.size.x; x++)
                {
                    Node n = Cell(x, y);
                    AddObstruction(n);
                }
            }
        }

        public void AddObstruction(Vector3 location)
        {
            Node n = Cell(location);
            if (n != null)
            {
                AddObstruction(n);
            }
        }

        public void AddObstruction(Node n)
        {
            Collider2D collision = Physics2D.OverlapBox(n.mapWorldPosition, tilemap.cellSize / 2, 0, Layers.Collidables);
            n.isObstructed = collision != null;
            //test 
        }

        public Vector3 cellSize => tilemap.cellSize;

        public Node Cell(int x, int y)
        {
            return Cell(new Vector2Int(x, y));
        }

        public Node Cell(Vector3 position)
        {
            return Cell(new Vector2Int((int)position.x, (int)position.y));
        }

        public Node Cell(Vector3Int position)
        {
            return Cell(new Vector2Int(position.x, position.y));
        }

        public Node Cell(Vector2Int position)
        {
            var index = position.y * tilemap.size.x + position.x;
            if (index < 0 || index >= grid.Count)
            {
                return null;
            }

            return grid[index];
        }
    }
}
