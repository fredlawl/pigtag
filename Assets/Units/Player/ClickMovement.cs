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

        public Animator animator;
        private float timer;
        private Vector3 nextPosition;
        private Vector3 startPosition;
        private SpriteRenderer sr;

        void Start()
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
            pathable = gameObject.GetComponent<Pathable>();
            animator = gameObject.GetComponent<Animator>();
            sr = gameObject.GetComponent<SpriteRenderer>();
            startPosition = transform.position;
            nextPosition = startPosition;
        }

        private void Update()
        {
            timer += Time.deltaTime * speed;
            if (transform.position != nextPosition)
            {
                transform.position = Vector3.Lerp(startPosition, nextPosition, timer);
            } 
            else 
            {
                if (path.Count > 0)
                {
                    nextPosition = path.Dequeue();
                    animator.Play("PlayerRunning");
                } 
                else
                {
                    animator.Play("PlayerAnimation");
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
                if (path.Count > 0)
                {
                    // Not very portable because it depends on start position but w/e
                    sr.flipX = newPosition.x > transform.position.x;
                }
            }
        }
    }
}
