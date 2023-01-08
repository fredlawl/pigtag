using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCamera
{
    public class FreeMove : MonoBehaviour
    {
        private Vector2 input;
        public float speed = 8;
        public Transform startAt;

        void Start()
        {
            transform.position = new Vector3(startAt.position.x, startAt.position.y, transform.position.z);
        }

        private void Update()
        {
            input = new Vector2()
            {
                x = Input.GetAxis("Horizontal"),
                y = Input.GetAxis("Vertical")
            };
        }

        void FixedUpdate()
        {
            transform.position = new Vector3()
            {
                x = transform.position.x + (input.x * Time.deltaTime * speed),
                y = transform.position.y + (input.y * Time.deltaTime * speed),
                z = transform.position.z
            };
        }
    }
}
