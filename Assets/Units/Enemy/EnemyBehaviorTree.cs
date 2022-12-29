using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyBehaviorTree : BehaviorTree.Tree
    {
        public float movementSpeed = 10f;
        public AttackStats attackStats;

        protected override Node SetupTree()
        {
            return new Selector(new List<Node>() {
                new Sequence(new List<Node>()
                {
                    new CheckEnemyInAttackRange(attackStats.AttackRange, transform),
                    new EnemyAttack(attackStats)
                }),
                new Sequence(new List<Node>()
                {
                    new FindPlayerFromCurrentPosition(transform),
                    new DashToTarget(movementSpeed, GetComponent<Rigidbody2D>(), transform)
                }),
                new DummyPatrol()
            });
        }
    }
}
