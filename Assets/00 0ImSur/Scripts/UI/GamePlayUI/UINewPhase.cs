using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Unicorn
{
    public class UINewPhase : UICanvas
    {
        [SerializeField] private Button resetButton;
        [SerializeField] private Button collectAllButton;
        [SerializeField] public EndPhasePower EndPhasePower;
        [SerializeField] public LuckShopScreen LuckShopScreen;
        
        public override void Show(bool _isShown, bool isHideMain = true)
        {
            base.Show(_isShown, isHideMain);
        }


        private void Start()
        {
            resetButton.onClick.AddListener(OnCLickResetButton);
            collectAllButton.onClick.AddListener(OnClickCollectAllButton);
            GameManager.Instance.GamePlayController.OpenUIPlaying(false);

        }

        private void OnEnable()
        {
            GameManager.Instance.GamePlayController.OpenUIPlaying(false);
            int randomShow = Random.Range(0, 3);
            if (PlayingManager.Instance.GetCurrentPhaseNumber() == 1 && PlayerDataManager.Instance.GetStage()>=2 && randomShow <= 1)
            {
                EndPhasePower.gameObject.SetActive(false);
                LuckShopScreen.gameObject.SetActive(true);
            }
            else
            {
                EndPhasePower.gameObject.SetActive(true);
                LuckShopScreen.gameObject.SetActive(false);
            }
            
            GameManager.Instance.GamePlayController.phaseTransation.FillIn();

        }

        private void OnDisable()
        {
            GameManager.Instance.GamePlayController.OpenUIPlaying(true);
            GameManager.Instance.GamePlayController.phaseTransation.FillOut();

        }

        public void OnCLickResetButton()
        {
            EndPhasePower.ResetButtonOnClick();
        }

        public void OnClickCollectAllButton()
        {
            EndPhasePower.CollectAllButton();
        }
    }
}
