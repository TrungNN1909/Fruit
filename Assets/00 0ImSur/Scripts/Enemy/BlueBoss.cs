using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

namespace Unicorn
{
    public class BlueBoss : BaseEnemy
    {
        
        [SerializeField] private GameObject BlueBullet;
        [SerializeField] public List<BlueBullet> bullets;
        [SerializeField] private List<Transform> bulletSpawnPos;

        public bool ischarge;
        private float chargeTime = 1f;
        private float timeToShot = 1f;
        private int attackCount;
  

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
            if (isAttacking) Attack();

        }


        public override void ChargeAttackUpdate()
        {
            base.ChargeAttackUpdate();
            if (ischarge)
            {
                ChargeAttack();
            }

            if (isAttaked)
            {
                animator.SetTrigger("Attack");
                StartCoroutine(DelayForAnimation());
                isAttaked = false;
            }
        }

        private IEnumerator DelayForAnimation()
        {
            yield return Yielders.Get(1f);
            animator.ResetTrigger("Attack");
            Shoot();
            stateMachine.ChangeState(waitToAttackState);
        }
        protected override void Move(Vector3 destinaton)
        {
            base.Move(destinaton);
            transform.position = Vector3.MoveTowards(transform.position, destinaton, spd * Time.deltaTime);
        }
        
        protected override void Attack()
        {
            base.Attack();
            stateMachine.ChangeState(chargeAttackState);
        }
        
        private void ChargeAttack()
        {
            if (timeToShot >= chargeTime && attackCount < 3)
            {
                BlueBullet bullet = SimplePool
                    .Spawn(BlueBullet, bulletSpawnPos[attackCount].position, quaternion.identity)
                    .GetComponent<BlueBullet>();
                PlayingManager.Instance.currentEnemies.Add(bullet);
                bullets.Add(bullet);
                bullet.atk = atk;
                timeToShot = 0;
                attackCount++;
            }

            if (attackCount == 3)
            {
                ischarge = false;
                isAttaked = true;
                attackCount = 0;
            }

            timeToShot += Time.deltaTime;
        }

        public void Shoot()
        {
            foreach (var bullet in bullets)
            {
                bullet.isShooting = true;
            }
        }
 
        public override void SetUp()
        {
            chargeTime = 1f;
            timeToShot = 1f;
            base.SetUp();
            attackCount = 0;
            isBoss = true;
            defaultPosition = new Vector3(15, 0, 0);
            spd = Mathf.Clamp(baseSpeed + 0.1f * PlayerDataManager.Instance.GetStage() , baseSpeed , 10f);
        }
        protected override void StatSetUpPerLevel()
        {
            if (PlayerDataManager.Instance.GetStage() < 10)
            {
                hpIncrease = 200f;
                atkIncrease = 20f;
            }else if (PlayerDataManager.Instance.GetStage() >= 10 && PlayerDataManager.Instance.GetStage() < 20)
            {
                hpIncrease = 400f;
                atkIncrease = 25f;
            }
            else if (PlayerDataManager.Instance.GetStage() >= 20 && PlayerDataManager.Instance.GetStage() < 40)
            {
                hpIncrease = 400f;
                atkIncrease = 25f;
            }
            else
            {
                hpIncrease = 700;
                atkIncrease = 10f;
            }
            hp =  hpIncrease * (PlayerDataManager.Instance.GetStage()+1);
            atk = atkIncrease * (PlayerDataManager.Instance.GetStage()+1);
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
