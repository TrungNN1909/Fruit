using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class Jump : MonoBehaviour
    {
        public Rigidbody2D rigidbody2D;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                Debug.Log("huhu");
                rigidbody2D.AddForce(Vector3.up*1000f);
            }
        }
    }
}
