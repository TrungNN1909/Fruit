using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Unicorn
{
    public class OnTakenDmg : MonoBehaviour
    {
        [SerializeField] public Image hitBackground;
        // Start is called before the first frame update
        private void OnEnable()
        {
            StartCoroutine(DelayToDisable());
        }

        private IEnumerator DelayToDisable()
        {
            yield return Yielders.Get(0.3f);

            hitBackground.DOFade(0,0.2f).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }

        private void OnDisable()
        {
            hitBackground.color = new Color(255, 255, 255, 255);
        }
    }
}
