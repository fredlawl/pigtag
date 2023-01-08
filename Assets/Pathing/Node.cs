using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathing
{
    public class Node : IEquatable<Node>
    {
        public Vector3 worldPosition { get; private set; }
        public Vector3 gridPosition { get; private set; }

        private Node() { }

        public Node(Vector3 worldPosition, Vector3 gridPosition)
        {
            this.worldPosition = worldPosition;
            this.gridPosition = gridPosition;
        }

        public List<Node> Neighbors(GameGrid grid)
        {
            var positions = new List<Vector3>() {
                new Vector3(gridPosition.x - 1, gridPosition.y - 1), // top left
                new Vector3(gridPosition.x, gridPosition.y - 1), // top
                new Vector3(gridPosition.x + 1, gridPosition.y - 1), // top right
                new Vector3(gridPosition.x + 1, gridPosition.y), // right
                new Vector3(gridPosition.x + 1, gridPosition.y + 1), // bottom right
                new Vector3(gridPosition.x, gridPosition.y + 1), // bottom
                new Vector3(gridPosition.x - 1, gridPosition.y + 1), // bottom left
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


        public override string ToString()
        {
            return $"gridPosition: {gridPosition}\nmapWorldPosition: {worldPosition}";
        }

        public bool Equals(Node other)
        {
            return other != null && gridPosition.Equals(other.gridPosition);
        }

        public override int GetHashCode()
        {
            return gridPosition.GetHashCode();
        }
    }
}
