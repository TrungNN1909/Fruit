using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{

    public class EnemyBullet : MonoBehaviour
    {
        private GameObject player;
        public float HP = 10;
        public float atk;
        private float speed = 5f;

        private float hpIncreasing = 5;
        private float spdIncrease = 0.2f;


        private void OnEnable()
        {
            
            player = GameObject.FindGameObjectWithTag("HitPoint");
            speed = Mathf.Clamp(speed + 0.1f * PlayerDataManager.Instance.GetStage() , 5 , 10f);

        }

        private void OnDisable()
        {
            transform.rotation = Quaternion.identity;
        }
        private void Update()
        {
            Shoot();  
        }

        public void Shoot()
        {
            
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("HitPoint"))
            {
                // gameObject.SetActive(false);
                SimplePool.Despawn(gameObject);
            }
            if (collision.gameObject.CompareTag("GunBullet"))
            {
                HP -= collision.gameObject.GetComponent<Bullet>().dame;
                if (HP <= 0)
                {
                    SimplePool.Despawn(gameObject);
                }
            }
        }
    }

}