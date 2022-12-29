using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCamera
{
    public class FreeMove : MonoBehaviour
    {
        private Vector2 input;
        public float speed = 8;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
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
