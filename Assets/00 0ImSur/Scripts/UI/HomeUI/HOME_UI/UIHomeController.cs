using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace Unicorn
{
    public class UIHomeController : MonoBehaviour
    {
        [SerializeField] public UIHome uiHome;
        [SerializeField] public HomeTutorial uiTutorial;
        
        //fx action
        public Action OnHoldingGunItem;
        public Action OnDropingGunItem;
        
        //save gunslot data to PlayerDataManager
        public Action StoredGunSlot;

        //check fran action
        public Action checkHighestGun;
        
        public void OpenUIHome(bool _bool)
        {
            uiHome.Show(_bool);
        }

        public void OnUpdate()
        {
            uiHome.OnUpdate();
        }

        private void Awake()
        {
            GameManager.Instance.HomeUIRegister(this);
            if (PlayerPrefs.GetInt("Tutorial",0) == 1)
            {
                uiTutorial.gameObject.SetActive(true);
            }
            else
            {
                uiTutorial.gameObject.SetActive(false);

            }
        }

        private void Start()
        {
            uiHome.giftScreen.GetComponent<GiftScreen>().Init();
            uiHome.ArchivementScreen.GetComponent<ArchivementScreen>().Init();
            uiHome.HomeUIDisPlay(true);
        }
    }
}
