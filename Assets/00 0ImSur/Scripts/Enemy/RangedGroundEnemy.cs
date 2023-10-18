using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class RangedGroundEnemy : MonoBehaviour
    {
        private Rigidbody2D rb;
        private Animator animator;

        public float HP = 100f;
        public float atk = 25f;
        public float hpIncrease = 25f;
        public float atkIncrease = 8f;
        private float moveSpd = 3f;
        private float fireRate = 0.5f;
        private float timeToAtack = 0f;

        [SerializeField] private GameObject enemyBullet;
        [SerializeField] private Transform bulletSpawnpos;
        private int numberOfBullets;

        private Vector3 defaultInScreenPosition;
        private bool isMovingToDefaultPos;
        private bool isAttacking;

        public Vector3 dir;
        public GameObject hitPoint;

        private List<GameObject> currentBullets;
        private void Awake()
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
            animator = gameObject.GetComponent<Animator>();
        }



        private void OnEnable()
        {
            hitPoint = GameObject.FindGameObjectWithTag("HitPoint");
            defaultInScreenPosition = new Vector3(Random.Range(10f, 14f), Random.Range(-6f, -2f), 0);
            isMovingToDefaultPos = true;
            Init();
        }

        public void Init()
        {
            HP = HP + hpIncrease * (PlayerPrefs.GetInt("Stage") + 1);
            atk = atk + atkIncrease * (PlayerPrefs.GetInt("Stage") + 1);
        }
        private void OnDisable()
        {
            // foreach (GameObject gameObject in currentBullets)
            // {
            //     SimplePool.Despawn(gameObject);
            // }
            ResetInfomation();

        }
        private void Update()
        {
            //Move to spawn position to default position
            if (isMovingToDefaultPos)
            {
                Move(defaultInScreenPosition, moveSpd);
                if (CheckDefaultPosition())
                {
                    isMovingToDefaultPos = false;
                    StartCoroutine(nameof(WaitToAttack));
                }
            }
            //attacking --> spawn 1 spicific number of s
            if (isAttacking)
            {
                Attack();
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

        private void ResetInfomation()
        {
            HP = 100f;
            transform.position = new Vector3(25, -5, 0);

        }

        private void Move(Vector3 destination, float spd)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, spd * Time.deltaTime);
        }

        private bool CheckDefaultPosition()
        {
            return Mathf.Abs(transform.position.x - defaultInScreenPosition.x) <= 0.1f;
        }

        private IEnumerator WaitToAttack()
        {
            yield return new WaitForSeconds(1f);
            isAttacking = true;
        }

        private void Attack()
        {
            numberOfBullets = Random.Range(1, 3);
            int countAttackTimes = 0;

            animator.SetBool("isAttacking", true);

            if (timeToAtack >= fireRate && isAttacking)
            {
                GameObject bullet = SimplePool.Spawn(enemyBullet);
                timeToAtack = 0f;
                // currentBullets.Add(bullet);
                bullet.SetActive(true);
                dir = bulletSpawnpos.position - hitPoint.transform.position ;
                bullet.transform.Rotate(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
                bullet.transform.position = bulletSpawnpos.position;
                bullet.GetComponent<EnemyBullet>().atk = atk;
                if (++countAttackTimes == numberOfBullets)
                {
                    isAttacking = false;
                    animator.SetBool("isAttacking", false);
                    StartCoroutine(nameof(WaitToRepeatAttack));
                    return;
                }
            }
            timeToAtack += Time.deltaTime;
        }
        private IEnumerator WaitToRepeatAttack()
        {
            yield return new WaitForSeconds(3f);

            isMovingToDefaultPos = true;
        }
    }

}

