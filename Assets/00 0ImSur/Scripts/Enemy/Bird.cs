using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Unicorn
{
    public class Bird : BaseEnemy
    {
        public override void WaitToAttackUpdate()
        {
            base.WaitToAttackUpdate();
            Move(defaultPosition);
            if (isReachDefaultPosition) stateMachine.ChangeState(attackState);
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
            if (isAttacking) Move(hitPoint.transform.position);
        }



        public override void SetUp()
        {
            base.SetUp();
            defaultPosition = new Vector3(Random.Range(10f, 18f), Random.Range(-6f, -5f), 0);
            spd = Mathf.Clamp(baseSpeed + 0.1f * PlayerDataManager.Instance.GetStage() , baseSpeed , 10f);
        }


        public override void DeadAction()
        {
            base.DeadAction();
            transform.DOMoveY(-3.5f, 0.5f).OnComplete(() =>
            {
                transform.DOMoveX(35f, 3f).OnComplete(() => { gameObject.SetActive(false); });
            });
        }
    }
}

