using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;

namespace Unicorn
{
    public class LoadingStartManager : MonoBehaviour
    {
        [SerializeField] CanvasGroup group;
        [SerializeField] private Image imgLoading;
        // [SerializeField] private Image imgLoad;
        [SerializeField] private float timeLoading = 5;

        private AsyncOperation loadSceneAsync;
        private AppOpenAdManager appOpenAdManager;
        public static LoadingStartManager Instance { get; set; }

        [SerializeField] private PopupGDPR popupGDPR;

        private void Awake()
        {
            appOpenAdManager = AppOpenAdManager.Instance;
            Instance = this;
        }

        void Start()
        {
            timeLoading = 5;
            popupGDPR.ActionClose = Init;

            AppStateEventNotifier.AppStateChanged += OnAppStateChanged;

            if (popupGDPR.IsChecked())
            {
                Init();
            }
        }

        public void Init()
        {
            Debug.Log("Init Splash");
            UnicornAdManager.Init();
            LoadAppOpen();
            DontDestroyOnLoad(gameObject);
            LoadMasterLevel();
            RunLoadingBar();
        }

        private void LoadAppOpen()
        {
#if UNITY_EDITOR
            return;
#endif
            MobileAds.Initialize(initStatus => { appOpenAdManager.LoadAd(); });
        }

        private void LoadMasterLevel()
        {
            loadSceneAsync = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        }

        private void RunLoadingBar()
        {
            imgLoading.DOFillAmount(0.9f, timeLoading)
                .OnUpdate(() =>
                {
                    Debug.Log("still loading");
                })
                .SetEase(Ease.OutQuint)
                .OnComplete(() =>
                {
                    imgLoading.fillAmount = 1f;
                    StartCoroutine(Fade());
                });
        }
        
        private IEnumerator Fade()
        {
            yield return new WaitForSeconds (0.5f);
            group.DOFade(0, 0.2f)
                .OnComplete(() =>
                {
                    imgLoading.fillAmount = 0f;
                    group.alpha = 1;
                    group.gameObject.SetActive(false);
                    SoundManager.Instance.PlayFxSound(SoundManager.GameSound.Lobby);
                });
        }

        public void LoadPlayingScreen()
        {
            group.gameObject.SetActive(true);
            timeLoading = 2;
            if (PlayerDataManager.Instance.GetStage() == 1)
            {
                loadSceneAsync = SceneManager.LoadSceneAsync(4, LoadSceneMode.Single);
            }
            else
            {
                loadSceneAsync = SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
            }
            RunLoadingBar();
        }


        private void OnAppStateChanged(AppState state)
        {
            // Display the app open ad when the app is foregrounded.
            Debug.Log("App State is " + state);
            if (state == AppState.Foreground)
            {
                appOpenAdManager.ShowAdIfAvailable();
            }
        }

    }
}