using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unicorn.Utilities;
namespace Unicorn
{
    public class SubGun : MonoBehaviour
    {

        public float dmg;
        public float spd;
        public float fireRate = 0.5f;
        public float timeToShoot = 0.5f;

        [SerializeField] private GameObject catHand;
        [SerializeField] private GameObject bulletPrefab;

        private SubGunInfo currentSubGunInfo;

        private AutoFindTargets autoFindTarget;

        List<GameObject> bullets;
        GameObject bullet;
        Vector3 dir;

        public bool hasEnemy;

        // Start is called before the first frame update
        private void Start()
        {
            autoFindTarget = gameObject.GetComponent<AutoFindTargets>();
            Init();

        }

        public void Init()
        {
            currentSubGunInfo = PlayerDataManager.Instance.GetSubGun(PlayerPrefs.GetInt(Helper.SubGunHighestLevel,1));
            dmg = currentSubGunInfo.baseDmg * currentSubGunInfo.lv;
            spd = currentSubGunInfo.spd;
            bulletPrefab = currentSubGunInfo.bullet;
            fireRate = currentSubGunInfo.fireRate;
            // ObjectPool.instance.go = bulletPrefab;
            // bullets = ObjectPool.instance.CreatingObjects(bulletPrefab, 10);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            catHand.SetActive(false);

        }


        private void Shooting()
        {
            if(PlayingManager.Instance.isTutorial) return;

            SetTarget(autoFindTarget.GetTarget());

            if (GameManager.Instance.GameStateController.CurrentGameState == GameState.IN_GAME && PlayingManager.Instance.isPlaying)
            {
                if (hasEnemy)
                {
                    transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg, Vector3.forward);
                    timeToShoot += Time.deltaTime;
                    if (timeToShoot >= fireRate)
                    {
                        SoundManager.Instance.PlayShotSupportSound();
                        gameObject.GetComponent<SpriteRenderer>().enabled = true;
                        catHand.SetActive(true);
                        ShotAnim();
                        Shoot();
                        timeToShoot = 0;
                    }
                }

            }
            if (!hasEnemy || !PlayingManager.Instance.isPlaying)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                transform.rotation = Quaternion.identity;
                catHand.SetActive(false);
            }
        }

        //Located in LevelManager, will be called on state  update action
        public void OnUpdate()
        {
            Shooting();
        }

        private void Shoot()
        {
            // bullet = ObjectPool.instance.GetPoolObject(bullets);
            bullet = SimplePool.Spawn(bulletPrefab);
            if (bullet != null)
            {
                bullet.GetComponent<Bullet>().dame = dmg;
                bullet.SetActive(true);
                Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                bullet.gameObject.transform.position = spawnPosition;
                bullet.transform.Rotate(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
                bullet.GetComponent<Bullet>().setSpeed(dir, spd);
            }
        }

        public void SetTarget(GameObject target)
        {
            if ( target == null || !target.activeInHierarchy )
            { 
                hasEnemy = false;
                return;
            }

            hasEnemy = true;
            dir = target.transform.position - transform.position;
            dir = dir.normalized;
            Debug.DrawRay(transform.position, dir*20f, Color.blue);

        }

        private void ShotAnim()
        {
            transform.DOShakePosition(0.25f, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
            {
                transform.localPosition = Vector3.zero;
            });
        }
    }
}
