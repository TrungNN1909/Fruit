using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class EnemyTest : FlyFly
    {

        public E_WaitToAttackState waitToAttackState;

        // protected override void Start()
        // {
        //     base.Start();
        //     waitToAttackState = new E_WaitToAttackState(this,stateMachine);
        //     stateMachine.Initialize(waitToAttackState); 
        // }

        protected override void Update()
        {
            base.Update();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }
    }
}
