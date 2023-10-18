using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
namespace Unicorn
{

    public class SceneTransation : MonoBehaviour
    {
        [SerializeField] CanvasGroup group;
        [SerializeField] private Image imgLoading;
        [SerializeField] private Image imgLoad;
        [SerializeField] private float timeLoading = 2;

        private AsyncOperation loadSceneAsync;

        void Start()
        {
            LoadPlayingScene();
            RunLoadingBar();


        }

        private void LoadPlayingScene()
        {
            loadSceneAsync = SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);

        }

        private void RunLoadingBar()
        {
            imgLoading.DOFillAmount(0.9f, timeLoading)
                .SetEase(Ease.OutQuint)
                .OnComplete(() =>
                {
                    imgLoading.fillAmount = 1f;
                    StartCoroutine(Fade());
                });
        }

        private IEnumerator Fade()
        {
            yield return new WaitForSeconds(1f);
            group.DOFade(0, 0.2f)
                .OnComplete(() =>
                {
                    group.gameObject.SetActive(false);
                });
        }

    }

}
