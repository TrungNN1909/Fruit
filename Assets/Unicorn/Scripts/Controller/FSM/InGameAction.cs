using Common.FSM;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn.FSM
{
    public class InGameAction : UnicornFSMAction
    {
        public InGameAction(GameManager gameManager, FSMState owner) : base(gameManager, owner)
        {
        }

        

        public override void OnEnter()
        {
            Debug.Log("OnEnter Ingame");
            GameManager.Instance.GamePlayController.uiNewPhase.EndPhasePower.Init();
            GameManager.Instance.GamePlayController.OpenUINewPhase(true);
            GameManager.Instance.GamePlayController.OpenUITutorial(true);

            PlayingManager.Instance.bossStageAction = GameManager.Instance.GamePlayController.uiPlaying.OpenBossWarning;
            PlayingManager.Instance.newPhaseStart = GameManager.Instance.GamePlayController.uiPlaying.skillsTab.NewPhaseStartAction;
            GameManager.Instance.GamePlayController.uiPlaying.skillsTab.Init();
            SoundManager.Instance.PlayFxSound(SoundManager.GameSound.Ingame);
            base.OnEnter();
            //LevelManager.Instance.StartLevel();

        }

        public override void OnExit()
        {
            Debug.Log("OnExit Ingame");
            GameManager.Instance.GamePlayController.OpenUINewPhase(false);
            GameManager.Instance.GamePlayController.OpenUIPlaying(false);
            GameManager.Instance.GamePlayController.OpenUITutorial(false);
            PlayingManager.Instance.bossStageAction = null;
            PlayingManager.Instance.newPhaseStart = null;
            PlayingManager.Instance.carMoveOutAction = null;
            PlayingManager.Instance.carMoveInAction = null;
            base.OnExit();
            SoundManager.Instance.StopSound(SoundManager.GameSound.Ingame);
            SoundManager.Instance.StopCarRunSound();
            SoundManager.Instance.StopShotSound();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            PlayingManager.Instance.OnIngameUpdate();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            PlayingManager.Instance.OnIngameFixUpdate();
        }

    }
}