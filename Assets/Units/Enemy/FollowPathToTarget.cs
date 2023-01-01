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

        private void DrawRect(Vector3 min, Vector3 max, Color color)
        {
            UnityEngine.Debug.DrawLine(min, new Vector3(min.x, max.y), color);
            UnityEngine.Debug.DrawLine(new Vector3(min.x, max.y), max, color);
            UnityEngine.Debug.DrawLine(max, new Vector3(max.x, min.y), color);
            UnityEngine.Debug.DrawLine(min, new Vector3(max.x, min.y), color);
        }

        public override BehaviorTree.Node.State Evaluate()
        {
            Queue<Pathing.Node> path = (Queue<Pathing.Node>)GetData("path_to_player");
            //if (path.Count > 0)

            // Need to debug wtf is going on with the pathing just "stopping" at some point
            foreach (Pathing.Node node in path)
            {
                DrawRect(node.mapWorldPosition, node.mapWorldPosition * 2, Color.black);
            }

            attackCounter += Time.deltaTime * movementSpeed;
             //&& attackCounter >= numberOfAttacksPerSecond
            //while (path.Count > 0 && attackCounter >= numberOfAttacksPerSecond)
            if (path.Count > 0 && attackCounter >= 1)
            {
                Pathing.Node next = path.Dequeue();
                //var movement = Vector2.MoveTowards(transform.position, next.mapWorldPosition, attackCounter);
                //var movement = next.mapWorldPosition;
                var movement = Vector3.Lerp(transform.position, next.mapWorldPosition, attackCounter);
                //rigidbody.MovePosition(movement);
                transform.position = movement;
                attackCounter = 0f;
            }

            return State.Running;
        }
    }
}
