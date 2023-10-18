using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace Unicorn
{
    public class Wheel : MonoBehaviour
    {
        private Tweener tween;
        private Tweener tween2;
        private Tweener tween3;

        private void Start()
        {
            tween2 = transform.DOMoveY(transform.position.y + 0.2f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
            // tween = transform.DORotate(new Vector3(0, 0, 360), 1f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
            tween3 = transform.DOMoveY(transform.position.y + 0.15f, 2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        }


        private void Update()
        {
            if (PlayingManager.Instance.isCarMoving)
            {
                // tween.Play();
                tween2.Play();
                tween3.Pause();

            }
            else
            {
                // tween.Pause();
                tween2.Pause();
                tween3.Play();

            }
        }
    }
}
