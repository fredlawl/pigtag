using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class FindPlayerFromCurrentPosition : BehaviorTree.Node
    {
        private Transform transform;

        public FindPlayerFromCurrentPosition(Transform transform)
        {
            this.transform = transform;
        }

        public override State Evaluate()
        {
            object target = GetData("target");
            if (target == null)
            {
                Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2()
                {
                    x = 600,
                    y = 600
                }, 45);

                if (colliders.Length > 0)
                {
                    GetRootNode().SetData("target", colliders[0].transform);
                    return State.Success;
                }

                return State.Failure;
            }

            return State.Running;
        }
    }
}
