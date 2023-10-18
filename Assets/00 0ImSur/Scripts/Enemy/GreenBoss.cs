using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

namespace Unicorn
{
  

    public class GreenBoss : BaseEnemy
    {
        [SerializeField] public GameObject smallGreen;
        [SerializeField] private Transform spawnPos1;
        [SerializeField] private Transform spawnPos2;
        private float spawnPos3Y;
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
            if (isAttacking)
            {
                Attack();
            }
        }

        public override void DeadUpdate()
        {
            base.DeadUpdate();
        }

        protected override void Move(Vector3 destinaton)
        {
            base.Move(destinaton);
            transform.position = Vector3.MoveTowards(transform.position,destinaton,spd*Time.deltaTime);
        }

        protected override void Attack()
        {
            base.Attack();
            Move(hitPoint.transform.position);
            
        }

        public void SpawnSmalle()
        {
            if(isDead) return;

            GameObject green1 = SimplePool.Spawn(smallGreen, spawnPos1.position, quaternion.identity);
            PlayingManager.Instance.currentEnemies.Add(green1.GetComponent<BaseEnemy>());
            GameObject green2 = SimplePool.Spawn(smallGreen,spawnPos2.position,quaternion.identity);
            PlayingManager.Instance.currentEnemies.Add(green2.GetComponent<BaseEnemy>());

        }

        public void SpawnSmall2()
        {
            if(isDead) return;
            
            GameObject green = SimplePool.Spawn(smallGreen,new Vector3(spawnPos2.position.x,spawnPos3Y,0),quaternion.identity);
            PlayingManager.Instance.currentEnemies.Add(green.GetComponent<BaseEnemy>());
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            isBoss = true;
        }

        public override void SetUp()
        {

            base.SetUp();
            defaultPosition = new Vector3(11f, -0.9f, 0);
            spd = Mathf.Clamp(baseSpeed + 0.1f * PlayerDataManager.Instance.GetStage() , baseSpeed , 10f);
            spawnPos3Y = spawnPos2.position.y;
        }
        
        protected override void StatSetUpPerLevel()
        {
            if (PlayerDataManager.Instance.GetStage() < 10)
            {
                hpIncrease = 200f;
                atkIncrease = 20f;
            }else if (PlayerDataManager.Instance.GetStage() >= 10 && PlayerDataManager.Instance.GetStage() < 20)
            {
                hpIncrease = 400f;
                atkIncrease = 25f;
            }
            else if (PlayerDataManager.Instance.GetStage() >= 20 && PlayerDataManager.Instance.GetStage() < 40)
            {
                hpIncrease = 400f;
                atkIncrease = 25f;
            }
            else
            {
                hpIncrease = 700;
                atkIncrease = 10f;
            }
            hp =  hpIncrease * (PlayerDataManager.Instance.GetStage()+1);
            atk = atkIncrease * (PlayerDataManager.Instance.GetStage()+1);
        }
        protected override void OnDisable()
        {
            base.OnDisable();
        }

        public override void CheckInDefaultPostion()
        {
            base.CheckInDefaultPostion();
        }

        public override bool CheckOutScreen()
        {
            return base.CheckOutScreen();
        }



        protected override void CheckDead()
        {
            base.CheckDead();
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
