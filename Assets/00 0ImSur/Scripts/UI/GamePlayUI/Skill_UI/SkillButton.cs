using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unicorn.Utilities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Unicorn
{
    public class SkillButton : MonoBehaviour
    {
        public int id;
        
        [SerializeField] private GameObject skillPrefab;

        [SerializeField] private Button activateSkill;
        [SerializeField] private GameObject lockSkill;
        [SerializeField] private Image coolDownImg;
        [SerializeField] private TextMeshProUGUI timeUseLeftText;

        [SerializeField] private GameObject watchAd;
        // [SerializeField] private GameObject freeUse;

        [SerializeField] public SkillsTab skillsTab;
        public int carLv;
        public bool used; // check whether skill is used or not
        public int usedTimes; // how many use.

        public bool isFreeToUse;
        public bool isCoolingDown;
        private Tween waitCoolDown;
        
        private void Start()
        {
            activateSkill.onClick.AddListener(ActivateSkillButtonOnClick);
            timeUseLeftText.text = usedTimes.ToString();
        }
        
        
        public void Init()
        {
            usedTimes = 2;
            carLv = PlayerDataManager.Instance.GetCarHighestLevel();
            timeUseLeftText.text = usedTimes.ToString();
            coolDownImg.fillAmount = 0;
            if (carLv >= id)
            {
                lockSkill.SetActive(false);
            }
            else
            {
                lockSkill.SetActive(true);
            }
        }

        private void OnEnable()
        {
            CheckNewDaySkill();
        }

        public void NewPhaseStart()
        {
            if(usedTimes == 0 || isCoolingDown) return;
            
            used = true;
            coolDownImg.fillAmount = 1;
            coolDownImg.DOFillAmount(0, 3f).OnComplete(() =>
            {
                used = false;
            });
        }
        
        private void ActivateSkillButtonOnClick()
        {
            if(used) return;

            if (isFreeToUse || PlayerDataManager.Instance.GetStage() == 0 || PlayerDataManager.Instance.GetStage() == 1)
            {
                ActiveSkill();
                PlayerDataManager.Instance.SetTimeUseFreeSkills(DateTime.Now.ToString());
                skillsTab.CheckFreeSkill();
            }
            else
            {
                UnicornAdManager.ShowAdsReward(ActiveSkill, Helper.skillAd);
            }
            skillsTab.NewPhaseStartAction();
        }

        public void CheckNewDaySkill()
        {
            isFreeToUse = Helper.CheckNewDay(PlayerDataManager.Instance.GetTimeUseFreeSkills());

            if (isFreeToUse)
            {
                // freeUse.SetActive(true);
                watchAd.SetActive(false);
            }
            else
            {
                // freeUse.SetActive(false);
                watchAd.SetActive(true);
            }
        }
        
        private void ActiveSkill()
        {
            
            used = true;
            usedTimes--;
            timeUseLeftText.text = usedTimes.ToString();
            isCoolingDown = true;
            coolDownImg.DOFillAmount(1, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                if (usedTimes > 0)
                {
                    coolDownImg.DOFillAmount(0, 30f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        used = false;
                        isCoolingDown = false;
                    });
                }
            });
            
            SimplePool.Spawn(skillPrefab,new Vector3(8f, -5f, 0),quaternion.identity);
        }
    }
}
