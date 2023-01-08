using BehaviorTree;
using Pathing;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    class FindPathToPlayer : BehaviorTree.Node
    {
        private Transform transform;
        private IPathfinder pathfinder;

        public FindPathToPlayer(IPathfinder pathfinder, Transform transform)
        {
            this.transform = transform;
            this.pathfinder = pathfinder;
        }

        public override BehaviorTree.Node.State Evaluate()
        {
            Queue<Vector3> path = (Queue<Vector3>) GetData("path_to_player");
            if (path != null && path.Count > 0)
            {
                return State.Running;
            }

            /*
             * Consider using Layers.Player here, but keep 
             * in mind that we want to target the player specifically
             * not just the nearest object on the player layer
             */
            GameObject player = GameObject.Find("Player");
            if (player == null)
            {
                return State.Failure;
            }

            var foundPath = pathfinder.FindPath(transform.position, player.transform.position);
            if (foundPath.Count == 0)
            {
                return State.Failure;
            }

            parent.SetData("path_to_player", foundPath);
            GetRootNode().SetData("target", player.transform);
            return State.Success;
        }
    }
}
