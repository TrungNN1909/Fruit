using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Internal;
using UnityEngine.Pool;
using DG.Tweening;
using Unicorn.Utilities;

namespace Unicorn
{
    public enum ShootingType
    {
        Normal,
        Double,
        Split,
        Triple,
        Quadra,
        Multi,
        DoubleMulti,
    }


    public class AimShoot : MonoBehaviour
    {

        private GunInfos gunInfos;

        public bool isShotting;
        public float dmg;
        public float spd;
        public float fireRate = 0.5f;
        public float timeToShoot = 0.5f;
        public int critRate ;

        public ShootingType type = ShootingType.Normal;

        public Vector3 dir;

        [SerializeField] private GameObject crosshair;
        [SerializeField] private GameObject catHand;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform bulletSpawnPos;
        [SerializeField] private GameObject fieldOfView;
        [SerializeField] private GameObject gunFireFX;
        
        List<GameObject> bullets;
        
        private Tweener tween;
        public void Init()
        {   //generate main gun's infomation.
            gunInfos = PlayerDataManager.Instance.GetGun(PlayerPrefs.GetInt(Helper.GunHightestLevel,1));
            dmg = gunInfos.baseDmg * gunInfos.lv;
            spd = gunInfos.spd;
            type = ShootingType.Normal;
            bulletPrefab = gunInfos.bullet;
            critRate = 5;
            
            catHand.SetActive(false);
            dir = Vector3.zero;
        }

        private void Start()
        {
            Init();
            PlayingManager.Instance.buffAttackAction += BuffAttackAction;
            PlayingManager.Instance.buffFireRateAction += BuffFireRateAction;
            tween = transform.DOLocalMoveX(transform.localPosition.x + 0.5f, 0.1f).SetLoops(-1, LoopType.Yoyo);
            tween.Pause();
        }

        public void OnUpdate()
        {
            OnShoot();
        }
  
        public void OnShoot()
        {
            if(PlayingManager.Instance.isTutorial) return;
            
            //update gun's quaternion
            dir = crosshair.transform.position - bulletSpawnPos.position; // vector forwarrd top of gun(spawnpos) and crosshair
            dir = dir.normalized;
            
            // if(Input.GetMouseButtonDown(0)) SoundManager.Instance.PlayShotSound();
            // check when to shoot
            if (Input.touchCount > 0 && PlayingManager.Instance.isPlaying)
            {
                isShotting = true;
                // catHand.SetActive(true);
                crosshair.SetActive(true);
                fieldOfView.SetActive(true);
                tween.Play();
            }

            if (!PlayingManager.Instance.isPlaying )
            {
                isShotting = false;
            }
            
            if (!isShotting)
            {
                
                isShotting = false;
                catHand.SetActive(false);
                crosshair.SetActive(false);
                fieldOfView.SetActive(false);
                timeToShoot = fireRate;
                transform.rotation = Quaternion.identity;
                transform.localPosition = Vector3.zero;
                tween.Pause();
            }

            //shooting
            if (isShotting)
            {
                RotateGun();
                SimplePool.Spawn(gunFireFX,bulletSpawnPos.position,Quaternion.Euler(0,-90f,90f));
                timeToShoot += Time.deltaTime;

                if (timeToShoot >= fireRate)
                {
                    SoundManager.Instance.PlayShotSound();
                    switch (type)
                    {
                        case ShootingType.Normal:
                            Shoot();
                            break;

                        case ShootingType.Double:
                            StartCoroutine(DoubleShoot());
                            break;

                        case ShootingType.Split:
                            SplitShoot();
                            break;

                        case ShootingType.Triple:
                            StartCoroutine(TripleShoot());
                            break;

                        case ShootingType.Quadra:
                            StartCoroutine(QuadraShoot());
                            break;

                        case ShootingType.Multi:
                            MultiShoot();
                            break;
                        
                        case ShootingType.DoubleMulti:
                            StartCoroutine(DoubleMultiShoot());
                            break;
                    }

                    timeToShoot = 0;
                }
            }
        }


        GameObject bullet;
        GameObject bullet1;
        GameObject bullet2;
        private void Shoot()
        {
            // bullet = ObjectPool.instance.GetPoolObject(bullets);
            bullet = SimplePool.Spawn(bulletPrefab);
            if (bullet != null)
            {
                if (Crit())
                {
                    bullet.GetComponent<Bullet>().crit = true;
                }
                else
                {
                    bullet.GetComponent<Bullet>().crit = false;
                }
                
                bullet.GetComponent<Bullet>().dame = dmg;
                bullet.SetActive(true);
                //Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                bullet.gameObject.transform.position = bulletSpawnPos.position;
                bullet.transform.Rotate(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
                bullet.GetComponent<Bullet>().setSpeed(dir, spd);
            }
        }

        private IEnumerator DoubleShoot()
        {
            for (int i = 0; i < 2; i++)
            {
                yield return new WaitForSeconds(Time.deltaTime * 7);
                Shoot();
            }
        }

        private IEnumerator TripleShoot()
        {
            for (int i = 0; i < 3; i++)
            {
                Shoot();
                yield return new WaitForSeconds(Time.deltaTime * 7);
            }
        }

        private IEnumerator QuadraShoot()
        {
            for(int i=0; i<2; i++)
            {
                SplitShoot();
                yield return new WaitForSeconds(Time.deltaTime * 7);
                
            }
        }

        private IEnumerator DoubleMultiShoot()
        {
            for (int i = 0; i < 2; i++)
            {
                MultiShoot();
                yield return Yielders.Get(Time.deltaTime * 7);
            }
        }

        private void MultiShoot()
        {
            //Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            // bullet = ObjectPool.instance.GetPoolObject(bullets);
            bullet = SimplePool.Spawn(bulletPrefab);
            if (bullet != null)
            {
                Vector3 upperBullet = new Vector3(crosshair.transform.position.x, crosshair.transform.position.y + 0.5f, crosshair.transform.position.z);
                Vector3 dir1 = upperBullet - bulletSpawnPos.position;
                dir1 = dir1.normalized;
                if (Crit())
                {
                    bullet.GetComponent<Bullet>().crit = true;

                }
                else
                {
                    bullet.GetComponent<Bullet>().crit = false;

                }
                bullet.GetComponent<Bullet>().dame = dmg;
                bullet.SetActive(true);
                bullet.gameObject.transform.position = bulletSpawnPos.position;
                bullet.transform.Rotate(0, 0, (Mathf.Atan2(dir1.y, dir1.x) * Mathf.Rad2Deg));
                bullet.GetComponent<Bullet>().setSpeed(dir1, spd);
            }
            // bullet2 = ObjectPool.instance.GetPoolObject(bullets);
            bullet2 = SimplePool.Spawn(bulletPrefab);
            if (bullet2 != null)
            {
                Vector3 dir3 = crosshair.transform.position - bulletSpawnPos.position;
                dir3 = dir3.normalized;
                if (Crit())
                {
                    bullet2.GetComponent<Bullet>().crit = true;

                }
                else
                {
                    bullet2.GetComponent<Bullet>().crit = false;
                }
                bullet2.GetComponent<Bullet>().dame = dmg;
                bullet2.SetActive(true);
                bullet2.gameObject.transform.position = bulletSpawnPos.position;
                bullet2.transform.Rotate(0, 0, (Mathf.Atan2(dir3.y, dir3.x) * Mathf.Rad2Deg));
                bullet2.GetComponent<Bullet>().setSpeed(dir3, spd);
            }

            // bullet1 = ObjectPool.instance.GetPoolObject(bullets);
            bullet1 = SimplePool.Spawn(bulletPrefab);
            if (bullet1 != null)
            {
                Vector3 lower = new Vector3(crosshair.transform.position.x, crosshair.transform.position.y - 0.5f, crosshair.transform.position.z);

                Vector3 dir2 = lower - bulletSpawnPos.position;
                dir2 = dir2.normalized;
                if (Crit())
                {
                    bullet1.GetComponent<Bullet>().crit = true;
                }
                else
                {
                    bullet1.GetComponent<Bullet>().crit = false;

                }
                bullet1.GetComponent<Bullet>().dame = dmg;
                bullet1.SetActive(true);
                bullet1.gameObject.transform.position = bulletSpawnPos.position;
                bullet1.transform.Rotate(0, 0, (Mathf.Atan2(dir2.y, dir2.x) * Mathf.Rad2Deg));
                bullet1.GetComponent<Bullet>().setSpeed(dir2, spd);
            }
        }

        private void SplitShoot()
        {
            //Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            // bullet = ObjectPool.instance.GetPoolObject(bullets);
            bullet = SimplePool.Spawn(bulletPrefab);
            if (bullet != null)
            {
                Vector3 upperBullet = new Vector3(crosshair.transform.position.x, crosshair.transform.position.y + 0.5f, crosshair.transform.position.z);
                Vector3 dir1 = upperBullet - bulletSpawnPos.position;
                dir1 = dir1.normalized;
                if (Crit())
                {
                    bullet.GetComponent<Bullet>().crit = true;

                }
                else
                {
                    bullet.GetComponent<Bullet>().crit = false;

                }
                
                bullet.GetComponent<Bullet>().dame = dmg;
                bullet.SetActive(true);
                bullet.gameObject.transform.position = bulletSpawnPos.position;
                bullet.transform.Rotate(0, 0, (Mathf.Atan2(dir1.y, dir1.x) * Mathf.Rad2Deg));
                bullet.GetComponent<Bullet>().setSpeed(dir1, spd);
            }

            // bullet1 = ObjectPool.instance.GetPoolObject(bullets);
            bullet1 = SimplePool.Spawn(bulletPrefab);
            if (bullet1 != null)
            {
                Vector3 lower = new Vector3(crosshair.transform.position.x, crosshair.transform.position.y - 0.5f, crosshair.transform.position.z);

                Vector3 dir2 = lower - bulletSpawnPos.position;
                dir2 = dir2.normalized;
                if (Crit())
                {
                    bullet1.GetComponent<Bullet>().crit = true;

                }
                else
                {
                    bullet1.GetComponent<Bullet>().crit = false;

                }
                bullet1.GetComponent<Bullet>().dame = dmg;
                bullet1.SetActive(true);
                bullet1.gameObject.transform.position = bulletSpawnPos.position;
                bullet1.transform.Rotate(0, 0, (Mathf.Atan2(dir2.y, dir2.x) * Mathf.Rad2Deg));
                bullet1.GetComponent<Bullet>().setSpeed(dir2, spd);
            }
        }



        public void RotateGun()
        {
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg, Vector3.forward);
        }

        public bool Crit()
        {
            int crit = Random.Range(0, 101);
            return crit < critRate;
        }

        public void BuffAttackAction()
        {
            StartCoroutine(BuffAtk());
        }

        private IEnumerator BuffAtk()
        {
            float currentDmg = dmg;
            dmg += dmg * 0.25f;
            yield return Yielders.Get(10f);
            dmg = currentDmg;
        }
        
        public void BuffFireRateAction()
        {
            StartCoroutine(BuffFireRate());
        }
        private IEnumerator BuffFireRate()
        {
            float currentFR = fireRate;
            fireRate -= fireRate * 0.25f;
            yield return Yielders.Get(10f);
            fireRate = currentFR;
        }
    }
}