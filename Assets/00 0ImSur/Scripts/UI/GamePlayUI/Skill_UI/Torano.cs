using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unicorn
{
    public class Torano : Skill
    {
        private float timeToHit = 0.5f;
        private float waitToHit=0f;
        private float duration = 0f;
        public override void OnEnable()
        {
            base.OnEnable();
            StartCoroutine(DelayMateoroid());
            if (PlayerDataManager.Instance.GetStage() == 0)
            {
                atk = 70f;
            }
            else
            {
                atk = PlayingManager.Instance.aimshot.dmg*0.8f;
            }
            SoundManager.Instance.PlaySKillTornadoSound();
        }
        private IEnumerator DelayMateoroid()
        {
            waitToHit = 0f;
            duration = 0f;
            for(int i=0; i<fxPos.Count; i++)
            {
                yield return Yielders.Get(0.5f);
                
                GameObject torano = SimplePool.Spawn(vfx, fxPos[i].position,Quaternion.Euler(-120,0,0));
                vfxList.Add(torano);
                torano.transform.DOMoveY(fxPos[i].transform.position.y + Random.Range(3f, 6f), 1f).SetLoops(3,LoopType.Yoyo);
                OnGivenDame();
            }
        }

        private void Update()
        {
            if (waitToHit > timeToHit && duration <= 3f)
            {
                OnGivenDame();
                waitToHit = 0;
            }

            waitToHit += Time.deltaTime;
            duration += Time.deltaTime;
        }

    }
}
