using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class BackgroundManager : MonoBehaviour
    {
        [SerializeField] public BackgroundRender ground;
        [SerializeField] public BackgroundRender semiBack;
        [SerializeField] public BackgroundRender back;

        private float groundSpeed;
        private float semiBackSpeed;
        private float backSpeed;

        private bool isSpeedup;
        private bool isSlowDown;
        
        private void Start()
        {
            groundSpeed = ground.speed;
            semiBackSpeed = semiBack.speed;
            backSpeed = back.speed;
        }


        public void OnFixedUpdate()
        {
            if(isSpeedup)
                ground.speed = Mathf.MoveTowards(ground.speed, 50f, 15f * Time.deltaTime);
            
            if(isSlowDown)
                ground.speed = Mathf.MoveTowards(ground.speed, 10f, 25f * Time.deltaTime);

        }

        public void SlowBackAction()
        {
            isSlowDown = true;
            isSpeedup = false;
            semiBack.speed = 1;
            back.speed = 0;
            // StartCoroutine(DelayResetSpeed());
        }

        public void SpeedUpAction()
        {
            isSpeedup = true;
            isSlowDown = false;
            semiBack.speed = semiBackSpeed;
            back.speed = backSpeed;
        }
        
        private IEnumerator DelayResetSpeed()
        {
            yield return Yielders.Get(5f);
            
            ground.speed = groundSpeed;
            semiBack.speed = semiBackSpeed;
            back.speed = backSpeed;
        }
        
    }
}
