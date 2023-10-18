using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Events;
using System;
using Unicorn.Utilities;

namespace Unicorn
{
    public class UIPlaying : UICanvas
    {
        [SerializeField] private Slider healthBar;
        [SerializeField] public Slider backHealthBar;
        
        // [SerializeField] private GameObject progress;

        [SerializeField] public Slider progress;
        [SerializeField] public Transform carSpawnPos;
        [SerializeField] public Image carAvatar;
        [SerializeField] private TextMeshProUGUI coinText;
        [SerializeField] private Button settingBtn;
        [SerializeField] private GameObject SettingScreen;
        [SerializeField] private TextMeshProUGUI stageText;
        [SerializeField] private TextMeshProUGUI hpText;
        [SerializeField] public TextMeshProUGUI coinEarned;
        [SerializeField] public SkillsTab skillsTab;
        [SerializeField] public GameObject BossWarningScreen;
        [SerializeField] public GameObject onTakenDmgBack;
        private Train player;
        private float hpDropSpeed;
        private float hpBackDropSpeed;

        private void Start()
        {
            settingBtn.onClick.AddListener(SettingButtonOnCLick);
            carAvatar.sprite = PlayerDataManager.Instance.CarListAsset.data[PlayerDataManager.Instance.GetCarHighestLevel()]
                .avatar;
        }

        public void OpenBossWarning()
        {
            BossWarningScreen.SetActive(true);
        }
        
        private void SettingButtonOnCLick()
        {
            SettingScreen.SetActive(true);
            Time.timeScale = 0;
        }

        public void Update()
        {
            setHP();
            SetProgress();
        }

        private void OnEnable()
        {
            player = PlayingManager.Instance.train;
            Instantiate(PlayingManager.Instance.carInfo.carInTab,carSpawnPos);
            SetText();
            SetStartHp();
            SetNumberOfPhase();
            SetCoinText();
        }

        public void SetText()
        {
            stageText.text = "Stage: " + (PlayerPrefs.GetInt("Stage")+1);
            hpText.text = healthBar.GetComponent<Slider>().maxValue.ToString();
        }

        public void SetCoinText()
        {
            coinText.text = PlayerDataManager.Instance.GetCoin().ToString();
            Helper.SetTextNumber(coinEarned, PlayerDataManager.Instance.GetCoin(), PlayerDataManager.Instance.GetCoin());
        }

        public void SetStartHp()
        {
            if(player.HP < player.maxHP)
            {
                healthBar.maxValue = player.maxHP;
                healthBar.value = player.maxHP;
                
                backHealthBar.maxValue = player.maxHP;
                backHealthBar.value = player.maxHP;
            }
            else
            {
                healthBar.maxValue = player.HP;
                healthBar.value = player.maxHP;

                backHealthBar.maxValue = player.HP;
                backHealthBar.maxValue = player.HP;
            }
            PlayingManager.Instance.hpOnRevive = healthBar.maxValue;
            hpDropSpeed = 0.6f * player.maxHP;
            hpBackDropSpeed = 0.2f * player.maxHP;
        }

        public void setHP()
        {
            healthBar.value = Mathf.MoveTowards(healthBar.value, player.HP,Time.deltaTime * hpDropSpeed);
            backHealthBar.value = Mathf.MoveTowards(backHealthBar.value, player.HP,Time.deltaTime * hpBackDropSpeed);
            hpText.text = ((int) player.HP).ToString();
        }

        public void SetNumberOfPhase()
        {
            progress.maxValue = PlayingManager.Instance.GetTotalNumberOfMonster();
        }

        public void SetProgress()
        {
            progress.value = Mathf.MoveTowards(progress.value, PlayingManager.Instance.totalEnemiesKilledCount,
                Time.deltaTime * 7.5f);
        }

    }
}
