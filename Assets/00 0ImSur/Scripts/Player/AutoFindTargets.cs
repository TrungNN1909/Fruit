using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class AutoFindTargets : MonoBehaviour
    {
        private float range;

        GameObject target;
        private Vector3 rightEdge;
        // Update is called once per frame
        private void Start()
        {
            rightEdge = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, transform.position.y, 10)) ;
        }

        // void Update()
        // {
        //     range = Vector3.Distance(transform.position, rightEdge) -1f;
        //     SetTarget();
        // }

        public void OnUpdate()
        {
            range = Vector3.Distance(transform.position, rightEdge) -2f;
            SetTarget();
        }

        private void SetTarget()
        {
            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, range);

            if (colliderArray.Length == 0)
            {
                target = null;
                return;
            }

            foreach (Collider2D collider in colliderArray)
            {
                if (collider.gameObject.CompareTag("Enemy") || collider.gameObject.CompareTag("EnemyBullet"))
                {
                    target = collider.gameObject;
                }
            }
        }

        public GameObject GetTarget()
        {
            return target;
        }
    }
}
 