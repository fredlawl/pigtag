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

        public Node GetNode(Vector3 position)
        {
            return grid.GetNodeFromWorldPosition(position);
        }

        /*
         * h is omitted, it's supplied by the node itself.
         */
        public Queue<Node> FindPath(Node from, Node to)
        {
            var seen = new HashSet<Node>();
            var queue = new Utils.PriorityQueue<Node, float>();

            /*
             * Since this gets called often, we need to reset the nodes because
             * we mutate the nodes specifically.
             * 
             * TODO: Make Node immutable or struct or something, 
             * and then adjust algorithms appropriately to handle
             * that.
             */ 
            grid.ResetNodes();

            from.gScore = 0;
            queue.Enqueue(from, from.gScore);

            while (queue.Count > 0) {
                var current = queue.Dequeue();
                seen.Add(current);

                if (current.Equals(to))
                {
                    return ReconstrctPath(current, from, to);
                }

                var neighborPositions = current.Neighbors(grid);
                foreach (Node neighbor in neighborPositions)
                {
                    // sometimes the target can be half way into an obstruction, in this case
                    // we can still have a path to them.
                    if ((neighbor.isObstructed && !neighbor.Equals(to)) || seen.Contains(neighbor))
                    {
                        continue;
                    }

                    var tenativeGScore = current.gScore + CalculateDistance(current.gridPosition, neighbor.gridPosition);
                    if (tenativeGScore < neighbor.gScore)
                    {
                        neighbor.parent = current;
                        neighbor.gScore = tenativeGScore;
                        neighbor.hScore = CalculateDistance(neighbor.gridPosition, to.gridPosition);
                        if (!seen.Contains(neighbor))
                        {
                            queue.Enqueue(neighbor, neighbor.fScore);
                        }
                    }
                }
            }

            return new Queue<Node>();
        }

        private float CalculateDistance(Vector3 from, Vector3 to) 
        {
            // return Vector3.Distance(from, to);
            return Mathf.Abs(from.x - to.x) + Mathf.Abs(from.y - to.y);
        }

        private Queue<Node> ReconstrctPath(Node current, Node from, Node target)
        {
            var path = new List<Node>();
            Node n = target;

            while (!from.Equals(n))
            {
                path.Add(n);
                n = n.parent;
            }

            path.Reverse();
            return new Queue<Node>(path);
        }
    }
}