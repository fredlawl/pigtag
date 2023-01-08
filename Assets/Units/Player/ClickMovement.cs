using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Player
{
    public class ClickMovement : MonoBehaviour
    {
        public float speed = 8.0f;
        private Rigidbody2D rb;
        private Pathable pathable;
        private Vector3 newPosition;
        private Queue<Vector3> path = new Queue<Vector3>();

        void Start()
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
            pathable = gameObject.GetComponent<Pathable>();
        }

        private void Update()
        {

        }

        void FixedUpdate()
        {
            /*
             * TODO: Player character should actually move when clicking on the
             * ground, and then the sprite moves in that direction...
             * Pathing is going to be a big deal...
             */
            Queue<Vector3> path = pathable.pathfinder.FindPath(transform.position, newPosition);
            while (path.Count > 0)
            {
                Vector3 next = path.Dequeue();
                var movement = Vector2.MoveTowards(transform.position, next, Time.deltaTime * speed);
                rb.MovePosition(movement);
                //transform.position = movement;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (PointerEventData.InputButton.Right == eventData.button)
            {
                newPosition = eventData.pointerCurrentRaycast.worldPosition;
            }
        }
    }
}
