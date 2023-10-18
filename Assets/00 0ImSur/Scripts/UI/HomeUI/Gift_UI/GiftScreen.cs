using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using DG.Tweening;
using Unicorn.Utilities;

namespace Unicorn
{
    public class GiftScreen : MonoBehaviour
    {
        [SerializeField] private Transform giftsCardPositon;
        [SerializeField] private GameObject giftCard;
        [SerializeField] private Button backgroundButton;
        [SerializeField] private Button backButton;
        [SerializeField] private GameObject tab;
        [SerializeField] private ScrollRect _scrollRect;
        public List<GiftInfo> giftInfos;

        public bool isLoaded = true;

        private bool isClosing;
        
        private void Start()
        {
            backgroundButton.onClick.AddListener(BackgroundButtonOnClick);
            backButton.onClick.AddListener(BackgroundButtonOnClick);
        }

        private void BackgroundButtonOnClick()
        {
            if(isClosing) return;
            isClosing = true;
            
            tab.transform.DOScale(0.3f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                tab.gameObject.transform.localScale = Vector3.one;
                gameObject.SetActive(false);
            });
        }

        private void OnEnable()
        {
            Init();
            if (isLoaded)
                LoadCard();
            isLoaded = false;
            isClosing = false;
            GameManager.Instance.HomeController.uiHome.HomeUIDisPlay(false);
            StartCoroutine(DelayForScrollRect());
        }

        public void Init()
        {
            giftInfos = PlayerDataManager.Instance.giftListAsset.data;
        }
        
        private IEnumerator DelayForScrollRect()
        {            
            _scrollRect.horizontal = false;
            yield return Yielders.Get(1f);
            _scrollRect.horizontal = true;
        }
        private void OnDisable()
        {
            GameManager.Instance.HomeController.uiHome.HomeUIDisPlay(true);
        }
        private void LoadCard()
        {
            foreach (GiftInfo gi in giftInfos)
            {
                GameObject card = Instantiate(giftCard, giftsCardPositon);
                card.GetComponent<GiftButton>().giftText.text = gi.textGift;
                card.GetComponent<GiftButton>().giftImg.sprite = gi.spriteGift;
                card.GetComponent<GiftButton>().cardNumber = gi.giftNumber;
                card.GetComponent<GiftButton>().coin = gi.coin;
                card.GetComponent<GiftButton>().subgunLevel = gi.subGunLv;
                card.GetComponent<GiftButton>().ButtonDayCheck();
                card.GetComponent<GiftButton>().CheckCardStatus();
            }
        }

        public bool Check()
        {
            if (NewDayDotCheck())
            {
                return true;
            }
            
            foreach (GiftInfo gi in giftInfos)
            {
                if (!PlayerDataManager.Instance.GetGiftCardTaken(gi.giftNumber))
                {
                    return true;
                }
            }
            return false;
        }

        public bool NewDayDotCheck()
        {
            var timeLogin = PlayerDataManager.Instance.GetTimeLoginOpenGift();
            var isNewDay = Helper.CheckNewDay(timeLogin);
            if (isNewDay)
            {
                return true;
            }

            return false;
        }
    }
}