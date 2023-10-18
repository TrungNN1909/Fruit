using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Unicorn
{
    public class E_WaitToAttackState : State
    {
        private BaseEnemy baseEnemy;
        public E_WaitToAttackState(BaseEnemy  entity, FiniteStateMachine finiteStateMachine) : base(entity, finiteStateMachine)
        {
            this.baseEnemy = entity;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
        }

        public override void OnExit()
        {
            base.OnExit();
            baseEnemy.isReachDefaultPosition = false;

        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            baseEnemy.WaitToAttackUpdate();
            
        }
        
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
        }

        
    }
}
