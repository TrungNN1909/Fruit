using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Unicorn
{
    public class GroundSlime : BaseEnemy
    {
        [SerializeField] private GameObject bulletPrefab;
        private float fireRate = 0.3f;
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

        public override void DeadUpdate()
        {
            base.DeadUpdate();
        }

        protected override void Move(Vector3 destinaton)
        {
            base.Move(destinaton);
            transform.position = Vector3.MoveTowards(transform.position,destinaton,spd*Time.deltaTime);
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

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        public override void SetUp()
        {
            base.SetUp();
            defaultPosition = new Vector3(Random.Range(10f, 17f), Random.Range(-2.5f, -1.5f), 0);
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

