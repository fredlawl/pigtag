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

        private float timer;
        private Vector3 currentPositionHolder;
        private Vector3 startPosition;
        private Vector3 playerPosition;
        private Queue<Vector3> foundPath = new Queue<Vector3>();
        private Pathing.IPathfinder pathfinder;

        public FollowPathToTarget(Pathing.IPathfinder pathfinder, float movementSpeed, Rigidbody2D rigidbody, Transform transform)
        {
            this.movementSpeed = movementSpeed;
            this.rigidbody = rigidbody;
            this.transform = transform;
            this.pathfinder = pathfinder;

            startPosition = transform.position;
            currentPositionHolder = startPosition;
        }

        public override BehaviorTree.Node.State Evaluate()
        {
            Transform targetPosition = (Transform) GetData("target");
            if (targetPosition == null)
            {
                return State.Failure;
            }

            // Find a path if the player has moved
            if (playerPosition != targetPosition.position)
            {
                playerPosition = targetPosition.position;
                foundPath = pathfinder.FindPath(transform.position, playerPosition);
                if (foundPath.Count == 0)
                {
                    return State.Failure;
                }
            }

            timer += Time.deltaTime * movementSpeed;
            if (transform.position != currentPositionHolder)
            {
                transform.position = Vector3.Lerp(startPosition, currentPositionHolder, timer);
            }
            else
            {
                if (foundPath.Count > 0)
                {
                    currentPositionHolder = foundPath.Dequeue();
                }

                startPosition = transform.position;
                timer = 0;
            }

            return State.Running;
        }
    }
}
