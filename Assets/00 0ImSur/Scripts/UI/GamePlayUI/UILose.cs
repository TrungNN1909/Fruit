using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using Unicorn.Utilities;

namespace Unicorn
{
    public class UILose : UICanvas
    {
        [SerializeField] private Button reviveBtn;
        [SerializeField] private Button noThanksBtn;
        [SerializeField] private TextMeshProUGUI coinEarned;
        [SerializeField] private TextMeshProUGUI countdownReviveText;

        [SerializeField] private Button ReturnHomeBtn;
        Coroutine ReviveCoroutine;
        public override void Show(bool _isShown, bool isHideMain = true)
        {
            base.Show(_isShown, isHideMain);

            if (!isShow)
            {
                return;
            }
            

        }

        private void OnEnable()
        {
            GameManager.Instance.GamePlayController.OpenUIPlaying(false);


            if (!PlayingManager.Instance.isRevived) // not revive yet
            {
                reviveBtn.gameObject.SetActive(true);
                ReturnHomeBtn.gameObject.SetActive(false);

                Debug.Log(PlayingManager.Instance.isRevived);

                ReviveCoroutine = StartCoroutine(CountDownRivive());
            }
            else
            {
                reviveBtn.gameObject.SetActive(false);
                ReturnHomeBtn.gameObject.SetActive(true);
            }
        }

        private void Start()
        {
            reviveBtn.onClick.AddListener(ReviveOnClick);
            noThanksBtn.onClick.AddListener(NoThanksOnClick);
            ReturnHomeBtn.onClick.AddListener(NoThanksOnClick);
            coinEarned.text = (PlayingManager.Instance.currentCoinEarn*PlayingManager.Instance.GetCurrentPhaseNumber()).ToString();

        }

        private void ReviveOnClick()
        {
            if (ReviveCoroutine != null)
                StopCoroutine(ReviveCoroutine);
            
            UnicornAdManager.ShowAdsReward(ReviveAction, Helper.ReviveAd);
        }

        private void ReviveAction()
        {
            GameManager.Instance.GamePlayController.OpenUILose(false);
            GameManager.Instance.GamePlayController.OpenUIPlaying(true);
            GameManager.Instance.Revive();
        }
        
        private void NoThanksOnClick()
        {
            UnicornAdManager.ShowInterstitial(Helper.EndStageLoseNothanks);
            GameManager.Instance.GameStateController.ChangeState(GameState.LOBBY);
            StopCoroutine(ReviveCoroutine);
            LoadingStartManager.Instance.LoadPlayingScreen();
            GameManager.Instance.GamePlayController.OpenUILose(false);
        }

        private IEnumerator CountDownRivive()
        {
            for(int i=5; i>=0; i--)
            {
                countdownReviveText.text = i.ToString();
                yield return new WaitForSecondsRealtime(1f);

                if(i == 0)
                {
                    reviveBtn.gameObject.SetActive(false);
                    ReturnHomeBtn.gameObject.SetActive(true);
                    GameManager.Instance.LevelManager.Result = LevelResult.Lose;
                    GameManager.Instance.GameStateController.ChangeState(GameState.END_GAME);
                    Debug.Log("called");
                }
            }

            
        }
    }


}
