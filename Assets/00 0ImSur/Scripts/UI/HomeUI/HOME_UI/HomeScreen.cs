using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class HomeScreen : MonoBehaviour
    {
        [SerializeField] public GameObject upgradeDotCheck;
        [SerializeField] public GameObject archivementDotCheck;
        [SerializeField] public GameObject giftDotCheck;

        private void OnEnable()
        {
            if(PlayerDataManager.Instance.GetStage() == 1) return;
            CheckUpgrade();
            CheckGift();
            CheckArchivement();
        }

        public void CheckUpgrade()
        {
            if (PlayerDataManager.Instance.GetCoin() >= 1000)
            {
                upgradeDotCheck.SetActive(true);
            }
            else
            {
                upgradeDotCheck.SetActive(false);
            }
        }

        public void CheckGift()
        {
            if(GameManager.Instance.uiHomeController.uiHome.giftScreen.GetComponent<GiftScreen>().Check())
            {
                giftDotCheck.SetActive(true);
            }
            else
            {
                giftDotCheck.SetActive(false);
            }
        }

        public void CheckArchivement()
        {
            if (GameManager.Instance.uiHomeController.uiHome.ArchivementScreen.GetComponent<ArchivementScreen>()
                .Check())
            {
                archivementDotCheck.SetActive(true);
            }
            else
            {
                archivementDotCheck.SetActive(false);
            }
        }
    }
}
