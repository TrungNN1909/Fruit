using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class Cat : MonoBehaviour
    {
        [HideInInspector] public AimShoot aimShoot;
        private Animator animator;
        //public bool shoting;

        private void Awake()
        {
            animator = gameObject.GetComponent<Animator>();
        }

        public void OnUpdate()
        {
            SetAnim();
        }

        public void LoseAction()
        {
            animator.SetBool("isShooting", false);
            animator.SetBool("isWin", false);
            animator.SetBool("isLose",true);
        }

        private void SetAnim()
        {
            if (aimShoot.isShotting && PlayingManager.Instance.isPlaying)
            {
                animator.SetBool("isShooting", true) ;
            }
            else
            {
                animator.SetBool("isShooting", false);
            }

            if (PlayingManager.Instance.isPhaseEnd)
            {
                animator.SetBool("isWin",true);
            }
            else
            {
                animator.SetBool("isWin", false);
            }
            
        }
    }
}
