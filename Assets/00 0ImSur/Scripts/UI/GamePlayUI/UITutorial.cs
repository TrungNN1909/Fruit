using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Unicorn
{
    public class UITutorial : UICanvas
    {
        [SerializeField] public GameObject ShootingTutorial;
        [SerializeField] public GameObject UsingSkillTutorial;
        [SerializeField] public Image HandTutorial;

        [SerializeField] public Transform destination;
        [SerializeField] public Transform suorce;
        
        [SerializeField] public Button shootingTutorialButton;
        [SerializeField] public Button usingSkillTutorialButton;

        [SerializeField] public GameObject NovaSkill;
        public override void Show(bool _isShown, bool isHideMain = true)
        {
            base.Show(_isShown, isHideMain);
        }

        private void Start()
        {
            usingSkillTutorialButton.onClick.AddListener(UsingSkillTutorialOnClick);
            // shootingTutorialButton.onClick.AddListener(ShootingTutorialOnClick);
        }

        private void Update()
        {
            if (Input.touchCount > 0 && ShootingTutorial.activeInHierarchy)
            {
                GameManager.Instance.Resume();
                ShootingTutorial.SetActive(false);
                Debug.Log("Touched");
            }
        }

        private void UsingSkillTutorialOnClick()
        {
            SimplePool.Spawn(NovaSkill,new Vector3(8f, -5f, 0),Quaternion.identity);
            UsingSkillTutorial.SetActive(false);
            GameManager.Instance.Resume();
            GameManager.Instance.GamePlayController.uiPlaying.skillsTab.gameObject.SetActive(false);
        }

        public void HandTutorialAction()
        {
            HandTutorial.DOFade(255, 1f).OnStart(() =>
            {
                HandTutorial.gameObject.transform.DOMove(destination.position, 1.5f).SetLoops(2, loopType: LoopType.Yoyo)
                    .SetEase(Ease.InOutCubic).OnComplete(
                        () =>
                        {
                            HandTutorial.DOFade(0f, 0.5f).OnComplete(() =>
                            {
                                HandTutorial.gameObject.SetActive(false);
                            });
                        });
            });
        }
    }
}
