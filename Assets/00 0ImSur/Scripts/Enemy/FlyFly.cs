using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Unicorn
{
    public class FlyFly : BaseEnemy 
    {
        
        public override void SetUp()
        {
            base.SetUp();
            defaultPosition = new Vector3(Random.Range(8f,12f), Random.Range(2f, 4f), 0);
            spd = Mathf.Clamp(baseSpeed + 0.1f * PlayerDataManager.Instance.GetStage() , baseSpeed , 10f);

        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SetUp();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }


        protected override void Update()
        {
            base.Update();
            
        }

        public override void WaitToAttackUpdate()
        {
            base.WaitToAttackUpdate();
            
            Move(defaultPosition);
            if (isReachDefaultPosition)
            {
                //TODO: change state to attack
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

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            
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
                Move(hitPoint.transform.position);
            }
        }

        public override void DeadAction()
        {
            base.DeadAction();
            
            transform.DOMove(new Vector3(35f,-5,0f), 3f, false).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }

  
    }
}
