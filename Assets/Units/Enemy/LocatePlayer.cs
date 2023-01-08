using BehaviorTree;
using Pathing;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    class LocatePlayer : BehaviorTree.Node
    {
        public LocatePlayer()
        {
        }

        public override BehaviorTree.Node.State Evaluate()
        {
            object target = GetData("target");
            if (target != null)
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

            GetRootNode().SetData("target", player.transform);
            return State.Success;
        }
    }
}
