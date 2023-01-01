using BehaviorTree;
using Pathing;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    class FindPathToPlayer : BehaviorTree.Node
    {
        private Transform transform;
        private Pathfinder pathfinder;

        public FindPathToPlayer(Pathfinder pathfinder, Transform transform)
        {
            this.transform = transform;
            this.pathfinder = pathfinder;
        }

        public override BehaviorTree.Node.State Evaluate()
        {
            Queue<Pathing.Node> path = (Queue<Pathing.Node>) GetData("path_to_player");
            if (path != null && path.Count > 0)
            {
                return State.Running;
            }

            GameObject player = GameObject.Find("Player");
            if (player == null)
            {
                return State.Failure;
            }

            var foundPath = pathfinder.FindPath(pathfinder.GetNode(transform.position), pathfinder.GetNode(player.transform.position));
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
