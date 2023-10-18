using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace Unicorn
{
    public class Snail : BaseEnemy
    {
        [SerializeField] private GameObject bulletPrefab;
        private float fireRate = 0.25f;
        private float timeToShoot = 0f;

        public override void WaitToAttackUpdate()
        {
            base.WaitToAttackUpdate();
            Move(defaultPosition);

            if (isReachDefaultPosition)
            {
                stateMachine.ChangeState(attackState);
            }
        }

        public override void AttackUpdate()
        {
            base.AttackUpdate();
            Attack();
        }

        protected override void Move(Vector3 destinaton)
        {
            base.Move(destinaton);
            transform.position = Vector3.MoveTowards(transform.position, destinaton, spd * Time.deltaTime);
        }

        protected override void Attack()
        {
            base.Attack();
            if (isAttacking)
            {
                animator.SetTrigger("Attack");
                if (timeToShoot >= fireRate)
                {
                    Shoot();
                    timeToShoot = 0;
                    stateMachine.ChangeState(waitToAttackState);
                }

                timeToShoot += Time.deltaTime;
            }
        }
        private void Shoot()
        {
            GameObject bullet = SimplePool.Spawn(bulletPrefab);
            bullet.SetActive(true);
            Vector3 dir = transform.position - hitPoint.transform.position ;
            bullet.GetComponent<EnemyBullet>().atk = atk;
            bullet.GetComponent<EnemyBullet>().HP = hp * 0.1f;
            bullet.transform.Rotate(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
            bullet.transform.position = transform.position;
        }
        
        public override void SetUp()
        {
            base.SetUp();
            defaultPosition = new Vector3(Random.Range(10f, 16.5f), Random.Range(-0.5f, 0.5f), 0);
            spd = Mathf.Clamp(baseSpeed + 0.1f * PlayerDataManager.Instance.GetStage() , baseSpeed , 10f);
        }


        public override void DeadAction()
        {
            base.DeadAction();
            transform.DOMoveX(35f, 3f).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
        
        
    }
}
