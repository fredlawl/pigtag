using BehaviorTree;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    class FollowPathToTarget : BehaviorTree.Node
    {
        private float movementSpeed;
        private Rigidbody2D rigidbody;
        private Transform transform;

        private float numberOfAttacksPerSecond = 1f;
        private float attackCounter = 0f;

        public FollowPathToTarget(float movementSpeed, Rigidbody2D rigidbody, Transform transform)
        {
            this.movementSpeed = movementSpeed;
            this.rigidbody = rigidbody;
            this.transform = transform;
        }

        public override BehaviorTree.Node.State Evaluate()
        {
            Queue<Pathing.Node> path = (Queue<Pathing.Node>)GetData("path_to_player");
            //if (path.Count > 0)

            attackCounter += Time.deltaTime;
             //&& attackCounter >= numberOfAttacksPerSecond
            //while (path.Count > 0 && attackCounter >= numberOfAttacksPerSecond)
            if (path.Count > 0)
            {
                Pathing.Node next = path.Dequeue();
                //var movement = Vector2.MoveTowards(transform.position, next.mapWorldPosition, movementSpeed);
                var movement = next.mapWorldPosition;
                rigidbody.MovePosition(movement);
                attackCounter = 0f;
            }

            return State.Running;
        }
    }
}
