using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMovement : MonoBehaviour
{
    public float speed = 8.0f;
    private Rigidbody2D rb;
    private Vector2 input;
    
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
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
        rb.MovePosition(new Vector2()
        {
            x = transform.position.x + (input.x * Time.deltaTime * speed),
            y = transform.position.y + (input.y * Time.deltaTime * speed)
        });
    }
}
