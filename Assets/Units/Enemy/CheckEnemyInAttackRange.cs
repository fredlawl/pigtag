using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class CheckEnemyInAttackRange : BehaviorTree.Node
    {
        private Transform transform;
        private float attackRange;

        public CheckEnemyInAttackRange(float attackRange, Transform transform)
        {
            this.transform = transform;
            this.attackRange = attackRange;
        }

        public override State Evaluate()
        {
            object target = GetData("target");
            if (target == null)
            {
                return State.Failure;
            }

            /*
             * TODO: Fix the distance calculation
             * The distance is from center of object to center of other object.
             * The problem is the enemy and house are both too big to hit
             * eachother because the distance doesn't take hitbox into
             * account :|
             * 
             * Or adjust attack range?
             */

            /*
             * MissingReferenceException: The object of type 'Transform' has been destroyed but you are still trying to access it.
                Your script should either check if it is null or you should not destroy the object.
                UnityEngine.Transform.get_position () (at <86acb61e0d2b4b36bc20af11093be9a5>:0)
                Enemy.CheckEnemyInAttackRange.Evaluate () (at Assets/Units/Enemy/CheckEnemyInAttackRange.cs:35)
                BehaviorTree.Sequence.Evaluate () (at Assets/BehaviorTree/Sequence.cs:17)
                BehaviorTree.Selector.Evaluate () (at Assets/BehaviorTree/Selector.cs:16)
                BehaviorTree.Tree.Update () (at Assets/BehaviorTree/Tree.cs:19)
             * 
             */
            float distanceToTarget = Vector2.Distance(transform.position, ((Transform)target).position);
            if (distanceToTarget <= attackRange)
            {
                return State.Success;
            }

            return State.Failure;
        }
    }
}