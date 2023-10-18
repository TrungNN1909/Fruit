using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using static DG.Tweening.DOTweenAnimation;
using System;

namespace Unicorn
{

    public class ButtonPower : MonoBehaviour
    {
        public int id;
        private AimShoot gun;
        public TextMeshProUGUI descriptionText;
        public Image img;
        public Image frame;

        private float dmg;
        private float fireRate;
        private float hp;
        private float baseHP;
        private float maxHP;

        private Tween anim;
        [SerializeField] private GameObject SparkleSoloFX;
        private void OnEnable()
        {
            Init();
        }

        public void SetPower()
        {
            switch (id)
            {
                case 0: // +10% dmg
                    dmg += dmg * 0.1f;
                    break;
                case 1:
                    dmg += dmg * 0.15f;
                    break;
                case 2:
                    dmg += dmg * 0.2f;
                    break;
                case 3:
                    fireRate -= fireRate * 0.15f;
                    break;
                case 4:
                    fireRate -= fireRate * 0.20f;
                    break;
                case 5:
                    fireRate -= fireRate * 0.25f;
                    break;
                case 6:
                    hp += baseHP * 0.2f;
                    maxHP += baseHP * 0.2f;
                    break;
                case 7:
                    hp += baseHP * 0.30f;
                    maxHP += baseHP * 0.3f;
                    break;

                case 8:
                    hp += baseHP * 0.50f;
                    maxHP += baseHP * 0.5f;
                    break;

                case 9: // double shoot

                    if (gun.type == ShootingType.Normal)
                    {
                        gun.type = ShootingType.Double;

                    }else if (gun.type == ShootingType.Double)
                    {
                        gun.type = ShootingType.Triple;
                    }else if (gun.type == ShootingType.Split)
                    {
                        gun.type = ShootingType.Quadra;
                    }else if (gun.type == ShootingType.Multi)
                    {
                        gun.type = ShootingType.DoubleMulti;
                    }

                    break;
                case 10:// split shoot
                    if (gun.type == ShootingType.Normal)
                    {
                        gun.type = ShootingType.Split;

                    }
                    else if (gun.type == ShootingType.Double)
                    {
                        gun.type = ShootingType.Quadra;
                    }
                    else if (gun.type == ShootingType.Split)
                    {
                        gun.type = ShootingType.Multi;
                    }else if (gun.type == ShootingType.Triple)
                    {
                        gun.type = ShootingType.DoubleMulti;
                    }
                   
                    break;
            }

            gun.GetComponent<AimShoot>().dmg = dmg;
            gun.GetComponent<AimShoot>().fireRate = fireRate;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Train>().HP = hp;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Train>().maxHP = maxHP;
        }

        public void OnCLick()
        {

            SetPower();
            SimplePool.Spawn(SparkleSoloFX, gameObject.transform.position, Quaternion.Euler(-90, 0, 0));
            SoundManager.Instance.PlaySoundReward();
            PlayingManager.Instance.LoadPhase();
            GameManager.Instance.GamePlayController.OpenUINewPhase(false);
            

        }

        public void Init()
        {
            gun = PlayingManager.Instance.aimshot;
            // 
            dmg = gun.dmg;
            fireRate = gun.fireRate;

            hp = PlayingManager.Instance.train.HP;
            baseHP = PlayingManager.Instance.train.baseHP;
            maxHP = PlayingManager.Instance.train.maxHP;
            
            // hp = GameObject.FindGameObjectWithTag("Player").GetComponent<Train>().HP;
            // baseHP = GameObject.FindGameObjectWithTag("Player").GetComponent<Train>().baseHP;
            // maxHP = GameObject.FindGameObjectWithTag("Player").GetComponent<Train>().maxHP;

        }

        public void Anim()
        {
            anim = transform.DOScale(1.1f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        }

        private void OnDisable()
        {
            anim.Kill();
        }
    }
}

