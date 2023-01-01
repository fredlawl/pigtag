using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathing
{
    public class Node : IEqualityComparer<Node>, IEqualityComparer
    {
        public float gScore { get; set; }
        public float hScore { get; set; }

        public float fScore => gScore + hScore;

        public bool isObstructed { get; set; }
        public Vector3 mapWorldPosition { get; private set; }
        public Vector3 gridPosition { get; private set; }

        public Node parent { get; set; }

        private Node() { }

        public Node(Vector3 worldPosition, Vector3 gridPosition)
        {
            this.mapWorldPosition = worldPosition;
            this.gridPosition = gridPosition;
            gScore = float.PositiveInfinity;
            hScore = 0;
        }

        public List<Node> Neighbors(GameGrid grid)
        {
            var positions = new List<Vector3>() {
                //new Vector3(gridPosition.x - 1, gridPosition.y - 1), // top left
                new Vector3(gridPosition.x, gridPosition.y - 1), // top
                //new Vector3(gridPosition.x + 1, gridPosition.y - 1), // top right
                new Vector3(gridPosition.x + 1, gridPosition.y), // right
                //new Vector3(gridPosition.x + 1, gridPosition.y + 1), // bottom right
                new Vector3(gridPosition.x, gridPosition.y + 1), // bottom
                //new Vector3(gridPosition.x - 1, gridPosition.y + 1), // bottom left
                new Vector3(gridPosition.x - 1, gridPosition.y), // left
            };

            var neighbors = new List<Node>();

            foreach (Vector3 pos in positions)
            {
                Node n = grid.Cell(pos);
                if (n != null)
                {
                    neighbors.Add(n);
                }
            }

            return neighbors;
        }

        public bool Equals(Node x, Node y)
        {
            return x.gridPosition.Equals(y);
        }

        public int GetHashCode(Node obj)
        {
            return obj.gridPosition.GetHashCode();
        }

        public new bool Equals(object x, object y)
        {
            return Equals(x, y);
        }

        public int GetHashCode(object obj)
        {
            return GetHashCode(obj);
        }

        public override string ToString()
        {
            return $"gridPosition: {gridPosition}\nmapWorldPosition: {mapWorldPosition}";
        }
    }
}
