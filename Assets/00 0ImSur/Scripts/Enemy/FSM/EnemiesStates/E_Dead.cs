using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Unicorn
{
    public class E_Dead : State
    {
        private BaseEnemy baseEnemy;

        public E_Dead(BaseEnemy entity, FiniteStateMachine finiteStateMachine) : base(entity, finiteStateMachine)
        {
            baseEnemy = entity;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            baseEnemy.DeadAction();
        }

        public override void OnExit()
        {
            base.OnExit();
            baseEnemy.isAttacking = false;
            baseEnemy.isAttaked = false;
            baseEnemy.isDead = false;
            baseEnemy.isReachDefaultPosition = false;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            baseEnemy.DeadUpdate();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
        }
    }
}
