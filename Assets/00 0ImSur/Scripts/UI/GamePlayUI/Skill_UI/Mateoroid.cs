using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

namespace Unicorn
{
    public class Mateoroid : Skill
    {
        [SerializeField] public List<Transform> spawnMateoPos;
        [SerializeField] public GameObject explosion;
        public override void OnEnable()
        {
            base.OnEnable();
            StartCoroutine(DelayMateoroid());
            atk = PlayingManager.Instance.aimshot.dmg * 2.5f;
        }

        private IEnumerator DelayMateoroid()
        {
            int j = 0;
            for(int i=0; i<spawnMateoPos.Count; i++)
            {
                yield return Yielders.Get(0.5f);
                SoundManager.Instance.PlaySkillMeteoroidAppear();
                GameObject mateo = SimplePool.Spawn(vfx, spawnMateoPos[i].position,Quaternion.Euler(0,0,0));
                vfxList.Add(mateo);
                mateo.transform.DOMove(fxPos[i].transform.position, 1f).OnComplete(() =>
                {
                    //TodoExplosion
                    SoundManager.Instance.PlaySkillMateoroidImpact();
                    SimplePool.Despawn(mateo);
                    vfxList.Add(SimplePool.Spawn(explosion,fxPos[j].transform.position,quaternion.identity));
                    OnGivenDame();
                    j++;
                });
            }
        }
    }
}
