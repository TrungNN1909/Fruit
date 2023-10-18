using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Unicorn
{
    public class Garage : MonoBehaviour
    {
        public float speed = 10;
        public bool isMovingIn;
        private void Start()
        {
            PlayingManager.Instance.garageMoveInAction = DOTMoveIn;
            PlayingManager.Instance.garageMoveOutAction = DOTMoveOut;
        }

        private void DOTMoveOut(float x)
        {
            isMovingIn = false;
            transform.DOMoveX(x, 2f).SetEase(Ease.InSine).OnComplete(() =>
            {
                if (!PlayingManager.Instance.isStageEnd)
                    transform.position = new Vector3(-45f, 4.75f, 100f);
            });
        }
        
        private void Move()
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }

        private void FixedUpdate()
        {
            if (isMovingIn && PlayingManager.Instance.isBackGoundMoving)
            {
                Move();
                if (transform.position.x >= 40f) isMovingIn = false;
            }
        }

        private void DOTMoveIn()
        {
            isMovingIn = true;
            transform.position = new Vector3(-45f, 4.75f, 100f);
        }
    }
}
