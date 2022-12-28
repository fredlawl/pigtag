using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyAttack : BehaviorTree.Node
    {
        private Transform lastTarget;
        private float numberOfAttacksPerSecond = 1f;
        private float attackCounter = 0f;

        public EnemyAttack()
        {

        }

        public override State Evaluate()
        {
            Transform target = (Transform)GetData("target");
            if (target != lastTarget)
            {
                lastTarget = target;
            }

            attackCounter += Time.deltaTime;
            if (attackCounter >= numberOfAttacksPerSecond)
            {
                // todo: lookup target and make it loose health
                attackCounter = 0f;
                Debug.Log("Attacking!");
            }

            return State.Running;
        }
    }
}