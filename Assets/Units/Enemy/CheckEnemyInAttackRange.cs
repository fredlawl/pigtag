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
            float distanceToTarget = Vector2.Distance(transform.position, ((Transform)target).position);
            if (distanceToTarget <= attackRange)
            {
                return State.Success;
            }

            return State.Failure;
        }
    }
}