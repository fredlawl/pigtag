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
        
        private float timer;
        private Vector3 currentPositionHolder;
        private Vector3 startPosition;

        void Start()
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
            pathable = gameObject.GetComponent<Pathable>();
            startPosition = transform.position;
            currentPositionHolder = startPosition;
        }

        private void Update()
        {
            timer += Time.deltaTime * speed;
            if (transform.position != currentPositionHolder)
            {
                transform.position = Vector3.Lerp(startPosition, currentPositionHolder, timer);
            } 
            else 
            {
                if (path.Count > 0)
                {
                    currentPositionHolder = path.Dequeue();
                }

                startPosition = transform.position;
                timer = 0;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (PointerEventData.InputButton.Right == eventData.button)
            {
                newPosition = eventData.pointerCurrentRaycast.worldPosition;
                path = pathable.pathfinder.FindPath(transform.position, newPosition);
            }
        }
    }
}
