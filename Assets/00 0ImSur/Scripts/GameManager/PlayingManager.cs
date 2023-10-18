using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using Unity.Mathematics;

namespace Unicorn
{
    public class PlayingManager : LevelManager
    {
        public static new PlayingManager  Instance;
        public Camera camera;
        
        [SerializeField] private GameObject nova;
        // These are infomations of gameObject will be spawned in hirachy when PlayingScene is loaded
        [SerializeField] LevelLoader levelLoader;
        public CarInfo carInfo;
        private GunInfos gunInfos;
        private SubGunInfo subGunInfo;

        // Manage the coin's behavior in Playing --> use for UI_Win / UI_Lose
        public int coin { get; private set; }
        public int baseCoin;
        public int maxCoinEarn;
        public int minCoinEarn;
        public int totalCoinEarn;
        public int currentCoinEarn;

        //Game Objects will be spawned in Hirachy when PlayingScene is load
        [SerializeField] private GameObject backgroundSpawnPosition;
        [HideInInspector] public GameObject car;
        [HideInInspector] public GameObject mainGun;
        [HideInInspector] public GameObject subGun;
        [HideInInspector] public GameObject cat;
        [HideInInspector] public GameObject subCat;
        [HideInInspector] public GameObject driver;
        [HideInInspector] public GameObject backGround;

        //These are the components of game objects was spawned in hirachy.
        public List<SubGun>  subGuns;
        private List<AutoFindTargets> autoFindTargets;
        private List<SubCat> subCats;
        public AimShoot aimshot;
        public Train train;
        public BackgroundManager backgroundManager;
        
        // these booleans control the game as their names
        public bool isPlaying;
        public bool isPhaseEnd;
        public bool isLose;
        public bool isBackGoundMoving;
        public bool isCarMoving;

        public bool isTutorial;

        public bool isImmortal;
        public bool isRevived = false;
        public float hpOnRevive;

        public Action bossStageAction;
        public Action newPhaseStart;
        

        //Check end phases
        public bool isStageEnd;
        public List<BaseEnemy> currentEnemies;
        public int enemiesKilledCount;
        public int totalEnemiesKilledCount;

        //Object Movement manager
        public Action garageMoveInAction;
        public Action<float> garageMoveOutAction;
        
        public Action gasStationMoveInAction;
        public Action<float> gasStationMoveOutAction;

        public Action gasFillerCrossOverAction;

        public Action carMoveInAction;
        public Action carMoveOutAction;
        
        public Action backgroundSlowdown;
        public Action backgroundSpeedUp;
        
        public Action skillMoveOutAction;

        private GameObject _camera;
        
        //Buff Action
        public bool isActiveBuffAtk;
        public bool isActiveBuffFireRate;
        public Action buffAttackAction;
        public Action buffFireRateAction;

        protected override void Awake()
        {
            currentEnemies = new List<BaseEnemy>();
            base.Awake();
            Instance = this;
            _camera = GameObject.FindGameObjectWithTag("MainCamera");
            if (PlayerPrefs.GetInt("isTutorial", 1) == 1)
            {
                isTutorial = true;
            }
        }

        protected override void Start()
        {
            base.Start();
            camera = Camera.main;
            subGuns = new List<SubGun>();
            autoFindTargets = new List<AutoFindTargets>();
            subCats = new List<SubCat>();
            LoadBackground();
            LoadAsset();
            Pools.Instance.Init();
        }

        public void CameraZoomIn()
        {
            camera.DOOrthoSize(8.5f, 1.5f).SetEase(Ease.OutSine);
        }

        public void CameraZoomOut()
        {
            camera.DOOrthoSize(10.5f, 1f).SetEase(Ease.OutSine);
        }
        
        public void LoadStage()
        {
            StartLevel();
        }

        public void EndStage(LevelResult result)
        {
            isPlaying = false;
            if (isRevived || result == LevelResult.Win)
            {
                EndGame(result);
            }
            else
            {
                GameManager.Instance.GamePlayController.OpenUIPlaying(false);
                GameManager.Instance.GamePlayController.OpenUILose(true);
            }
        }

        public void NewPhase()
        {
            GameManager.Instance.GamePlayController.OpenUINewPhase(true);
        }
        
        public int GetNumberOfPhases()
        {
            return levelLoader.phaseList.Count;
        }

        public int GetCurrentPhaseNumber()
        {
            return levelLoader.index;
        }

        public void StoredNumberOfPhase()
        {
            PlayerDataManager.Instance.SetCurrentNumberOfPhase(levelLoader.index);
        }
        
        public int GetTotalNumberOfMonster()
        {
            return levelLoader.TotalNumber();
        }

        private void LoadBackground()
        {
            int randomBackgroundId = Random.Range(1, 4);
            BackgroundInfo backgroundInfo = PlayerDataManager.Instance.backgroundListAsset.backgrounds[randomBackgroundId];
            backGround = Instantiate(backgroundInfo.backgroundPrefab, backgroundSpawnPosition.transform);
            backGround.transform.localPosition = new Vector3(0, -4f, 100f);
            backgroundManager = backGround.GetComponent<BackgroundManager>();
            backgroundSlowdown = backgroundManager.SlowBackAction;
            backgroundSpeedUp = backgroundManager.SpeedUpAction;
        }

        public void LoadCar()
        {
            //Generate Car with highest level Object in PlayingScene
            int carLevel = PlayerDataManager.Instance.GetCarHighestLevel();
            carInfo = PlayerDataManager.Instance.GetCurrentCar(carLevel);
            car = Instantiate(carInfo.car);
            train = car.GetComponent<Train>();
            train.Init();
        }

        public void LoadMainGun()
        {
            //Generate highest level main gun object in car 
            int gunLevel = PlayerDataManager.Instance.GetGunHighestLevel();
            gunInfos = PlayerDataManager.Instance.GetGun(gunLevel);
            mainGun = Instantiate(gunInfos.Gun,car.GetComponent<Train>().gunPosition);
            // mainGun.transform.localPosition = Vector3.zero;
            aimshot = mainGun.GetComponent<AimShoot>();
        }

        public void LoadSupGun()
        {

            //Generate in scene support gun --> need support cat to set parent
            int subGunLevel = PlayerDataManager.Instance.GetSubGunHighestLevel();
            subGunInfo = PlayerDataManager.Instance.GetSubGun(subGunLevel);
            subGun = Instantiate(subGunInfo.subGun);
            subGuns.Add( subGun.GetComponent<SubGun>());
            autoFindTargets.Add(subGun.GetComponent<AutoFindTargets>()); 
        }

        public void LoadMainCat()
        {
            int catID = PlayerDataManager.Instance.GetMainCatChossen();
            cat = Instantiate(PlayerDataManager.Instance.catListAsset.data[catID].Cat,
                train.catPositon);
            cat.GetComponent<MeshRenderer>().sortingLayerID = SortingLayer.NameToID("Cat");
            cat.AddComponent<Cat>();
            cat.GetComponent<Cat>().aimShoot = mainGun.GetComponent<AimShoot>();
            // cat.transform.localPosition = Vector3.zero;
        }
        
        public void LoadSubCat()
        {
            for (int i = 0; i < PlayerDataManager.Instance.GetCarHighestLevel(); i++)
            {
                LoadSupGun();

                int subcatID = Random.Range(0, PlayerDataManager.Instance.GetMaxCatSlotOpen() + 1);
                subCat = Instantiate(PlayerDataManager.Instance.catListAsset.data[subcatID].Cat,
                    train.subCatPosition[i]);
                subCat.AddComponent<SubCat>();
                subCat.GetComponent<MeshRenderer>().sortingLayerID = SortingLayer.NameToID("Cat");
                subCat.GetComponent<SubCat>().init();
                subCat.GetComponent<SubCat>().subGun = subGuns[i];
                subCats.Add( subCat.GetComponent<SubCat>());
                // subCat.transform.localPosition = Vector3.zero;
                
                subGun.transform.SetParent(subCat.GetComponent<SubCat>().subGunPosition);
                subGun.transform.localPosition = Vector3.zero;
            }
            
        }   

        public void LoadDriver()
        {
            int driverID = Random.Range(0, PlayerDataManager.Instance.GetMaxCatSlotOpen() + 1); 
            driver = Instantiate(PlayerDataManager.Instance.catListAsset.data[driverID].Cat,
               train.driver);
            driver.GetComponent<MeshRenderer>().sortingLayerID = SortingLayer.NameToID("Driver");
            driver.AddComponent<Cat>();
            driver.GetComponent<Cat>().aimShoot = mainGun.GetComponent<AimShoot>();
        }
        
        public void LoadAsset()
        {
            autoFindTargets.Clear();
            subCats.Clear();
            subGuns.Clear();
            
            LoadCar();

            LoadMainGun();

            LoadMainCat();

            LoadSubCat();

            LoadDriver();
        }

        public void DestroyAsset()
        {
            Destroy(car);
            Destroy(mainGun);
            Destroy(subGun);
            Destroy(cat);
            Destroy(subCat);
            Destroy(driver);
        }

        public void Revive()
        {
            isImmortal = true;
            isRevived = true;
            isPlaying = true;
            train.life = 1;
            Time.timeScale = 1f;
            car.GetComponent<Train>().HP = hpOnRevive;
            car.GetComponent<Train>().immortalEffect.SetActive(true);
            StartCoroutine(ImmortalDuration());
        }

        private IEnumerator ImmortalDuration()
        {
            yield return new WaitForSecondsRealtime(5f);
            isImmortal = false;
            
            car.GetComponent<Train>().immortalEffect.SetActive(false);
        }

        public void EarnCoin()
        {
            PlayerDataManager.Instance.SetCoin(currentCoinEarn);
            GameManager.Instance.GamePlayController.uiPlaying.coinEarned.gameObject.SetActive(true);
        }

        /// <summary>
        /// INGAME UPDATES
        /// </summary>
        public override void OnIngameUpdate()
        {
            base.OnIngameUpdate();
            
            // levelLoader.OnUpdate();
            cat.GetComponent<Cat>().OnUpdate();
            driver.GetComponent<Cat>().OnUpdate();
            train.OnUpdate();
            // train.OnUpdate();
            foreach (var _subCat in subCats)
            {
                _subCat.OnUpdate();

            }
        }

        public override void OnIngameFixUpdate()
        {
            //TODO:
         
            //Gun, Car update
            base.OnIngameFixUpdate();
            aimshot.OnUpdate();
            backgroundManager.OnFixedUpdate();
            
            foreach (var autoFindTarget in autoFindTargets)
            {
                autoFindTarget.OnUpdate();
            }
            foreach (var subGun in subGuns)
            {
                subGun.OnUpdate();
            }
        }


        private void setUpBaseCoin()
        {
            if (PlayerPrefs.GetInt("Stage") <= 30)
            {
                baseCoin = 1000 * (PlayerPrefs.GetInt("Stage")+1);
                maxCoinEarn = baseCoin + 200 * (PlayerPrefs.GetInt("Stage")+1);
                minCoinEarn = baseCoin - 50 * (PlayerPrefs.GetInt("Stage") + 1);
            }
            else
            {
                baseCoin = 2000 * PlayerPrefs.GetInt("Stage");
                maxCoinEarn = baseCoin + 300 * (PlayerPrefs.GetInt("Stage") + 1);
                minCoinEarn = baseCoin - 100 * (PlayerPrefs.GetInt("Stage") + 1);
            }

            totalCoinEarn = Random.Range(minCoinEarn, maxCoinEarn);
            currentCoinEarn = totalCoinEarn / GetNumberOfPhases();
        }

        public override void StartLevel()
        {
            levelLoader.init();
            setUpBaseCoin();
        }

        public void LoadPhase()
        {
            isPlaying = true;
            isBackGoundMoving = true;
            isCarMoving = true;
            CameraZoomOut();
            levelLoader.LoadGame();
            if(isActiveBuffAtk)
                buffAttackAction?.Invoke();
            if(isActiveBuffFireRate)
                buffFireRateAction?.Invoke();
        }
        
        public void CheckEndPhase()
        {
            enemiesKilledCount++;
            totalEnemiesKilledCount++;
            if (enemiesKilledCount == currentEnemies.Count)
            {
                //End Phase
                isPlaying = false;
                isPhaseEnd = true;
                enemiesKilledCount = 0;
                CheckEndStage();
            }
        }

        private void CheckEndStage()
        {
            //Stage end --> show UI win lose
            if (isPhaseEnd && isStageEnd)
            {
                //TODO: EndGame 
                levelLoader.StageFinish();
            }
            else
            {
                levelLoader.PhaseFinish();
            }
        }

        public void ShakeCamera()
        {
            _camera.transform.DOShakePosition(0.2f, 2f, 10).OnComplete(() =>
            {
                _camera.transform.position = new Vector3(0, 1, -10);
            });
        }
    }
}
