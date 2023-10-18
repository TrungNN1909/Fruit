using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class E_Attack : State
    {
        private BaseEnemy baseEnemy;
        public Transform target;
        public E_Attack(BaseEnemy entity, FiniteStateMachine finiteStateMachine) : base(entity, finiteStateMachine)
        {
            baseEnemy = entity;
        }

        public override void OnEnter()
        {
            if (baseEnemy is BlueBullet)
            {
                BlueBullet bl = (BlueBullet)baseEnemy;
                bl.isAttacking = true;
                return;
            }
            base.OnEnter()
            ;
            if (baseEnemy.isBoss)
            { 
                baseEnemy.StartCoroutine(DelayToAttack(Random.Range(5f,10f)));
            }
            else
            {
                baseEnemy.StartCoroutine(DelayToAttack(Random.Range(4f,7f)));
            }

            if (baseEnemy is GreenBoss)
            {
                GreenBoss green = (GreenBoss) baseEnemy;
                baseEnemy.StartCoroutine(DelaySpawnSmallGreen(green));
            }
        }

        private IEnumerator DelaySpawnSmallGreen(GreenBoss greenBoss)
        {
            yield return Yielders.Get(2f);
            greenBoss.SpawnSmalle();
        }
        
        public override void OnExit()
        {
            baseEnemy.isAttacking = false;
            base.OnExit();
            baseEnemy.animator.ResetTrigger("Attack");

            if (baseEnemy is GreenBoss && PlayingManager.Instance.isPlaying)
            {
                GreenBoss greenBoss = (GreenBoss) baseEnemy;
                greenBoss.SpawnSmall2();
            }
            baseEnemy.StopAllCoroutines();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            baseEnemy.AttackUpdate();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
        }
        
        private IEnumerator DelayToAttack(float delayTime = 2f)
        {
            yield return Yielders.Get(delayTime);
            
            if (baseEnemy is BlueBoss)
            {
                BlueBoss blueBoss = (BlueBoss)baseEnemy;
                blueBoss.animator.SetBool("Charge",true);
            }
            
            baseEnemy.isAttacking = true;
        }

    }
}
