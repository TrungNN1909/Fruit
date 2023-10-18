using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unicorn.UI;
using Unicorn.Utilities;

namespace Unicorn
{
    public class UpgradeScreen : MonoBehaviour
    {
        [SerializeField] private Button backgroundButton;
        [SerializeField] private Button backHomeButton;
        [SerializeField] private Button buyGunButton;
        [SerializeField] public Button upgradeButton;
        [SerializeField] private Button giftGunButton;
        [SerializeField] private TextMeshProUGUI coinText;
        [SerializeField] private TextMeshProUGUI numberOfCGiftText;
        [SerializeField] private GameObject WatchVideo;
        [SerializeField] private GameObject BuyGun;
        
        [SerializeField] private GameObject tab;
        
        [SerializeField] public GameObject tabGunManager;
        [SerializeField] private GameObject tabCarManager;
        
        
        private bool isClosing;

        private bool isclicked;
        //tutorial
        [SerializeField] public GameObject TabGunTutorial;
        [SerializeField] public GameObject TabCarTutorial;
        [SerializeField] public GameObject TabCarTurotialBack2;
        [SerializeField] public GameObject handCloseTutorial;
        [SerializeField] public GameObject textGunTorial;
        [SerializeField] public GameObject textCarTutorial;
        [SerializeField] public GameObject handMerge;
        [SerializeField] public Image cover;
        
        private void Start()
        {
            backgroundButton.onClick.AddListener(BackgroundButtonOnClick);
            buyGunButton.onClick.AddListener(BuyGunButtonOnClick);
            upgradeButton.onClick.AddListener(UpgradeCarButtonOnClick);
            backHomeButton.onClick.AddListener(BackHomeButtonOnClick);
            giftGunButton.onClick.AddListener(GiftGunButtonOnClick);
        }

        public void OnStartBackTutorial()
        {
            textGunTorial.SetActive(false);
            TabGunTutorial.SetActive(true);
            TabCarTutorial.SetActive(true);
            handCloseTutorial.SetActive(true);
        }
        
        private void BackHomeButtonOnClick()
        {
            //tutorial
            if (PlayerPrefs.GetInt("Tutorial") == 6)
            {
                TabGunTutorial.SetActive(false);
                TabCarTutorial.SetActive(false);
                handCloseTutorial.SetActive(false);
                PlayerPrefs.SetInt("Tutorial", 7);
            }
            
            tab.transform.DOScale(0.3f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                // tab.gameObject.transform.localScale = Vector3.one;
                gameObject.SetActive(false);
            });
        }

        private void UpgradeCarButtonOnClick()
        {
            if(isclicked) return;
            if(tabCarManager.GetComponent<TabCarManager>().isUpgrading) return;
            //tutorial
            if (PlayerPrefs.GetInt("Tutorial", 0) == 1)
            {
                isclicked = true;
                cover.gameObject.SetActive(true);
                upgradeButton.interactable = false;
                tabCarManager.GetComponent<TabCarManager>().hand.SetActive(false);
                StartCoroutine(waitToUpgradeCarAnimator());
            }
            
            if (PlayerDataManager.Instance.GetCoin() >= tabCarManager.GetComponent<TabCarManager>().currentCostConsume)
            {
                tabCarManager.GetComponent<TabCarManager>().UpgradeCar();
                CheckMaxCarLevel(tabCarManager.GetComponent<TabCarManager>().carLevel);
                DisplayCoin();
                SoundManager.Instance.PlayUpgradeCarSound();
            }
            else
            {
                PopupDialogCanvas.Instance.Show("Not enough coin");
                Debug.Log("not enough Coin");
            }
        }

        private IEnumerator waitToUpgradeCarAnimator()
        {
            yield return Yielders.Get(4.5f);
            PlayerPrefs.SetInt("Tutorial",2);
            isclicked = false;
            cover.gameObject.SetActive(false);
            textCarTutorial.SetActive(false);
            TabGunTutorial.SetActive(false);
            TabCarTutorial.SetActive(true);
            TabCarTurotialBack2.SetActive(false);
            tabGunManager.GetComponent<TabGunManager>().handBuy.SetActive(true);
            tabGunManager.GetComponent<TabGunManager>().backTabGun.SetActive(true);
            
        }

        private void BuyGunButtonOnClick()
        {
            if(PlayerDataManager.Instance.GetCoin() >= 1000)
            {
                PlayerDataManager.Instance.SetCoin(-1000);
                GiveGunBuy();
            }
            else
            {
                UnicornAdManager.ShowAdsReward(GiveGunBuy, Helper.buyGunAd);
            }
            
            CheckMoney();
            
            //tutorial
            if(PlayerPrefs.GetInt("Tutorial") <5){}
            {
                PlayerPrefs.SetInt("Tutorial",PlayerPrefs.GetInt("Tutorial")+1);
                tabGunManager.GetComponent<TabGunManager>().handBuy.transform.localScale = Vector3.one;
                tabGunManager.GetComponent<TabGunManager>().handBuy.transform.DOScale(0.5f, 0.2f)
                    .SetLoops(1,LoopType.Yoyo).OnComplete(() =>
                    {
                        tabGunManager.GetComponent<TabGunManager>().handBuy.transform.localScale = Vector3.one;
                    });
            }

            if (PlayerPrefs.GetInt("Tutorial") == 5)
            {
                tabGunManager.GetComponent<TabGunManager>().tutorial.SetActive(true);
                tabGunManager.GetComponent<TabGunManager>().handBuy.SetActive(false);
                tabGunManager.GetComponent<TabGunManager>().OnActionTutorial();
            }
        }

        private void GiveGunBuy()
        {
            GunSlot slot = tabGunManager.GetComponent<TabGunManager>().GetSlot();
            tabGunManager.GetComponent<TabGunManager>().BuyGun(slot);
            DisplayCoin();
        }
        
        private void BackgroundButtonOnClick()
        {
            if(isClosing) return;
            
            isClosing = true;
            tab.transform.DOScale(0.3f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                // tab.gameObject.transform.localScale = Vector3.one;
                gameObject.SetActive(false);
            });
        }

        public void DisplayCoin()
        {
            coinText.text = PlayerDataManager.Instance.GetCoin().ToString();
        }

        private void OnEnable()
        {
            if (PlayerPrefs.GetInt("Tutorial", 0) == 1)
            {
                //
                TabCarTutorial.SetActive(false);
                tabCarManager.GetComponent<TabCarManager>().hand.SetActive(true);
                TabCarTurotialBack2.SetActive(true);
                TabGunTutorial.SetActive(true);
            }

            isClosing = false;
            CheckMoney();
            GameManager.Instance.HomeController.uiHome.HomeUIDisPlay(false);
            DisplayCoin();
            CheckGift();
        }
        private void OnDisable()
        {
            
            GameManager.Instance.HomeController.uiHome.HomeUIDisPlay(true);
            
        }
        private void CheckGift()
        {
            int giftCount = PlayerDataManager.Instance.GetTotalGiftGunStored();

            if (giftCount != 0)
            {
                giftGunButton.gameObject.SetActive(true);
                numberOfCGiftText.text = giftCount.ToString();
            }
            else
            {
                giftGunButton.gameObject.SetActive(false);

            }
        }

        private void CheckMaxCarLevel(int Carlevel)
        {
            if (Carlevel == 4)
            {
                upgradeButton.interactable = false;
                upgradeButton.gameObject.GetComponent<Image>().raycastTarget = false;
                upgradeButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "MAX";
            }
        }

        private void GiftGunButtonOnClick()
        {

            GunSlot slot = tabGunManager.GetComponent<TabGunManager>().GetSlot();
            int lv = PlayerDataManager.Instance.GetGiftGunLevel(PlayerDataManager.Instance.GetGiftIndex());
            tabGunManager.GetComponent<TabGunManager>().SpawnItem(slot, EGunType.SUB_GUN, lv);
            PlayerDataManager.Instance.SetTotalGiftGunStored(-1);
            PlayerDataManager.Instance.SetGiftIndex(-1);
            CheckGift();
        }

        private void CheckMoney()
        {
            if (PlayerDataManager.Instance.GetCoin() >= 1000)
            {
                BuyGun.SetActive(true);
                WatchVideo.SetActive(false);
            }
            else
            {
                BuyGun.SetActive(false);
                WatchVideo.SetActive(true);
            }
        }

        
        
    }
}
