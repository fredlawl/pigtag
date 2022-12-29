using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCamera
{
    public class TrackObject : MonoBehaviour
    {
        public GameObject objectToTrack;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void LateUpdate()
        {
            if (objectToTrack == null)
            {
                return;
            }

            transform.position = new Vector3(objectToTrack.transform.position.x, objectToTrack.transform.position.y, transform.position.z); ;
        }
    }
}
