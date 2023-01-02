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

        /*
         * Strictly debugging:
         */
        //private Pathing.Pather pather;
        //private GameObject player;
        //private Queue<Pathing.Node> path = new Queue<Pathing.Node>();

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
            var pather = GetComponent<Pathing.Pather>();
            /*
             * We need to check for null here because the spawner
             * will automatically instantiate and call this, but
             * we don't know the pather until after we manually
             * set it and then reactivate the object. What
             * a flipping pain...
             */
            if (pather == null || pather.pathfinder == null)
            {
                return null;
            }


            return new Selector(new List<Node>() {
                new Sequence(new List<Node>()
                {
                    new CheckEnemyInAttackRange(attackStats.AttackRange, transform),
                    new EnemyAttack(attackStats)
                }),
                //new Sequence(new List<Node>()
                //{
                //    new FindPlayerFromCurrentPosition(transform),
                //    new DashToTarget(movementSpeed, GetComponent<Rigidbody2D>(), transform),
                //}),
                new Sequence(new List<Node>()
                {
                    new FindPathToPlayer(pather.pathfinder, transform),
                    new FollowPathToTarget(movementSpeed, GetComponent<Rigidbody2D>(), transform)
                }),
                new DummyPatrol()
            });
        }

        //private void FixedUpdate()
        //{
        //    if (pather == null || pather.pathfinder == null || player == null)
        //    {
        //        return;
        //    }

        //    Pathing.Pathfinder pathfinder = pather.pathfinder;
        //    path = pathfinder.FindPath(pathfinder.GetNode(transform.position), pathfinder.GetNode(player.transform.position));
        //}

        //private void LateUpdate()
        //{
        //    if (path.Count > 0)
        //    {
        //        Pathing.Node next = path.Dequeue();
        //        var movement = Vector2.MoveTowards(transform.position, next.mapWorldPosition, Time.deltaTime * movementSpeed);
        //        //var movement = next.mapWorldPosition;
        //        //var movement = Vector3.Lerp(transform.position, next.mapWorldPosition, Time.deltaTime * movementSpeed);
        //        //rigidbody.MovePosition(movement);
        //        transform.position = movement;
        //        //rigidbody.MovePosition(movement);
        //    }
        //}

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

        public static explicit operator EnemyBehavior(GameObject v)
        {
            throw new NotImplementedException();
        }
    }
}
