using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Unicorn
{
    public class Fidd : BaseEnemy
    {
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
            transform.position =
                Vector3.MoveTowards(transform.position, destinaton, spd * Time.deltaTime);
        }

        protected override void Attack()
        {
            base.Attack();
            if(isAttacking) Move(hitPoint.transform.position);
        }
        
        public override void SetUp()
        {
            base.SetUp();
            defaultPosition = new Vector3(Random.Range(10f, 18f), -7.5f, 0);
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
