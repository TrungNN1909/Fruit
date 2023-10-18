using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class BanBan : MonoBehaviour
    {
        public float HP = 2000f;
        public float atk = 100f;
        private float spd = 7f;

        public float hpIncrease = 200f;
        public float atkIncrease = 10f;

        private Rigidbody2D rb;

        private bool isAttacked;
        private bool isMoving;
        private bool inCheckPoint;
        private bool isAttacking;

        private Vector3 defaultPostion;
        
        private Vector2 rightEdge;
        private GameObject player;

        [SerializeField] public Animator animator;

        private void Awake()
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
            rightEdge = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        }

        private void OnEnable()
        {
            player = GameObject.FindGameObjectWithTag("HitPoint");
            defaultPostion = new Vector3(10f,-2f,0);
            spd = 7;
            isMoving = true;
            inCheckPoint = false;
            isAttacking = false;
            isAttacked = false;
        }

        public void Init()
        {
            HP = HP + hpIncrease* PlayerPrefs.GetInt("Stage");
            atk = atk + atkIncrease * PlayerPrefs.GetInt("Stage");
        }

        private void OnDisable()
        {
            ResetInfomation();
        }

        private void ResetInfomation()
        {
            HP = 2000f;
            atk = 100f;
            transform.position = new Vector3(25, -2, 0);
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
                Move(new Vector3 (player.transform.position.x, transform.position.y, transform.position.z));
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
                    gameObject.SetActive(false);
                }
            }

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
            animator.SetBool("attacking", true);
            rb.AddForce(new Vector2(-5f, 0));

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
            transform.position = Vector3.MoveTowards(transform.position, defaultPostion, spd * 1f * Time.deltaTime);

        }

        private bool CheckInDefaultPosition()
        {
            return Mathf.Abs(transform.position.x - defaultPostion.x) <= 0.05f;
        }

        private IEnumerator WaitToMoveBack()
        {

            yield return new WaitForSeconds(1.333f);
            animator.SetBool("attacking", false);
            defaultPostion = new Vector3(6.5f, -2f, 0);
            spd = 4f;
            isAttacked = true;
        }
        private IEnumerator WaitToAttack()
        {
            float timeToWait = Random.Range(2f, 6f);
            yield return new WaitForSeconds(timeToWait);
            isAttacking = true;
        }
    }
}
