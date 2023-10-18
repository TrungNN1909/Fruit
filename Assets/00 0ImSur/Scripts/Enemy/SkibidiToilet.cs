using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{

    public class SkibidiToilet : MonoBehaviour
    {
        public float HP = 100f;
        public float atk = 20f;
        private float spd = 5f;

        public float hpIncreasePerLevel = 35f;
        public float atkIncreasePerLevel = 5f;

        private Rigidbody2D rb;

        private bool isAttacked;
        private bool isMoving;
        private bool inCheckPoint;
        private bool isAttacking;

        private Vector3 defaultPostion;
        private Vector2 rightEdge;
        private GameObject player;

        private Animator animator;

        private void Awake()
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
            animator = gameObject.GetComponent<Animator>();
            rightEdge = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        }

        private void OnEnable()
        {
            player = GameObject.FindGameObjectWithTag("HitPoint");
            isMoving = true;
            inCheckPoint = true;
            defaultPostion = new Vector3(Random.Range(10f, 14f), Random.Range(-8f, -4f), 0);

            Init();
        }

        public void Init()
        {
            HP = HP + hpIncreasePerLevel * PlayerPrefs.GetInt("Stage");
            atk = atk + atkIncreasePerLevel * PlayerPrefs.GetInt("Stage");
        }

        private void OnDisable()
        {
            ResetInfomation();

        }
        private void Update()
        {
            Dead();

            //move to default position
            if (isMoving)
            {
                Move(defaultPostion);

                if (CheckInDefaultPosition())
                {
                    isMoving = false;
                    inCheckPoint = true;
                }

            }

            //wait to attack
            if (inCheckPoint)
            {
                StartCoroutine(nameof(WaitToAttack));
                inCheckPoint = false;
            }

            // move to player
            if (isAttacking)
            {
                Move(player.transform.position);
            }

            if (isAttacked)
            {
                MoveBack();
                if (CheckInDefaultPosition())
                {
                    isAttacked = false;
                    inCheckPoint = true;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("GunBullet"))
            {
                HP -= collision.gameObject.GetComponent<Bullet>().dame;
                if (HP <= 0)
                {
                    //Destroy(gameObject);
                    gameObject.SetActive(false);
                }
            }

        }

        private void ResetInfomation()
        {
            HP = 100f;
            atk = 20f;
            transform.position = new Vector3(25, -5, 0);

        }

        private void Dead()
        {
            if (HP <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void Move(Vector3 destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, spd * Time.deltaTime);
        }

        public void Colided()
        {
            isAttacking = false;
            animator.SetBool("Attack", true);
            rb.velocity = Vector3.zero;
            Debug.Log("Colided");

            StartCoroutine(nameof(WaitToMoveBack));
        }

        private void WaitForAttack()
        {
            if (transform.position.x < rightEdge.x && transform.position.x > 10f)
            {
                inCheckPoint = true;
            }
        }

        private void MoveBack()
        {
            transform.position = Vector3.MoveTowards(transform.position, defaultPostion, spd * 1.5f * Time.deltaTime);

        }

        private bool CheckInDefaultPosition()
        {
            return Mathf.Abs(transform.position.x - defaultPostion.x) <= 0.1f;
        }

        private IEnumerator WaitToMoveBack()
        {

            yield return new WaitForSeconds(0.5f);
            animator.SetBool("Attack", false);

            yield return new WaitForSecondsRealtime(1f);

            rb.velocity = Vector3.zero;
            isAttacked = true;
        }
        private IEnumerator WaitToAttack()
        {
            yield return new WaitForSeconds(3f);

            isAttacking = true;
        }
    }

}
