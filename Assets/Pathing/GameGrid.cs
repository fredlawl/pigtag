using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

namespace Pathing
{
    public class GameGrid : IPathfinder
    {
        private Tilemap tilemap;
        public Vector2Int size => new Vector2Int(tilemap.size.x, tilemap.size.y);
        private HashSet<Node> obstacles = new HashSet<Node>();

        public event EventHandler obstacleAddedEvent;

        public bool hasObstacles => obstaclesCount > 0;
        public int obstaclesCount => obstacles.Count;
        public Vector3 cellSize => tilemap.cellSize;

        private GameGrid() { }

        public GameGrid(Tilemap map)
        {
            tilemap = map;
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
            float x = position.x - (tilemap.localBounds.center.x - tilemap.size.x * tilemap.tileAnchor.x) + (cellSize.x * tilemap.tileAnchor.x) - tilemap.tileAnchor.x;
            float y = position.y - (tilemap.localBounds.center.y - tilemap.size.y * tilemap.tileAnchor.y) + (cellSize.y * tilemap.tileAnchor.y) - tilemap.tileAnchor.y;
            
            x = Mathf.Clamp(x, 0, tilemap.size.x - 1);
            y = Mathf.Clamp(y, 0, tilemap.size.y - 1);

            return new Vector3(x, y);
        }

        /*
         * Convient way to add grid obstacales automatically
         */
        public void HydrateObstacles()
        {
            for (int y = 0; y < tilemap.size.y; y++)
            {
                for (int x = 0; x < tilemap.size.x; x++)
                {
                    Node n = Cell(x, y);
                    Collider2D collision = Physics2D.OverlapBox(n.worldPosition, cellSize / 2, 0, Layers.Collidables);
                    if (collision != null)
                    {
                        AddObstacle(n);
                    }
                }
            }
        }

        public void AddObstacle(Vector3 location)
        {
            Node n = Cell(location);
            if (n != null)
            {
                AddObstacle(n);
                obstacleAddedEvent?.Invoke(this, null);
            }
        }

        public void AddObstacle(GameObject obj)
        {
            AddObstacle(GetNodeFromWorldPosition(obj.transform.position).gridPosition);
        }

        public bool IsObstacle(Node node)
        {
            return obstacles.Contains(node);
        }

        public void AddObstacle(Node n)
        {
            obstacles.Add(n);
        }

        public bool RemoveObstacle(Node n)
        {
            return obstacles.Remove(n);
        }

        public IEnumerable<Node> Obstacles()
        {
            return obstacles;
        }

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
            if (position.x < 0 || position.x > tilemap.size.x - 1)
            {
                return null;
            }

            if (position.y < 0 || position.y > tilemap.size.y - 1)
            {
                return null;
            }

            Node n = new Node(new Vector3()
            {
                x = position.x + (tilemap.localBounds.center.x - tilemap.size.x * tilemap.tileAnchor.x) + (cellSize.x * tilemap.tileAnchor.x),
                y = position.y + (tilemap.localBounds.center.y - tilemap.size.y * tilemap.tileAnchor.y) + (cellSize.y * tilemap.tileAnchor.y),
            }, new Vector3(position.x, position.y));

            return n;
        }

        public Queue<Vector3> FindPath(Vector3 from, Vector3 to)
        {
            var start = GetNodeFromWorldPosition(from);
            var end = GetNodeFromWorldPosition(to);
            return FindPathImpl(start, end);
        }

        private Queue<Vector3> FindPathImpl(Node from, Node to)
        {
            var seen = new HashSet<Node>();
            var openSet = new Utils.PriorityQueue<Node, float>();
            var cameFrom = new Dictionary<Node, Node>();
            var gScore = new Dictionary<Node, float>();
            var fScore = new Dictionary<Node, float>();

            openSet.Enqueue(from, 0);
            gScore.Add(from, 0);
            fScore.Add(from, 0);

            while (openSet.Count > 0)
            {
                var current = openSet.Dequeue();
                seen.Add(current);

                if (current.Equals(to))
                {
                    return ReconstrctPath(cameFrom, from, current);
                }

                var neighborPositions = current.Neighbors(this);
                foreach (Node neighbor in neighborPositions)
                {
                    // sometimes the target can be half way into an obstruction, in this case
                    // we can still have a path to them.
                    // TODO: A neighbor can be true for a diamond pattern where a path exists
                    // to the center, so we need to add a collision where if the top left/right
                    // and bottom left/right are blocked, we can't go to that spot
                    // this could make for more interesting paths
                    /*  O
                     * O O
                     *  O 
                     */
                    // On second thought, maybe I don't want to add this
                    // functionality in. There's enough of a gap in sprites
                    // that make it look like anything can pass through 
                    // that gap anyway
                    if (IsObstacle(neighbor) && !neighbor.Equals(to))
                    {
                        continue;
                    }

                    gScore.TryAdd(neighbor, float.PositiveInfinity);
                    fScore.TryAdd(neighbor, float.PositiveInfinity);

                    var tenativeGScore = gScore[current] + CalculateDistance(current.gridPosition, neighbor.gridPosition);
                    if (tenativeGScore < gScore[neighbor])
                    {
                        if (!cameFrom.ContainsKey(neighbor))
                        {
                            cameFrom.Add(neighbor, current);
                        }

                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tenativeGScore;
                        fScore[neighbor] = tenativeGScore + CalculateDistance(neighbor.gridPosition, to.gridPosition);
                        if (!seen.Contains(neighbor))
                        {
                            openSet.Enqueue(neighbor, fScore[neighbor]);
                        }
                    }
                }
            }

            return new Queue<Vector3>();
        }

        private float CalculateDistance(Vector3 from, Vector3 to)
        {
            return Vector3.Distance(from, to);
        }

        private Queue<Vector3> ReconstrctPath(Dictionary<Node, Node> cameFrom, Node start, Node current)
        {
            var path = new List<Vector3>();
            Node node = current;

            path.Add(current.worldPosition);
            while (cameFrom.ContainsKey(node))
            {
                node = cameFrom[node];
                if (!start.Equals(node))
                {
                    path.Add(node.worldPosition);
                }
            }

            path.Reverse();
            return new Queue<Vector3>(path);
        }
    }
}
