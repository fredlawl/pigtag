using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableBoundary : MonoBehaviour
{
    public Bounds boundary;
    private BoxCollider2D boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (boxCollider == null)
        {
            return;
        }

        /*
         * TODO: Clamp/detect on objects bounding box. currently its to the objects
         * center point which is the transformat.position.x/y
         */
        if (transform.position.x <= boundary.min.x || transform.position.y <= boundary.min.y)
        {
            transform.position = new Vector3()
            {
                x = Mathf.Clamp(transform.position.x, boundary.min.x, transform.position.x),
                y = Mathf.Clamp(transform.position.y, boundary.min.y, transform.position.y),
                z = 0
            };
        }

        if (transform.position.x >= boundary.max.x || transform.position.y >= boundary.max.y)
        {
            transform.position = new Vector3()
            {
                x = Mathf.Clamp(transform.position.x, transform.position.x, boundary.max.x),
                y = Mathf.Clamp(transform.position.y, transform.position.y, boundary.max.y),
                z = 0
            };
        }
    }
}
