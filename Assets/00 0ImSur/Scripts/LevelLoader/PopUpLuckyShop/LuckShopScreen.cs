using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Unicorn
{

    public class LuckShopScreen : UICanvas
    {

        [SerializeField] private List<LuckyShopItemButton> items;
        [SerializeField] private Button skipButton;
        [SerializeField] private GameObject tab;
        private bool isClosing;
        
        public override void Show(bool _isShown, bool isHideMain = true)
        {
            base.Show(_isShown, isHideMain);
        }

        private void Start()
        {
            skipButton.onClick.AddListener(SkipButtonOnClick);
        }

        private void OnEnable()
        {
            isClosing = false;
        }

        private void SkipButtonOnClick()
        {
            if(isClosing) return;

            tab.transform.DOScale(0.3f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                GameManager.Instance.GamePlayController.uiNewPhase.EndPhasePower.gameObject.SetActive(true);
                gameObject.SetActive(false);
            });
        }
    }
}
