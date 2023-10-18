using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Unicorn.UI;

namespace Unicorn
{
    public enum TypeTab
    {
        MAIN,
        SUB,
    }

    public class ShopCatScreen : MonoBehaviour
    {
        [SerializeField] private Button backgroundBtn;
        // [SerializeField] private Button MainCatBtnOn;
        // [SerializeField] private Button MainCatBtnOff;
        // [SerializeField] private Button SubCatBtnOn;
        // [SerializeField] private Button SubCatBtnOff;
        [SerializeField] private Button buyCatBtn;
        [SerializeField] private Button backButton;
        [SerializeField] private TextMeshProUGUI coinText;

        [SerializeField] private GameObject MaxBuyButton;
        [SerializeField] private GameObject tab;
        [SerializeField] private GameObject TabMainCat;
        private bool isClosing;
        public TypeTab type;

        private void Start()
        {
            backgroundBtn.onClick.AddListener(BackgroundButtonOnClick);
            backButton.onClick.AddListener(BackgroundButtonOnClick);
            
            // MainCatBtnOff.onClick.AddListener(MainCatButtonOnClick);
            // SubCatBtnOff.onClick.AddListener(SubCatButtonOnClick);
            
            buyCatBtn.onClick.AddListener(BuyCatButtonOnClick);
            
        }

        private void BuyCatButtonOnClick()
        {
            if (PlayerDataManager.Instance.GetCoin() < 20000)
            {
                PopupDialogCanvas.Instance.Show("Not enough coin");
                return;
            };
            SoundManager.Instance.PlayBuyCatSound();
            TabMainCat.GetComponent<TabMainCat>().Buy();
            PlayerDataManager.Instance.SetCoin(-20000);
            TextDisPlay();
            CheckMaxCatOpened();
        }

        private void OnEnable()
        {
            isClosing = false;
            type = TypeTab.MAIN;
            // SubCatBtnOn.gameObject.SetActive(false);
            // SubCatBtnOff.gameObject.SetActive(true);
            // MainCatBtnOff.gameObject.SetActive(false);
            // MainCatBtnOn.gameObject.SetActive(true);
            TextDisPlay();
            CheckMaxCatOpened();
            TabMainCat.SetActive(true);
            TabMainCat.GetComponent<TabMainCat>().type = type;
            GameManager.Instance.HomeController.uiHome.HomeUIDisPlay(false);
        }

        private void OnDisable()
        {
            GameManager.Instance.HomeController.uiHome.HomeUIDisPlay(true);
        }

        private void TextDisPlay()
        {
            coinText.text = PlayerDataManager.Instance.GetCoin().ToString();
        }

        private void BackgroundButtonOnClick()
        {
            if(isClosing) return;
            
            isClosing = true;
            tab.transform.DOScale(0.3f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                tab.gameObject.transform.localScale = Vector3.one;
                gameObject.SetActive(false);
                TabMainCat.SetActive(false);
            });

        }

   

        private void CheckMaxCatOpened()
        {
            if(PlayerDataManager.Instance.GetMaxCatSlotOpen() >= PlayerDataManager.Instance.catListAsset.data.Count - 1)
            {
                buyCatBtn.gameObject.SetActive(false);
                MaxBuyButton.SetActive(true);
            }
            else
            {
                buyCatBtn.gameObject.SetActive(true);
                MaxBuyButton.SetActive(false);
            }
        }

        // private void ResetPosition()
        // {
        //     MainCatBtnOn.gameObject.transform.localPosition = new Vector3(0, 100, 0);
        //     MainCatBtnOff.gameObject.transform.localPosition = new Vector3(0, 100, 0);
        //     SubCatBtnOn.gameObject.transform.localPosition = new Vector3(0, -100, 0);
        //     SubCatBtnOff.gameObject.transform.localPosition = new Vector3(0, -100, 0);
        // }
    }
}
