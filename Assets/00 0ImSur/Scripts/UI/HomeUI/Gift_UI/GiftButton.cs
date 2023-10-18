using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unicorn.Utilities;
using DG.Tweening;
using System;

namespace Unicorn
{
    public class GiftButton : MonoBehaviour
    {
        [SerializeField] private Button collectBtn;
        [SerializeField] private GameObject lockCardImg;
        [SerializeField] private GameObject collectedCardImg;

        public int cardNumber;
        public TextMeshProUGUI giftText;
        public Image giftImg;
        public int coin;
        public int subgunLevel;
        public bool isTaken;
 


        private void Start()
        {
            collectBtn.onClick.AddListener(CollectButtonOnClick);
        }

        private void OnEnable()
        {
            CheckCardStatus();
        }

        private void CollectButtonOnClick()
        {
            UnicornAdManager.ShowAdsReward(Collect, Helper.DailyGiftAd);
            SoundManager.Instance.PlaySoundReward();
        }

        private void Collect()
        {
            PlayerDataManager.Instance.SetGiftCardTaken(cardNumber, true);
            isTaken = true;
            CheckCardStatus();
            //TODO: take gift;
            TakeGift();
            PlayerDataManager.Instance.SetTimeLoginOpenGift(DateTime.Now.ToString());
        }
        
        private void TakeGift()
        {

            if (coin > 0 )
            {
                PlayerDataManager.Instance.SetCoin(coin);
            }
            if(subgunLevel > 0)
            {
                //Todo: spawn gun in inventory.

                StoredGun();
            }
        
        }

        public void ButtonDayCheck()
        {
            var timeLogin = PlayerDataManager.Instance.GetTimeLoginOpenGift();
            var isNewDay = Helper.CheckNewDay(timeLogin);
            if (isNewDay)
            {
                PlayerDataManager.Instance.SetGiftCardTaken(cardNumber, false);
            }
        }

        private void StoredGun()
        {
            PlayerDataManager.Instance.SetTotalGiftGunStored(1);
            PlayerDataManager.Instance.SetGiftIndex(1);
            PlayerDataManager.Instance.SetGiftGunLevel(PlayerDataManager.Instance.GetGiftIndex(), subgunLevel);
        }

        public void CheckCardStatus()
        {
            isTaken = PlayerDataManager.Instance.GetGiftCardTaken(cardNumber);
            if (isTaken)
            {
                collectedCardImg.SetActive(true);
                lockCardImg.SetActive(true);
            }
            else
            {
                collectedCardImg.SetActive(false);
                lockCardImg.SetActive(false);
            }
        }

        
    }
}
