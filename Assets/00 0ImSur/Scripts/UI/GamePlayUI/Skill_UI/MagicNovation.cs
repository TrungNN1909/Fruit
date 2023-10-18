using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Unicorn
{
    public class MagicNovation : Skill
    {
        private List<GameObject> charge;
        [SerializeField] private GameObject explosion;
        [SerializeField] private GameObject core;
        public override void OnEnable()
        {
            
            StartCoroutine(Explore());
            base.OnEnable();
            
            for (int i = 0; i < 3; i++)
            {
                SimplePool.Spawn(vfx, fxPos[0].position, quaternion.identity); 
            }  
            SimplePool.Spawn(core, fxPos[0].position, Quaternion.Euler(-90,90,0));
        }

        public IEnumerator Explore()
        {            
            atk = PlayingManager.Instance.aimshot.dmg * 1.5f;
            yield return Yielders.Get(1.5f);
            vfxList.Add(SimplePool.Spawn(explosion,fxPos[0].position,quaternion.identity));
            SoundManager.Instance.PlayMagicNovationImpact();
            OnGivenDame();
            
            atk = PlayingManager.Instance.aimshot.dmg * 2.5f;
            yield return Yielders.Get(0.75f);
            SoundManager.Instance.PlayMagicNovationImpact();
            vfxList.Add(SimplePool.Spawn(explosion,fxPos[0].position,quaternion.identity));
            OnGivenDame();
            
            atk = atk = PlayingManager.Instance.aimshot.dmg * 4.5f;
            yield return Yielders.Get(0.75f);
            SoundManager.Instance.PlayMagicNovationImpact();
            vfxList.Add(SimplePool.Spawn(explosion,fxPos[0].position,quaternion.identity));
            OnGivenDame();
        }

        public override void OnDisable()
        {
            base.OnDisable();
        }
    }
}
