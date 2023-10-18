using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using Unity.Mathematics;

namespace Unicorn
{
    public class TabCarManager : MonoBehaviour
    {
        [SerializeField] private GameObject CarPosition;
        [SerializeField] private TextMeshProUGUI amountHPText;
        [SerializeField] private TextMeshProUGUI coinComsume;
        [SerializeField] private Slider progress;
        [SerializeField] private Image currentCar;
        [SerializeField] private Image nextCar;
        [SerializeField] public GameObject hand;
        
        [SerializeField] private GameObject levelUpFx;

        private CarInfo car;
        public int carLevel;
        private float baseHP;
        private float hpIncreasing;
        private int baseCost;
        private int costIncreasing;
        private int timeToNextLevel;
        private int maxTimeToNextLevel;
        private GameObject carInTab;

        public int currentCostConsume;
        private float currentHP;
        private int currentTimeToNextLevel;

        private Train carObject;

        public bool isUpgrading;
        
        private void OnEnable()
        {
            Init();
            isUpgrading = false;
            if (PlayerPrefs.GetInt("Tutorial") == 1)
            {
                //HandClick active
                hand.SetActive(true);
            }
            else
            {
                hand.SetActive(false);
            }
        }

        public void Init()
        {
            carLevel = PlayerDataManager.Instance.GetCarHighestLevel();
            int carNextLevel = Mathf.Clamp(PlayerDataManager.Instance.GetCarHighestLevel() + 1, 0, 4);
            currentCar.sprite = PlayerDataManager.Instance.CarListAsset.data[carLevel].avatar;
            nextCar.sprite = PlayerDataManager.Instance.CarListAsset.data[carNextLevel].avatar;
            
            car = PlayerDataManager.Instance.GetCurrentCar(carLevel);
            carObject = PlayingManager.Instance.train;
            
            Debug.Log(carLevel);
            
            carInTab = Instantiate(car.carInTab, CarPosition.transform);
            carInTab.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            carInTab.transform.DOScale(1f, 1.5f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                GameManager.Instance.HomeController.uiHome.upgradeScreen.GetComponent<UpgradeScreen>().upgradeButton
                    .interactable = true;
                isUpgrading = false;
            });
            
            
            baseHP = car.baseHP; //BaseHP each level, it's obviously higher when max updated of previous level
            hpIncreasing = car.amountIncreaseHP; // amount of hp will be added to base hp each level
            maxTimeToNextLevel = car.timesToNextLevel; //maximun of max upgrade each level (diffirent values)
            timeToNextLevel = PlayerDataManager.Instance.GetTimeToNextLevel(); // number of times upgraded (current)
            baseCost = car.baseCost;
            costIncreasing = car.costIncreasing;

            currentTimeToNextLevel = PlayerDataManager.Instance.GetTimeToNextLevel();
            currentHP = baseHP + timeToNextLevel * hpIncreasing;
            currentCostConsume = baseCost + timeToNextLevel * costIncreasing;

            progress.maxValue = maxTimeToNextLevel;
            progress.value = timeToNextLevel;

            carObject.Init();

            SetText();

        }

        public void SetText()
        {
            amountHPText.text = currentHP.ToString();
            coinComsume.text = currentCostConsume.ToString();
        }

        public void UpgradeCar()
        {
            //TODO: Upgrade on Click

                PlayerDataManager.Instance.SetTimeToNextLevel(1);
                PlayerDataManager.Instance.SetCoin(-currentCostConsume);

                currentTimeToNextLevel = PlayerDataManager.Instance.GetTimeToNextLevel();
                
                //1. hp
                currentHP = baseHP + currentTimeToNextLevel * hpIncreasing;
                
                //2. cost
                currentCostConsume = baseCost + currentTimeToNextLevel * costIncreasing;

                SetText();

                //3. slider
                progress.value = currentTimeToNextLevel + 1;
                CheckUpdate();
            
        }

        private void OnDisable()
        {
            Destroy(carInTab);
        }

        private void CheckUpdate()
        {
            if(isUpgrading) return;
            if(currentTimeToNextLevel == maxTimeToNextLevel)
            {
                isUpgrading = true;
                GameManager.Instance.HomeController.uiHome.upgradeScreen.GetComponent<UpgradeScreen>().upgradeButton
                    .interactable = false;
                PlayerDataManager.Instance.SetCarHighestLevel(carLevel + 1);
                PlayerDataManager.Instance.SetTimeToNextLevel(-maxTimeToNextLevel);
                PlayingManager.Instance.DestroyAsset();
                PlayingManager.Instance.LoadAsset();
                GameObject levelUpObj = SimplePool.Spawn(levelUpFx);
                levelUpObj.transform.SetParent(CarPosition.transform);
                levelUpObj.transform.localPosition = new Vector3(0,-300f,0);
                
                carInTab.transform.DOShakeRotation(2f, new Vector3(0, 0, 30f), 40, 34).SetEase(Ease.InOutCubic)
                    .OnComplete(() =>
                    {
                        carInTab.transform.DOScale(0.3f, 1f).SetEase(Ease.InBack).OnComplete(() =>
                        {
                            SoundManager.Instance.PlayRewardSound();
                            Destroy(carInTab);
                            Init();
                        });
                    });
            }
        }
    }
}
