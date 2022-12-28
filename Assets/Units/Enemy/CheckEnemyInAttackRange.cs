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

            float distanceToTarget = Vector2.Distance(transform.position, ((Transform)target).position);
            if (distanceToTarget <= attackRange)
            {
                return State.Success;
            }

            return State.Failure;
        }
    }
}