using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unicorn.Utilities;

namespace Unicorn
{
    public class MainGunButton : MonoBehaviour
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
            isTaken = PlayerDataManager.Instance.GetMainGunCardTaken(cardNumber);
        }

        private void CollectButtonOnClick()
        {
            SoundManager.Instance.PlaySoundReward();
            UnicornAdManager.ShowAdsReward(Collect, Helper.ArchivementAd);
        }

        private void Collect()
        {
            isTaken = true;
            PlayerDataManager.Instance.SetCoin(coin);
            PlayerDataManager.Instance.SetMainGunCardTaken(cardNumber, isTaken);
        }
        
        private void Update()
        {
            CheckCardStatus();
        }

        private void CheckCardStatus()
        {
            if (PlayerDataManager.Instance.GetGunHighestLevel() >= cardNumber)
            {
                if (isTaken) //earned
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
