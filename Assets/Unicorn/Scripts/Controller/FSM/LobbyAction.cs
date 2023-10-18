using Common.FSM;
using UnityEngine;

namespace Unicorn.FSM
{
    public class LobbyAction : UnicornFSMAction
    {
        public LobbyAction(GameManager gameManager, FSMState owner) : base(gameManager, owner)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("OnEnter Lobby");
            Time.timeScale = 1;
            GameManager.Instance.HomeController.OpenUIHome(true);
            base.OnEnter();

            //GameManager.UiController.UiMainLobby.Show(true);
            // SoundManager.Instance.PlayFxSound(SoundManager.GameSound.Lobby);
        }

        public override void OnExit()
        {
            Debug.Log("OnExit Lobby");
            GameManager.Instance.HomeController.OpenUIHome(false);
            SoundManager.Instance.StopSound(SoundManager.GameSound.Lobby);
            base.OnExit();
            //GameManager.UiController.UiMainLobby.Show(true);
            //SoundManager.Instance.StopSound(SoundManager.GameSound.Lobby);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            GameManager.Instance.HomeController.OnUpdate();
            if(Input.GetMouseButtonDown(0))
            {
                SoundManager.Instance.PlaySoundButton();
            }
        }

    }
}