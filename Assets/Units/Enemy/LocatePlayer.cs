using BehaviorTree;
using Pathing;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    class LocatePlayer : BehaviorTree.Node
    {
        private GameObject playerObj;

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
             * Memoize playerObj
             */
            if (playerObj == null)
            {
                playerObj = GameObject.Find("Player");
                return State.Failure;
            }

            GetRootNode().SetData("target", playerObj.transform);
            return State.Success;
        }
    }
}
