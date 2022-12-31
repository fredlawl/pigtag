using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathing
{
    public class Pathfinder
    {
        private GameGrid grid;

        public Pathfinder(GameGrid grid)
        {
            this.grid = grid;
        }

        public List<Node> FindPath(Transform from, Transform to, float hCost)
        {
            var open = new HashSet<Transform>();
            open.Add(from);

            var cameFrom = new HashSet<Transform>();
            float gScore = 0;
            return new List<Node>();
        }
    }
}