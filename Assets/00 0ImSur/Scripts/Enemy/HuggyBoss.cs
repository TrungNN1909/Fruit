using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Unicorn
{
    public class HuggyBoss : BaseEnemy
    {
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

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
            transform.position = Vector3.MoveTowards(transform.position, destinaton, spd * Time.deltaTime);
        }

        protected override void Attack()
        {
            base.Attack();
            if (isAttacking)
            {
                Move(hitPoint.transform.position);
            }
        }

        protected override void OnEnable()
        {
            SetUp();
            isBoss = true;
        }

        public override void SetUp()
        {
            base.SetUp();
            defaultPosition = new Vector3(10, -3.3f, 0);
            spd = Mathf.Clamp(baseSpeed*2 + 0.1f * PlayerDataManager.Instance.GetStage() , baseSpeed , 15f);

        }

        protected override void StatSetUpPerLevel()
        {
            if (PlayerDataManager.Instance.GetStage() < 10)
            {
                hpIncrease = 250f;
                atkIncrease = 20f;
            }else if (PlayerDataManager.Instance.GetStage() >= 10 && PlayerDataManager.Instance.GetStage() < 20)
            {
                hpIncrease = 400f;
                atkIncrease = 25f;
            }
            else if (PlayerDataManager.Instance.GetStage() >= 20 && PlayerDataManager.Instance.GetStage() < 40)
            {
                hpIncrease = 400f;
                atkIncrease = 10f;
            }
            else
            {
                hpIncrease = 600;
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

        protected override void OnTriggerEnter2D(Collider2D col)
        {
            base.OnTriggerEnter2D(col);
            if (col.gameObject.CompareTag("HitPoint"))
            {
                SoundManager.Instance.PlaySoundMonsterAttac();
            }
        }
    }
}
