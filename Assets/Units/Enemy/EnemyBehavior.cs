using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyBehavior : BehaviorTree.Tree
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
                    new DashToTarget(movementSpeed, GetComponent<Rigidbody2D>(), transform),
                }),
                new DummyPatrol()
            });
        }

        public void OnDied()
        {
            Debug.Log($"{gameObject.name} died!");
            gameObject.SetActive(false);
        }

        public void OnDamaged(float amount)
        {
            /*
             * TODO: There's a bug here
             * For w/e reason, this is given the value
             * passed in via the UI instead of from the 
             * .Invoke() :thinkingface:
             */
            Debug.Log($"{gameObject.name} damaged for {amount}!");
        }
    }
}