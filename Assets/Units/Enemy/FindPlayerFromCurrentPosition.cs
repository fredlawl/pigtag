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
                }, 45, Layers.PlayerLayerMask);

                if (colliders.Length > 0)
                {
                    Collider2D picked = null;


                    /*
                     * TODO: Add logic to only fetch player
                     * Then we can have a separate behavior tree
                     * for the case the player is unreachable because
                     * player is hiding behind their buildings
                     */
                    foreach (Collider2D collider in colliders)
                    {
                        // TODO: Remove this hack
                        if ("Player".Equals(collider.gameObject.name))
                        {
                            picked = collider;
                            break;
                        }

                        if (!collider.GetComponent<Health>().IsImmune())
                        {
                            picked = collider;
                        }
                    }

                    if (picked == null)
                    {
                        return State.Failure;
                    }

                    GetRootNode().SetData("target", picked.transform);
                    return State.Success;
                }

                return State.Failure;
            }

            return State.Running;
        }
    }
}
