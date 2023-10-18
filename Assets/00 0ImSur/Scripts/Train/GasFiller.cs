using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Unicorn
{
    public class GasFiller : MonoBehaviour
    {
        private void Start()
        {
            PlayingManager.Instance.gasFillerCrossOverAction = Move;
        }

        public void Move()
        {
            transform.DOMoveX(55f, 3.5f).OnStart(() =>
            {
                transform.position = new Vector3(-40, 0, 0);
            }).OnComplete(() =>
            {
                transform.position = new Vector3(-40, 0, 0);
            });
        }
    }
}
