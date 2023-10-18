using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.Serialization.Utilities;
using UnityEngine;

namespace Unicorn
{
    public class HitPoint : MonoBehaviour
    {
        [SerializeField] Train car;
        private Vector3 carPosition;
        bool isDead = false;
        private bool isTakenDmg = true;
        Coroutine hpDecreaseCotoutine;

        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (PlayingManager.Instance.isImmortal || !PlayingManager.Instance.isPlaying)
            {
                isDead = false;
                return;
            }

            if (collision.gameObject.CompareTag("Enemy"))
            {
                if(isDead) return;
                
                if (car.isTakeDameAnimation)
                {
                    car.OnTakenDame();
                    car.isTakeDameAnimation = false;
                    GameManager.Instance.uiGamePlayController.uiPlaying.onTakenDmgBack.SetActive(true);
                }
                

                if (collision.gameObject.GetComponent<BaseEnemy>().isBoss && isTakenDmg)
                {
                    car.HP -= collision.gameObject.GetComponent<BaseEnemy>().atk;
                    car.gameObject.transform.DOShakePosition(1.5f, 2f).SetEase(Ease.Linear).OnStart(() =>
                    {
                        isTakenDmg = false;
                    }).OnComplete(() =>
                    {
                        gameObject.GetComponent<BoxCollider2D>().enabled = true;
                        car.GetComponent<Train>().ResetPostion();
                        isTakenDmg = true;
                    });
                    
                }
                else if(!collision.gameObject.GetComponent<BaseEnemy>().isBoss)
                {
                    car.HP -= collision.gameObject.GetComponent<BaseEnemy>().atk;
                }

                CheckDead();
                
                return;

            }

            if (collision.gameObject.CompareTag("EnemyBullet"))
            {
                if (car.isTakeDameAnimation)
                {
                    car.OnTakenDame();
                    GameManager.Instance.uiGamePlayController.uiPlaying.onTakenDmgBack.SetActive(true);
                    car.isTakeDameAnimation = false;
                }
                if(isDead) return;
                car.HP -= collision.gameObject.GetComponent<EnemyBullet>().atk;
                CheckDead();
            }

            if (collision.gameObject.CompareTag("BlueBullet"))
            {
                if (car.isTakeDameAnimation)
                {
                    car.OnTakenDame();
                    car.isTakeDameAnimation = false;
                }
                if(isDead) return;
                car.HP -= collision.gameObject.GetComponent<BlueBullet>().atk;
                CheckDead();
            }
        }
        
        private void CheckDead()
        {
            if (car.HP <= 0 && isDead == false)
            {
                car.life--;
                if (car.life != 0)
                {
                    car.HP = PlayingManager.Instance.hpOnRevive;
                    PlayingManager.Instance.isImmortal = true;
                    Invoke(nameof(DelayForImmortal),3f);
                    car.ActiveHealingFx();
                    return;
                }
                Debug.Log("Check");
                isDead = true;
                Time.timeScale = Mathf.MoveTowards(Time.timeScale,0.1f, 5f*Time.deltaTime);
                PlayingManager.Instance.EndStage(LevelResult.Lose);
            }
        }

        private void DelayForImmortal()
        {
            PlayingManager.Instance.isImmortal = false;
        }
    }
}
