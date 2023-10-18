using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Unicorn
{
    public class E_ChargeAttack : State
    {
        private BaseEnemy baseEnemy;
        public E_ChargeAttack(BaseEnemy entity, FiniteStateMachine finiteStateMachine) : base(entity, finiteStateMachine)
        {
            baseEnemy = entity;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            if (baseEnemy is BlueBoss)
            {
                BlueBoss blueBoss = (BlueBoss)baseEnemy;
                blueBoss.ischarge = true;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            if (baseEnemy is BlueBoss)
            {
                BlueBoss blueBoss = (BlueBoss)baseEnemy;
                foreach (var bl in  blueBoss.bullets)
                {
                    bl.isShooting = true;
                }
                blueBoss.bullets.Clear();
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            baseEnemy.ChargeAttackUpdate();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
        }
    }
}
