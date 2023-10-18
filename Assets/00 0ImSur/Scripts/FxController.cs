using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class FxController : MonoBehaviour
    {
        private void OnEnable()
        {
            StartCoroutine(DelayToDisable(delayTime));
        }

        public float delayTime;
        // Start is called before the first frame update
        private IEnumerator DelayToDisable(float deleyTime = 2f)
        {
            yield return Yielders.Get(deleyTime);
            SimplePool.Despawn(gameObject);
        }
    }
}
