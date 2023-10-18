using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class UIGamePlayController : MonoBehaviour
    {
        [SerializeField] private UILose uiLose;
        [SerializeField] public UINewPhase uiNewPhase;
        [SerializeField] private UIWin uiWin;
        [SerializeField] public UIPlaying uiPlaying;
        [SerializeField] public UITutorial uiTutorial;
        [SerializeField] public GameObject uiConfetti;
        [SerializeField] public PhaseTransation phaseTransation;
        
        private void Awake()
        {
            GameManager.Instance.GamePlayUIRegister((this));
        }

        public void OpenUILose(bool _bool)
        {
            uiLose.Show(_bool);
        }

        public void OpenUIWin(bool _bool)
        {
            uiWin.Show(_bool);
        }

        public void OpenUINewPhase(bool _bool)
        {
            uiNewPhase.Show(_bool);
        }

        public void OpenUIPlaying(bool _bool)
        {
            uiPlaying.Show(_bool);
        }
        public void OpenUITutorial(bool _bool)
        {
            uiTutorial.Show(_bool);
        }
    }
}
