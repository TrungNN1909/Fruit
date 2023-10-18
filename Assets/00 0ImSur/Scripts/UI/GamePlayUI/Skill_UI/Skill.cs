using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Unicorn
{
    public class Skill : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] public List<Transform> fxPos;
        [SerializeField] public GameObject vfx;

        protected List<GameObject> vfxList;

        public float atk;

        public virtual void OnEnable()
        {
            vfxList = new List<GameObject>();
            PlayingManager.Instance.skillMoveOutAction = EndPhaseOnActiveSkillAction;
        }

        public virtual void OnDisable()
        {
            PlayingManager.Instance.skillMoveOutAction = null;
            vfxList.Clear();
        }
        
        private void EndPhaseOnActiveSkillAction()
        {
            if (gameObject.activeInHierarchy)
            {
                foreach (var VARIABLE in vfxList)
                {
                    VARIABLE.transform.DOMoveX(40f, 3f);
                }
            }
        }

        public virtual void OnGivenDame()
        {
            foreach (BaseEnemy enemy in PlayingManager.Instance.currentEnemies)
            {
                enemy.OnTakenDmg(atk,false);
            }
        }
        
    }
}
