using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class SubBullet : MonoBehaviour
    {
        [SerializeField] Rigidbody2D rb;


        public void SetSpeed(float spd, Vector3 dir)
        {
            rb.velocity = dir * spd;
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyBullet"))
            {
                gameObject.SetActive(false);
                transform.rotation = Quaternion.identity;
            }
        }

        protected void OnBecameInvisible()
        {
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
        }
    }
}
