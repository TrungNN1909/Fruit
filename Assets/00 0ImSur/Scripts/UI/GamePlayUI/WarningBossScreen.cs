using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


namespace Unicorn
{
    public class WarningBossScreen : MonoBehaviour
    {
        [SerializeField] private Image rectangle;
        [SerializeField] private Image topLine;
        [SerializeField] private Image botLine;
        [SerializeField] private CanvasGroup group;

        private Tween recFade;
        private Tween recScale;
        private void OnEnable()
        {
            RectangleBlink();
            TopLineMove();
            BotLineMove();
            StartCoroutine(DelayToFadeOut());
        }

        private void RectangleBlink()
        {
            recScale = rectangle.transform.DOScale(1.2f, 1f).SetEase(Ease.InOutBack).SetLoops(-1, LoopType.Yoyo);
        }

        private void TopLineMove()
        {
            topLine.transform.DOLocalMoveX(1000f, 5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                topLine.transform.localPosition = new Vector3(0, 335.232f, 0);
            });
        }

        private void BotLineMove()
        {
            botLine.transform.DOLocalMoveX(-1000f, 5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                botLine.transform.localPosition = new Vector3(0, -335.232f, 0);
            });

        }

        private IEnumerator DelayToFadeOut()
        {
            yield return Yielders.Get(2f);

            group.DOFade(0, 1.5f).OnComplete(() => { gameObject.SetActive(false); });
        }

        private void OnDisable()
        {
            // recFade.Kill();
            recScale.Kill();
            rectangle.transform.localScale = Vector3.one;
            group.alpha = 1;
        }
    }
}
