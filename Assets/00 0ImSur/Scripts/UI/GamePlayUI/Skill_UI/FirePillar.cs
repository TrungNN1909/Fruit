using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Unicorn
{
    public class FirePillar : Skill
    {
        private float timeToHit = 0.2f;
        private float waitToHit=0.1f;
        private float duration = 0f;
        public override void OnEnable()
        {
            base.OnEnable();
            timeToHit = 0.2f;
            waitToHit=0.1f;
            duration = 0f;
            SoundManager.Instance.PlaySkillFirePillar();
            foreach (Transform pos in fxPos)
            {
                vfxList.Add(SimplePool.Spawn(vfx, pos.position,Quaternion.Euler(-90,0,0) ));
            }

            atk = PlayingManager.Instance.aimshot.dmg*0.6f;
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

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("hahaha");
            }
        }
    }
}
