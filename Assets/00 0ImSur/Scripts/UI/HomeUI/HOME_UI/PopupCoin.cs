using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unicorn.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Unicorn
{
    public class PopupCoin : UICanvas
    {
        [SerializeField] private Button watchButton;
        [SerializeField] private Button backgroundButton;
        [SerializeField] private GameObject tab;
        [SerializeField] private GameObject lockImg;
        [SerializeField] private GameObject donePerchase;
         
        private bool isClosing;
        
        private void Start()
        {
            isClosing = false;
            watchButton.onClick.AddListener(Onclick);
            backgroundButton.onClick.AddListener(BackgroundButtonOClick);
            donePerchase.SetActive(false);
            lockImg.SetActive(false);

        }

        public override void Show(bool _isShown, bool isHideMain = true)
        {
            base.Show(_isShown, isHideMain);
          
        }

        private void OnEnable()
        {
            isShow = true;
            GameManager.Instance.uiGamePlayController.uiNewPhase.LuckShopScreen.gameObject.SetActive(false);
        }

        private void BackgroundButtonOClick()
        {
            if(isClosing) return;
            
            isClosing = true;
            tab.transform.DOScale(0.3f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                Show(false);
            });
        }

        private void Onclick()
        {
            if(isClosing) return;
            UnicornAdManager.ShowAdsReward(RewardOnClick, Helper.PopupCoin);
        }

        private void RewardOnClick()
        {
            PlayerDataManager.Instance.SetCoin(15000);
            PlayerDataManager.Instance.SetTimeEarnPopUoCoin();
            isClosing = true;
            donePerchase.SetActive(true);
            lockImg.SetActive(true);

            tab.transform.DOScale(0.3f, 0.5f).SetDelay(0.75f).SetEase(Ease.InBack).OnComplete(() =>
            {
                this.Show(false);
                GameManager.Instance.uiGamePlayController.uiNewPhase.LuckShopScreen.gameObject.SetActive(true);

            });
        }
    }
}
