using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unicorn.Utilities;

namespace Unicorn
{
    public class SupportGunButton : MonoBehaviour
    {
        [SerializeField] private Button collectBtn;
        [SerializeField] private GameObject lockCardImg;
        [SerializeField] private GameObject collectedCardImg;

        public int cardNumber;
        public TextMeshProUGUI mainText;
        public Image mainImg;
        public int coin;

        public bool isTaken;

        private void Start()
        {
            collectBtn.onClick.AddListener(CollectButtonOnClick);
            isTaken = PlayerDataManager.Instance.GetSuPGunCardTaken(cardNumber);
        }

        private void CollectButtonOnClick()
        {
            UnicornAdManager.ShowAdsReward(Collect, Helper.ArchivementAd);

        }

        private void Collect()
        {
            isTaken = true;

            PlayerDataManager.Instance.SetSupGunCardTaken(cardNumber,isTaken);
            PlayerDataManager.Instance.SetCoin(coin);
        }

        private void Update()
        {
            CheckCardStatus();
        }

        private void CheckCardStatus()
        {
            if (PlayerDataManager.Instance.GetSubGunHighestLevel() >= cardNumber)
            {
                if (isTaken)
                {
                    collectedCardImg.SetActive(true);
                    lockCardImg.SetActive(true);
                }
                else
                {
                    lockCardImg.SetActive(false);

                }
            }
            else
            {
                lockCardImg.SetActive(true);
            }
        }

    }
}
