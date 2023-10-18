using Common.FSM;
using Unicorn.Utilities;
using UnityEngine;
using System.Collections;

namespace Unicorn.FSM
{
    public class EndgameAction : UnicornFSMAction
    {
        public EndgameAction(GameManager gameManager, FSMState owner) : base(gameManager, owner)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("OnEnter EndGameState");
            base.OnEnter();
            //PlayingManager.
            //SoundManager.Instance.StopFootStep();

            //Debug.Log("CurrentLevel_" + GameManager.Instance.CurrentLevel);

            ProcessWinLose();

            //SoundManager.Instance.PlayFxSound(GameManager.LevelManager.Result);
        }

        private void ProcessWinLose()
        {
            Debug.Log("CurrentLevel_" + GameManager.LevelManager.Result);
            SoundManager.Instance.PlayFxSound(GameManager.Instance.LevelManager.Result);
            switch (GameManager.LevelManager.Result)
            {
                case LevelResult.Win:
                    //TODO: code win UI
                    GameManager.Instance.GamePlayController.OpenUIWin(true);

                    //GameManager.UiController.OpenUiWin(Constants.GOLD_WIN);
                    //Analytics.LogEndGameWin(GameManager.Instance.CurrentLevel);
                    //PrefabStorage.Instance.fxWinPrefab.SetActive(true);
                    break;
                case LevelResult.Lose:
                    //TODO: code lose ui here.
                    
                    PlayingManager.Instance.cat.GetComponent<Cat>().LoseAction();
                    PlayingManager.Instance.subCat.GetComponent<SubCat>().LoseAction();
                    GameManager.Instance.GamePlayController.OpenUILose(true);
                    Debug.Log("Lose endgame action");


                    //GameManager.UiController.OpenUiLose();
                    //Analytics.LogEndGameLose(GameManager.Instance.CurrentLevel);
                    break;
                default:
                    break;
            }

            //GameManager.Instance.StartCoroutine(IEShowInter());
        }

        private IEnumerator IEShowInter()
        {
            yield return new WaitForSeconds(0.4f);

            switch (GameManager.LevelManager.Result)
            {
                case LevelResult.Win:
                    UnicornAdManager.ShowInterstitial(Helper.inter_end_game_win);
                    break;
                case LevelResult.Lose:
                    UnicornAdManager.ShowInterstitial(Helper.inter_end_game_lose);
                    break;
                default:
                    break;
            }
        }

        public override void OnExit()
        {
            Debug.Log("OnExit EndGameState");
            // Debug.Log("CurrentLevel_" + GameManager.LevelManager.Result);

            Time.timeScale = 1f;
            GameManager.Instance.GamePlayController.OpenUIWin(false);
            GameManager.Instance.GamePlayController.OpenUILose(false);
            // SoundManager.Instance.StopSound(GameManager.Instance.LevelManager.Result);
            base.OnExit();
            //SoundManager.Instance.StopSound(GameManager.LevelManager.Result);
            //PrefabStorage.Instance.fxWinPrefab.SetActive(false);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if(Input.GetMouseButtonDown(0))
            {
                SoundManager.Instance.PlaySoundButton();
            }
        }
    }
}