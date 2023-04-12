using BehaviorTree;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyBehavior : BehaviorTree.Tree
    {
        public float movementSpeed = 10f;
        public AttackStats attackStats;

        protected override void Start()
        {
        }

        protected void OnEnable()
        {
            root = SetupTree();
            //player = GameObject.Find("Player");
            //pather = GetComponent<Pathing.Pather>();
        }

        protected override Node SetupTree()
        {
            var pathable = GetComponent<Pathable>();
            if (pathable == null || pathable.pathfinder == null)
            {
                return null;
            }

            return new Selector(new List<Node>() {
                new Sequence(new List<Node>()
                {
                    new CheckEnemyInAttackRange(attackStats.AttackRange, transform),
                    new AttackTarget(attackStats)
                }),
                new Sequence(new List<Node>()
                {
                    new LocatePlayer(),
                    new FollowPathToTarget(pathable.pathfinder, movementSpeed, GetComponent<Rigidbody2D>(), transform)
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
             * UPDATE: this may be fixed, but it's currently untested
             */
            Debug.Log($"{gameObject.name} damaged for {amount}!");
        }
    }
}
