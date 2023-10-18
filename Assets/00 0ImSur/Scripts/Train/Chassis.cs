using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace Unicorn
{
    public class Chassis : MonoBehaviour
    {
        private void Start()
        {
            transform.DOMoveY(transform.position.y + 0.3f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);

        }
        
    }
}
