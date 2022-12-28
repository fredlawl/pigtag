using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class DashToTarget : BehaviorTree.Node
    {
        private Transform transform;
        private Rigidbody2D rigidBody;
        private float speed;

        public DashToTarget(float speed, Rigidbody2D rigidBody, Transform transform)
        {
            this.rigidBody = rigidBody;
            this.transform = transform;
            this.speed = speed;
        }

        public override State Evaluate()
        {
            Transform target = (Transform)GetData("target");
            float distanceToTarget = Vector2.Distance(transform.position, target.position);

            if (distanceToTarget > 0.01f)
            {
                var movement = Vector2.MoveTowards(transform.position, target.position, Time.deltaTime * speed);
                rigidBody.MovePosition(movement);
                //transform.LookAt(target.position);
            }

            return State.Running;
        }
    }
}
