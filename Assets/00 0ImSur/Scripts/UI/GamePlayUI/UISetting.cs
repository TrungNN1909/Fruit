using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Unicorn
{
    public class UISetting : MonoBehaviour
    {
        [SerializeField] private Button homeBtn;
        [SerializeField] private Button resumeBtn;


        private void Start()
        {
            homeBtn.onClick.AddListener(HomeButtonOnCLick);
            resumeBtn.onClick.AddListener(ResumeButtonOnClick);
        }


        private void OnEnable()
        {
            GameManager.Instance.uiGamePlayController.uiTutorial.Show(false);
        }

        private void OnDisable()
        {
            GameManager.Instance.uiGamePlayController.uiTutorial.Show(true);

        }

        private void ResumeButtonOnClick()
        {
            GameManager.Instance.Resume();
            gameObject.SetActive(false);
        }

        private void HomeButtonOnCLick()
        {
            GameManager.Instance.Resume();
            gameObject.SetActive(false);
            LoadingStartManager.Instance.LoadPlayingScreen();

        }
    }
}
