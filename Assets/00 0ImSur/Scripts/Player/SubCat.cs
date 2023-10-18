using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class SubCat : MonoBehaviour
    {
        [HideInInspector] public SubGun subGun;
        [HideInInspector] public Transform subGunPosition;
        private Animator animator;

        public void init()
        {
            animator = gameObject.GetComponent<Animator>();
            subGunPosition = gameObject.transform.GetChild(0);
        }

        public void OnUpdate()
        {
            SetAnimaton();
        }

        public void LoseAction()
        {
            animator.SetBool("isShooting", false);
            animator.SetBool("isWin", false);
            animator.SetBool("isLose",true);
        }
        
        private void SetAnimaton()
        {
            if (subGun.hasEnemy && PlayingManager.Instance.isPlaying)
            {
                animator.SetBool("isShooting", true);
            }
            else
            {
                animator.SetBool("isShooting", false);
            }

            if (PlayingManager.Instance.isPhaseEnd)
            {
                animator.SetBool("isWin", true);
            }
            else
            {
                animator.SetBool("isWin", false);
            }
        }
    }
}
