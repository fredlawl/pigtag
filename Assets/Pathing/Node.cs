using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathing
{
    public class Node
    {
        public float GCost { get; set; }
        public float HCost { get; set; }

        public float FCost => GCost + HCost;

        public bool Obstructed { get; set; }
        public Vector3 Position { get; private set; }

        public Node Parent { get; set; }

        private Node() { }

        public Node(Vector3 position, float gCost, float hCost)
        {
            Position = position;
            GCost = gCost;
            HCost = hCost;
        }

        public Node(Vector3 position, bool obstructed, float gCost, float hCost)
        {
            Position = position;
            Obstructed = obstructed;
            GCost = gCost;
            HCost = hCost;
        }
    }
}
