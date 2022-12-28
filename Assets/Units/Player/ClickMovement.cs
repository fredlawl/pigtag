using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class ClickMovement : MonoBehaviour
    {
        public float speed = 8.0f;
        private Rigidbody2D rb;
        private Vector2 nextLocation;

        void Start()
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
            Debug.Log("test");
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

            //rb.MovePosition(new Vector2() { 
            //    x = transform.position.x + (input.x * Time.deltaTime * speed),
            //    y = transform.position.y + (input.y * Time.deltaTime * speed)
            //});
        }
    }
}
