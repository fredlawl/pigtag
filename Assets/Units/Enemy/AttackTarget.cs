using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class AttackTarget : BehaviorTree.Node
    {
        private Transform lastTarget;
        private float numberOfAttacksPerSecond = 1f;
        private float attackCounter = 0f;
        private Health targetHealth;
        private AttackStats attackStats;

        public AttackTarget(AttackStats attackStats)
        {
            this.attackStats = attackStats;
        }

        public override BehaviorTree.Node.State Evaluate()
        {
            Transform target = (Transform)GetData("target");
            if (target != lastTarget)
            {
                targetHealth = target.gameObject.GetComponent<Health>();
                lastTarget = target;
            }

            attackCounter += Time.deltaTime;
            if (attackCounter >= numberOfAttacksPerSecond)
            {
                Debug.Log($"Target({target.gameObject.name}) being attacked!");
                targetHealth.TakeDamage(attackStats.CalculateDamageDelt(), () =>
                {
                    if (targetHealth.IsDead())
                    {
                        ClearData("target");
                    } else {
                        attackCounter = 0f;
                    }

                    return BehaviorTree.Node.State.Running;
                });
            }

            return BehaviorTree.Node.State.Running;
        }
    }
}