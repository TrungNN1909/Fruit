using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unicorn
{
    public class GasStation : MonoBehaviour
    {
        public bool isMovingIn;
        public float speed = 10f;
        
        [SerializeField] private List<Sprite> restAreas;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private void Start()
        {
            PlayingManager.Instance.gasStationMoveInAction = DOTMoveIn;
            PlayingManager.Instance.gasStationMoveOutAction = DOTMoveOut;
        }

        private void DOTMoveOut(float x)
        {
            isMovingIn = false;
            transform.DOMoveX(x, 2f).SetEase(Ease.InSine)
                .OnComplete(() =>
            {
                if (!PlayingManager.Instance.isPhaseEnd)
                {
                    transform.position = new Vector3(-40f, 3f, 100f);
                }
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
            }
        }
        
        private void DOTMoveIn()
        {
            isMovingIn = true;
            _spriteRenderer.sprite = restAreas[Random.Range(0, restAreas.Count)];
            gameObject.transform.position = new Vector3(-40f, 3f, 100f);
            
        }
      
    }
}
