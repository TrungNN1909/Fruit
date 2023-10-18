using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using UnityEngine;

namespace Unicorn
{
    public class Crosshair : MonoBehaviour
    {
        private float maxLimitX;
        private Vector3 mousePos;
       

        private void OnEnable()
        {
            transform.SetParent(PlayingManager.Instance.aimshot.transform.parent);
        }

        public void Update()
        {
            CrossHair();
        }

        private void CrossHair()
        {
            
            mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            maxLimitX = gameObject.transform.parent.position.x + 2f;

            if (mousePos.x >= maxLimitX)
            {
                transform.position = mousePos;
            }
        }

    }
}


