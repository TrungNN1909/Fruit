using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unicorn.Utilities;
using UnityEngine.UI;
using TMPro;

namespace Unicorn
{
    public class AddCoin : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI coin;
        

        private void OnEnable()
        {
            int currentCoinEarn = PlayingManager.Instance.currentCoinEarn;
            coin.text = "+ " + currentCoinEarn.ToString();


            coin.DOFade(100f, 1f).SetEase(Ease.OutSine);
            coin.rectTransform.DOLocalMoveY(-100f, 1f).SetEase(Ease.OutSine).OnComplete(() =>
            {
                coin.DOFade(0, 1f).SetDelay(1f);
                coin.rectTransform.DOLocalMoveY(-50f, 1f).SetDelay(1f).SetEase(Ease.InSine).OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    coin.rectTransform.anchoredPosition = new Vector3(0, -150f, 0);
                });
                
            });
        }


        private void OnDisable()
        {
            GameManager.Instance.GamePlayController.uiPlaying.SetCoinText();
        }
    }
}
