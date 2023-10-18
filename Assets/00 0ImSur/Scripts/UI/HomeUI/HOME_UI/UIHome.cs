using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Unicorn
{
    public class UIHome : UICanvas
    {
        [SerializeField] private Button touchToPlayBtn;
        [SerializeField] private Button settingBtn;
        [SerializeField] private Button shopBtn;
        [SerializeField] private Button upgradeBtn;
        [SerializeField] private Button archivementBtn;
        [SerializeField] private Button giftBtn;
        [SerializeField] private Button shopCatBtn;

        [SerializeField] private TextMeshProUGUI currentStageText;
        [SerializeField] private TextMeshProUGUI nextStageText;
        [SerializeField] private TextMeshProUGUI coinText;

        [SerializeField] private GameObject settingScreen;
        [SerializeField] public GameObject giftScreen;
        [SerializeField] public GameObject ArchivementScreen;
        [SerializeField] public GameObject upgradeScreen;
        [SerializeField] private GameObject shopCatScreen;

        [SerializeField] private GameObject completePhase1;
        [SerializeField] private GameObject completePhase2;
        [SerializeField] private GameObject completePhase3;
        
        [SerializeField] public GameObject HomeUI;

        [SerializeField] private GameObject handTutorial;
        
        public override void Show(bool _isShown, bool isHideMain = true)
        {
            base.Show(_isShown, isHideMain);
        }


        private void Start()
        {
            touchToPlayBtn.onClick.AddListener(PlayGame);
            settingBtn.onClick.AddListener(SettingButtonOnCLick);
            shopBtn.onClick.AddListener(ShopButtonOnCLick);
            upgradeBtn.onClick.AddListener(UpgradeButtonOnClick);
            archivementBtn.onClick.AddListener(ArchivementButtonOnClick);
            giftBtn.onClick.AddListener(GiftButtonOnClick);
            shopCatBtn.onClick.AddListener(ShopCatButtonOnClick);
        }

        private void OnEnable()
        {
            SetText();
            DisplayCurrentPhase();

        }

        public void DisplayCurrentPhase()
        {
            int current = PlayerDataManager.Instance.GetCurrentNumberOfPhase();
            switch (current)
            {
                case 0:
                    completePhase1.SetActive(true);
                    completePhase2.SetActive(false);
                    completePhase3.SetActive(false);
                    break;
                case 1:
                    completePhase1.SetActive(true);
                    completePhase2.SetActive(true);
                    completePhase3.SetActive(false);
                    break;
                case 2:
                    completePhase1.SetActive(true);
                    completePhase2.SetActive(true);
                    completePhase3.SetActive(true);
                    break;
                default:
                    completePhase1.SetActive(false);
                    completePhase2.SetActive(false);
                    completePhase3.SetActive(false);
                    break;
            }
        }

        private void ShopCatButtonOnClick()
        {
            shopCatScreen.SetActive(true);
        }

        private void GiftButtonOnClick()
        {
            giftScreen.SetActive(true);
        }

        private void ArchivementButtonOnClick()
        {
            ArchivementScreen.SetActive(true);
        }

        private void UpgradeButtonOnClick()
        {
            upgradeScreen.SetActive(true);
        }

        private void ShopButtonOnCLick()
        {
        }

        private void SettingButtonOnCLick()
        {
            settingScreen.SetActive(true);
        }

        //chang to state ingame
        public void PlayGame()
        {
            if(PlayerPrefs.GetInt("Tutorial") == 7)
                PlayerPrefs.SetInt("Tutorial",8);
            handTutorial.SetActive(false);
            GameManager.Instance.StartLevel();
            PlayingManager.Instance.LoadStage();
        }


        public void SetText()
        {
            currentStageText.text = (PlayerPrefs.GetInt("Stage")+1).ToString();
            nextStageText.text = (PlayerPrefs.GetInt("Stage") + 2).ToString();
            coinText.text = PlayerPrefs.GetInt("Coin").ToString();
        }

        public void OnUpdate()
        {
            DisplayCoin();
        }

        public void DisplayCoin()
        {
            coinText.text = PlayerDataManager.Instance.GetCoin().ToString();
        }

        public void HomeUIDisPlay(bool isShow)
        {
            if (isShow)
            {
                HomeUI.SetActive(true);
                if (PlayerPrefs.GetInt("Tutorial")== 7)
                {
                    handTutorial.SetActive(true);
                }
            }
            else
                HomeUI.SetActive(false);
        }
    }
}
