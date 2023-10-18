using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Unicorn
{
    public class UIConfetti : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _group;
        [SerializeField] private Image textImg;
        [SerializeField] private List<Sprite> textSprites;

        private void OnEnable()
        {
            RandomCongraText();
            StartCoroutine(DelayToDisable());
        }

        private IEnumerator DelayToDisable()
        {
            yield return Yielders.Get(2f);
            _group.DOFade(0, 0.7f).OnComplete(() =>
            {
                _group.alpha = 1;
                gameObject.SetActive(false);
            });
        }

        private void RandomCongraText()
        {
            int i = Random.Range(0, 6);

            textImg.sprite = textSprites[i];
            textImg.SetNativeSize();
            // switch (i)
            // {
            //     case 0:
            //         break;
            //     case 1:
            //         textImg.sprite = "Perfect";
            //         break;
            //     case 2:
            //         congraText.text = "Wonderful";
            //         break;
            //     case 3:
            //         congraText.text = "Amazing";
            //         break;
            //     case 4:
            //         congraText.text = "Hooray";
            //         break;
            //     case 5:
            //         congraText.text = "Bravo";
            //         break;
            // }
        }
    }
}
