using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unicorn
{
    public class Train : MonoBehaviour
    {
        public float HP = 200f;
        public float baseHP;
        public float maxHP ;
        public int life;
        
        private int level;
        public CarInfo carinfo;

        [SerializeField] public Transform gunPosition;
        [SerializeField] public Transform catPositon;
        [SerializeField] public Transform[] subCatPosition;
        [SerializeField] public Transform driver;
        [SerializeField] public Transform hitPointPosition; // --> where enemies will find to hit
        [SerializeField] public Transform smokePosition;
        [SerializeField] public Transform dustPosition;
        [SerializeField] public Transform healPosition;
        
        [SerializeField] public GameObject immortalEffect;
        [SerializeField] public GameObject wheelLeft;
        [SerializeField] public GameObject wheelRight;
        [SerializeField] public GameObject wheelMiddle;

        [SerializeField] private GameObject smoke;
        [SerializeField] private GameObject dust;
        [SerializeField] private GameObject atkBuffFx;
        [SerializeField] private GameObject fireRateBuffFx;
        [SerializeField] private GameObject HealingField;
        
        private Tween wheelLefTween;
        private Tween wheelLefTween1;

        private Tween wheelRightTween;
        private Tween wheelRightTween1;

        private Tween wheelMidleTween;
        private Tween wheelMidleTween1;

        public bool isTakeDameAnimation;
        private float timeToActiveAnimation = 0f;
        private float timePerAnimation = 0.28f;

        
        private void Start()
        {
            PlayingManager.Instance.carMoveInAction = MoveInAction;
            PlayingManager.Instance.carMoveOutAction = MoveOutAction;
            PlayingManager.Instance.buffAttackAction += BuffAttackFXAction;
            PlayingManager.Instance.buffFireRateAction += BuffFireRateFXAction;
        }

        public void Init()
        {
            life = 1;
            level = PlayerDataManager.Instance.GetCarHighestLevel();
            carinfo = PlayerDataManager.Instance.GetCurrentCar(level);

            baseHP = carinfo.baseHP + PlayerDataManager.Instance.GetTimeToNextLevel() * carinfo.amountIncreaseHP;
            HP = carinfo.baseHP + PlayerDataManager.Instance.GetTimeToNextLevel() * carinfo.amountIncreaseHP;
            maxHP = HP;
            transform.position = carinfo.defaultPositionNotOnPlaying;
        }

        public void OnUpdate()
        {
            TakeDameAnimationAction();
        }

        private void TakeDameAnimationAction()
        {
            if (timeToActiveAnimation >= timePerAnimation)
            {
                isTakeDameAnimation = true;
                timeToActiveAnimation = 0;
            }
            timeToActiveAnimation += Time.deltaTime;
        }

        public void OnTakenDame()
        {
            SoundManager.Instance.PlayCatHitSound();
            SoundManager.Instance.PlayCarHitSound();
            SoundManager.Instance.PlayVibration();
            transform.DOScale(new Vector3(1.15f, 1.15f, 1.15f), 0.125f).SetEase(Ease.InBack).OnStart(() =>
            {
                transform.localScale = Vector3.one;
            }).OnComplete(() =>
            {
                transform.DOScale(Vector3.one, 0.125f).SetEase(Ease.OutBack);
            });
        }
        
        private void MoveInAction()
        {
            transform.DOMoveX(carinfo.defaultPositionNotOnPlaying.x, 2f).SetEase(Ease.OutSine).OnStart(() =>
            {
                wheelRightTween.Kill();
                wheelLefTween.Kill();
                wheelMidleTween.Kill();
                
                ResetPostion();
            
                wheelLefTween1 = wheelLeft.transform.DORotate(new Vector3(0, 0, 360), 1f, RotateMode.FastBeyond360)
                    .SetLoops(3, LoopType.Restart).SetEase(Ease.Linear);
                wheelRightTween1 = wheelRight.transform.DORotate(new Vector3(0, 0, 360), 1f, RotateMode.FastBeyond360)
                    .SetLoops(3, LoopType.Restart).SetEase(Ease.Linear);
                wheelMidleTween1 = wheelMiddle.transform.DORotate(new Vector3(0, 0, 360), 1f, RotateMode.FastBeyond360)
                    .SetLoops(3, LoopType.Restart).SetEase(Ease.Linear);
                
               GameObject smokeTemp = SimplePool.Spawn(smoke,smokePosition.position,Quaternion.Euler(0,90,90));
               smokeTemp.transform.SetParent(smokePosition);
            }).OnComplete((() =>
            {
                StartCoroutine(DelayToStopMoving());
            }));
        }

        private void MoveOutAction()
        {
            SoundManager.Instance.PlayCarStartRunSound();
            StartCoroutine(DelayToPlaySoundCarRun());
            transform.DOMoveX(carinfo.defaultPositonOnPlaying.x, 2f).SetEase(Ease.InSine).OnStart((() =>
            {
                GameObject smokeTemp = SimplePool.Spawn(smoke, smokePosition.position, Quaternion.Euler(0, 90, 90));
                smokeTemp.transform.SetParent(smokePosition);
                ResetPostion();

                wheelLeft.transform.DORotate(new Vector3(0, 0, 360), 1.5f, RotateMode.FastBeyond360)
                    .SetEase(Ease.InSine).OnComplete(
                        () =>
                        {
                            wheelLefTween = wheelLeft.transform
                                .DORotate(new Vector3(0, 0, 360), 0.5f, RotateMode.FastBeyond360)
                                .SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
                        });
                wheelRightTween = wheelRight.transform.DORotate(new Vector3(0, 0, 360), 1.5f, RotateMode.FastBeyond360)
                    .SetEase(Ease.InSine).OnComplete(
                        () =>
                        {
                            wheelRightTween = wheelRight.transform
                                .DORotate(new Vector3(0, 0, 360), 0.5f, RotateMode.FastBeyond360)
                                .SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
                        });
                wheelMiddle.transform.DORotate(new Vector3(0, 0, 360), 1.5f, RotateMode.FastBeyond360)
                    .SetEase(Ease.InSine).OnComplete(
                        () =>
                        {
                            wheelMidleTween = wheelMiddle.transform
                                .DORotate(new Vector3(0, 0, 360), 0.5f, RotateMode.FastBeyond360)
                                .SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
                        });
            }));
        }

        private IEnumerator DelayToPlaySoundCarRun()
        {
            yield return Yielders.Get(1.5f);
            SoundManager.Instance.PlayCarRunSound();
        }
        
        private IEnumerator DelayToStopMoving()
        {            
            
            yield return Yielders.Get(1f);
            SoundManager.Instance.PlayCarBrakeSound();
            SoundManager.Instance.StopCarRunSound();
            wheelRightTween1.Kill();
            wheelLefTween1.Kill();
            wheelMidleTween1.Kill();
            ResetPostion();
            wheelLeft.transform.DORotate(new Vector3(0, 0, 360), 2f, RotateMode.FastBeyond360)
                .SetLoops(1, LoopType.Restart).SetEase(Ease.OutCubic);
            wheelRight.transform.DORotate(new Vector3(0, 0, 360), 2f, RotateMode.FastBeyond360)
                .SetLoops(1, LoopType.Restart).SetEase(Ease.OutCubic);
            wheelMiddle.transform.DORotate(new Vector3(0, 0, 360), 2f, RotateMode.FastBeyond360)
                .SetLoops(1, LoopType.Restart).SetEase(Ease.OutCubic);
                
            yield return Yielders.Get(01f);
            SimplePool.Spawn(dust, dustPosition.position, quaternion.identity);                
            PlayingManager.Instance.isBackGoundMoving = false;

        }


        public void ResetPostion()
        {
            wheelLeft.transform.rotation = quaternion.identity;
            wheelRight.transform.rotation = quaternion.identity;
            wheelMiddle.transform.rotation = quaternion.identity;

        }

        private void BuffAttackFXAction()
        {
            GameObject vfx = SimplePool.Spawn(atkBuffFx,transform.position,Quaternion.Euler(-90,0,0));
            vfx.transform.SetParent(gameObject.transform);
        }

        private void BuffFireRateFXAction()
        {
            GameObject vfx =SimplePool.Spawn(fireRateBuffFx,transform.position,Quaternion.Euler(-90,0,0));
            vfx.transform.SetParent(gameObject.transform);
        }

        public void ActiveHealingFx()
        {
            SimplePool.Spawn(HealingField, healPosition.position, Quaternion.Euler(-100, 0, 0));
        }
    }

}