using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{

    public class Bullet : MonoBehaviour
    {
        public float dame;
        public bool crit = false;
        private Rigidbody2D rb;
        [SerializeField] private GameObject exploreFx;

        private void Awake()
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
        }

        public void setSpeed(Vector3 dir, float spd)
        {

            // transform.position += dir * spd * Time.deltaTime;
            rb.velocity = dir * spd;
        }



        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyBullet") ||collision.gameObject.CompareTag("BlueBullet"))
            {
                SimplePool.Spawn(exploreFx,transform.position,Quaternion.identity);
                SimplePool.Despawn(gameObject);
                // gameObject.SetActive(false);
                transform.rotation = Quaternion.identity;
            }
        }

        protected void OnBecameInvisible()
        {
            // gameObject.SetActive(false);
            SimplePool.Despawn(gameObject);
            transform.rotation = Quaternion.identity;
        }


    }





}
