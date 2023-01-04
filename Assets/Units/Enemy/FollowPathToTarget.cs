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

        public FollowPathToTarget(float movementSpeed, Rigidbody2D rigidbody, Transform transform)
        {
            this.movementSpeed = movementSpeed;
            this.rigidbody = rigidbody;
            this.transform = transform;
        }

        public override BehaviorTree.Node.State Evaluate()
        {
            Queue<Pathing.Node> path = (Queue<Pathing.Node>)GetData("path_to_player");
            if (path.Count > 0)
            {
                Pathing.Node next = path.Dequeue();
                var movement = Vector2.MoveTowards(transform.position, next.mapWorldPosition, Time.deltaTime * movementSpeed);
                rigidbody.MovePosition(movement);
            }

            return State.Running;
        }
    }
}
