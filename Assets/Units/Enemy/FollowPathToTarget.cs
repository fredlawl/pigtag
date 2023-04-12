using BehaviorTree;
using System;
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
        private Vector3 nextPosition;
        private Vector3 startPosition;
        private Vector3 playerPosition;
        private Queue<Vector3> foundPath = new Queue<Vector3>();
        private Pathing.IPathfinder pathfinder;
        private Animator animator;
        private SpriteRenderer sr;

        public FollowPathToTarget(Pathing.IPathfinder pathfinder, float movementSpeed, Rigidbody2D rigidbody, Transform transform)
        {
            this.movementSpeed = movementSpeed;
            this.rigidbody = rigidbody;
            this.transform = transform;
            this.pathfinder = pathfinder;

            startPosition = transform.position;
            nextPosition = startPosition;
            animator = transform.gameObject.GetComponent<Animator>();
            sr = transform.gameObject.GetComponent<SpriteRenderer>();
            pathfinder.obstacleAddedEvent += RecalculatePath;
        }

        public void RecalculatePath(object sender, EventArgs args)
        {
            Transform targetPosition = (Transform)GetData("target");
            if (targetPosition == null)
            {
                return;
            }

            // Todo: Race here? it works OK
            playerPosition = targetPosition.position;
            foundPath = pathfinder.FindPath(transform.position, playerPosition);
        }

        public override BehaviorTree.Node.State Evaluate()
        {
            Transform targetPosition = (Transform) GetData("target");
            if (targetPosition == null)
            {
                return State.Failure;
            }

            if (playerPosition != targetPosition.position)
            {
                playerPosition = targetPosition.position;
                foundPath = pathfinder.FindPath(transform.position, playerPosition);
                if (foundPath.Count == 0)
                {
                    animator.Play("Stationary");
                    return State.Failure;
                }
                else
                {
                    // Not very portable because it depends on start position but w/e
                    sr.flipX = transform.position.x < targetPosition.position.x;
                }
            }

            timer += Time.deltaTime * movementSpeed;
            if (transform.position != nextPosition)
            {
                transform.position = Vector3.Lerp(startPosition, nextPosition, timer);
            }
            else
            {
                if (foundPath.Count > 0)
                {
                    nextPosition = foundPath.Dequeue();
                    animator.Play("Running");
                }
                else 
                {
                    animator.Play("Stationary");
                }

                startPosition = transform.position;
                timer = 0;
            }

            return State.Running;
        }
    }
}
