using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unicorn.UI;
using TMPro;
using DG.Tweening;
using Unicorn.Utilities;

namespace Unicorn
{
    public class UIWin : UICanvas
    {
        public override void Show(bool _isShown, bool isHideMain = true)
        {
            base.Show(_isShown, isHideMain);
        }

        [SerializeField] private Button noThanksBtn;
        [SerializeField] private Button collectBonusBtn;
        [SerializeField] private TextMeshProUGUI totalCoinEarnText;
        [SerializeField] private ForceBar forceBar;
        [SerializeField] private TextMeshProUGUI ButtonCoinText;

        private void Start()
        {
            noThanksBtn.onClick.AddListener(OnClickNoThanksButton);
            collectBonusBtn.onClick.AddListener(OnClickCollectBonusButton);
        }

        private void OnEnable()
        {
            totalCoinEarnText.text = PlayingManager.Instance.currentCoinEarn.ToString();
        }

        private void Update()
        {
            ButtonCoinText.text = (forceBar.GetValue() * PlayingManager.Instance.currentCoinEarn).ToString();
        }

        public void OnClickNoThanksButton()
        {
            
            Time.timeScale = 1;
            UnicornAdManager.ShowInterstitial(Helper.EndStageWinNothanks);
            StartCoroutine(WaitToReTurnHome());

        }

        public void OnClickCollectBonusButton()
        {
            UnicornAdManager.ShowAdsReward(ForceBarReward, Helper.forceBarRewardAd);
        }

        private void ForceBarReward()
        {
            forceBar.StopRunning();
            int coinEarned = forceBar.GetValue() * PlayingManager.Instance.currentCoinEarn - PlayingManager.Instance.currentCoinEarn;
            PlayerDataManager.Instance.SetCoin(coinEarned);
            totalCoinEarnText.text = (forceBar.GetValue() * PlayingManager.Instance.currentCoinEarn).ToString();
            StartCoroutine(WaitToReTurnHome());
            
        }
        
        public IEnumerator WaitToReTurnHome()
        {
            yield return new WaitForSecondsRealtime(1f);
            Show(false);
            //GameManager.Instance.LoadLevel(3);
            LoadingStartManager.Instance.LoadPlayingScreen();
        }
    }
}