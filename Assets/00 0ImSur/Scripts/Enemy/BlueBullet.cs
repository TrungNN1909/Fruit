using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Unicorn
{
    public class BlueBullet : BaseEnemy
    {
        public bool isShooting;
        [SerializeField] private GameObject explosion;
      
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
            if(isAttacking && isShooting) Move(hitPoint.transform.position);
        }

        public override void SetUp()
        {
            base.SetUp();
            isShooting = false;
            isAttacking = false;
            stateMachine.ChangeState(attackState);
            spd = 7f;
        }

        protected override void CheckDead()
        {
            if (hp <= 0)
            {
                PlayingManager.Instance.CheckEndPhase();
                isShooting = false;
                isAttacking = false;
                SimplePool.Despawn(gameObject);
            }
        }

        protected override void OnTriggerEnter2D(Collider2D col)
        {
            if(hp <0) return;
            
            if (col.gameObject.CompareTag("HitPoint"))
            {
                SimplePool.Spawn(explosion);
                PlayingManager.Instance.CheckEndPhase();
                SimplePool.Despawn(gameObject);
                return;
            }

            if (col.gameObject.CompareTag("GunBullet"))
            {
                hp -= col.gameObject.GetComponent<Bullet>().dame;
                healthBar.ChangeValue(hp);
                CheckDead();
            }

        }
    }
}
