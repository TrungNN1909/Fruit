using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using DamageNumbersPro;
using Random = UnityEngine.Random;

namespace Unicorn
{
 
    
    public class BaseEnemy : Entity
    {
        //Note reState, stats when onenable
        // scriptable object for enemies stats
        // move to eneme's Stats script
        
        public E_WaitToAttackState waitToAttackState;
        public E_Attack attackState;
        public E_Dead deadState;
        public E_ChargeAttack chargeAttackState;

        public bool isBoss = false;
        
        public float hp;
        public float atk;
        public float spd;
        public float hpIncrease = 25f;
        public float atkIncrease = 5f;
        public float baseSpeed = 3f;
        public GameObject hitPoint;
        public Vector3 defaultPosition;

        public bool isReachDefaultPosition;
        public bool isAttacking;
        public bool isAttaked;
        public bool isDead;

        public Tween onTakenDmgTween;
        public Vector3 localScale;
        
        [SerializeField] public Animator animator;
        // [SerializeField] public Rigidbody2D rigidbody2D;
        [SerializeField] public GameObject spriteHolder;
        [SerializeField] public EnemiesHpbar healthBar;
        [SerializeField] public DamageNumberMesh dameNumber;
        [SerializeField] public DamageNumberMesh dameNumberCrit;

        protected override void Awake()
        {
            base.Awake();
            waitToAttackState = new E_WaitToAttackState(this,stateMachine);
            attackState = new E_Attack(this, stateMachine);
            deadState = new E_Dead(this, stateMachine);
            chargeAttackState = new E_ChargeAttack(this, stateMachine);
            
            localScale = spriteHolder.transform.localScale;
        }

        // protected override void Start()
        // {
        //     base.Start();
        //     
        //     // Debug.Log("Enemy enter wait to attack state, state = " + stateMachine.currentState);
        // }
        protected override void Update()
        {
            base.Update();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }
            
        public virtual void WaitToAttackUpdate()
        {
            CheckInDefaultPostion();
        }

        public virtual void AttackUpdate()
        {
            
        }

        public virtual void DeadUpdate()
        {
            
        }

        public virtual void ChargeAttackUpdate()
        {
            
        }
        
        protected virtual void Move(Vector3 destinaton)
        {
            
        }

        protected virtual void Attack()
        {
            
        }

        protected virtual void OnEnable()
        {
            SetUp();
        }
    
        public virtual void SetUp()
        {

            stateMachine.Initialize(waitToAttackState);
            gameObject.GetComponent<PolygonCollider2D>().enabled = true;
            StatSetUpPerLevel();
            healthBar.slider.maxValue = hp;
            healthBar.slider.value = hp;
            hitPoint = PlayingManager.Instance.train.hitPointPosition.gameObject;
            
            isDead = false;
        }

        protected  virtual void StatSetUpPerLevel()
        {
            if (PlayerDataManager.Instance.GetStage() < 10)
            {
                hpIncrease = 27f;
                atkIncrease = 3f;
            }else if (PlayerDataManager.Instance.GetStage() >= 10 && PlayerDataManager.Instance.GetStage() < 20)
            {
                hpIncrease = 50f;
                atkIncrease = 5f;
            }
            else if(PlayerDataManager.Instance.GetStage() >= 20 && PlayerDataManager.Instance.GetStage() < 30)
            {
                hpIncrease = 38f;
                atkIncrease = 6f;
            }
            else if(PlayerDataManager.Instance.GetStage() >=30 && PlayerDataManager.Instance.GetStage()<44)
            {
                hpIncrease = 43f;
                atkIncrease = 6f;
            }
            else if (PlayerDataManager.Instance.GetStage() >=44 && PlayerDataManager.Instance.GetStage()<60)
            {
                hpIncrease = 45f;
                atkIncrease = 5f;
            }
            else
            {
                hpIncrease = 45f;
                atkIncrease = 3.5f;
            }

            if (PlayingManager.Instance.GetCurrentPhaseNumber() == 0)
            {
                hp =  hpIncrease * (PlayerDataManager.Instance.GetStage()+1);
                atk = atkIncrease * (PlayerDataManager.Instance.GetStage()+1);
            }else if (PlayingManager.Instance.GetCurrentPhaseNumber() == 1)
            {
                hp =  hpIncrease * (PlayerDataManager.Instance.GetStage()+2);
                atk = atkIncrease * (PlayerDataManager.Instance.GetStage()+2);
            }
            else
            {
                hp =  hpIncrease * (PlayerDataManager.Instance.GetStage()+3);
                atk = atkIncrease * (PlayerDataManager.Instance.GetStage()+3);
            }
            
        }

        protected virtual void OnDisable()
        {
        }

        public virtual void CheckInDefaultPostion()
        {
            if (Mathf.Abs(transform.position.x - defaultPosition.x)<= 0.1f)
            {
                isReachDefaultPosition = true;
            }
            else
            {
                isReachDefaultPosition = false;
            }
        }

        public virtual bool CheckOutScreen()
        {
            if (Mathf.Abs(transform.position.x - 25f) <= 0.1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public virtual void OnTakenDmg(float dameDeal, bool isCrit = false)
        {
            if ( hp<=0 ) return;

            if (onTakenDmgTween != null)
            {
                onTakenDmgTween.Kill();
                spriteHolder.transform.localScale = localScale;
            }

            onTakenDmgTween = spriteHolder.transform.DOScaleY(localScale.y + 0.15f, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                spriteHolder.transform.DOScaleY(localScale.y, 0.1f).SetEase(Ease.InFlash);
            });
     
            if (isCrit)
            {
                dameDeal *= 2;
                dameNumberCrit.Spawn(transform.position, dameDeal);
            }
            else
            {
                dameNumber.Spawn(transform.position, dameDeal);
            }
            
            hp -= dameDeal;
            healthBar.ChangeValue(hp);
            CheckDead();
        }

        protected virtual void CheckDead()
        {
            if (hp <= 0)
            {
                SoundManager.Instance.PlayMonsterDieSound();
                gameObject.GetComponent<PolygonCollider2D>().enabled = false;
                isDead = true;
                animator.SetTrigger("Dead");
                stateMachine.ChangeState(deadState);
            }
        }

        public virtual void DeadAction()
        {
            PlayingManager.Instance.CheckEndPhase();
        }
        protected virtual void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("GunBullet"))
            {
                var dmg = (int)col.gameObject.GetComponent<Bullet>().dame;
                bool isCrit = col.gameObject.GetComponent<Bullet>().crit;
                var offset = Random.Range(-dmg * 0.1f, dmg * 0.3f);
                OnTakenDmg(dmg + offset,isCrit);
            }

            if (col.gameObject.CompareTag("HitPoint"))
            {
                animator.SetTrigger("Attack");
                StartCoroutine(DelayForAnimation());
            }
        }

        protected virtual IEnumerator DelayForAnimation(float delay = 1f)
        {
            yield return  Yielders.Get(delay);
            stateMachine.ChangeState(waitToAttackState);
            isAttacking = false;
        }
        
    }
}
