using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Unicorn
{
    public class SmallGreen : BaseEnemy
    {
        [SerializeField] private GameObject darkSmokeSpawnFX;
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
            transform.position = Vector3.MoveTowards(transform.position,destinaton ,spd * Time.deltaTime);
        }

        protected override void Attack()
        {
            base.Attack();
            if(isAttacking) Move(hitPoint.transform.position);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SimplePool.Spawn(darkSmokeSpawnFX,transform.position,Quaternion.identity);
        }

        public override void SetUp()
        {
            base.SetUp();
            defaultPosition = transform.position;
            spd = Mathf.Clamp(baseSpeed + 0.1f * PlayerDataManager.Instance.GetStage() , baseSpeed , 10f);
            stateMachine.ChangeState(attackState);
        }

        public override void DeadAction()
        {
            base.DeadAction();
            transform.DOMoveX(35f, 3f).OnComplete(() =>
            {
                SimplePool.Despawn(gameObject);
            });
        }

    }
}
