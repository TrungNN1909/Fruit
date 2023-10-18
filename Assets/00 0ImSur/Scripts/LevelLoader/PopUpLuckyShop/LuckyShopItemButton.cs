using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unicorn
{
    public class LuckyShopItemButton : MonoBehaviour
    {
        [SerializeField] public Button mainButton;
        [SerializeField] public Image itemImg;
        [SerializeField] public TextMeshProUGUI description;
        [SerializeField] public GameObject lockItem;
        [SerializeField] public GameObject donePerchase;
        public List<LuckyShopItemInfo> items;
        public int itemId;
        public LuckyShopItemInfo itemInfo;
        public Train train;
        public AimShoot mainGun;
        public List<SubGun> subGuns;
        public GameObject SparkleFx;

        private void Start()
        {
            mainButton.onClick.AddListener(MainButtonOnClick);
        }
        
        protected virtual void MainButtonOnClick()
        {
            OnClickAction();
            
        }

        private void OnEnable()
        {
            
            mainButton.interactable = true;
            lockItem.SetActive(false);
            donePerchase.SetActive(false);
            Init();
        }

        public virtual void SetItem()
        {
            
        }
        
        protected virtual void OnClickAction()
        {
            SimplePool.Spawn(SparkleFx);
            mainButton.interactable = false;
            lockItem.SetActive(true);
            donePerchase.SetActive(true);
        }
        public virtual  void Init()
        {
            items = PlayerDataManager.Instance.luckShopItemListAsset.data;
            train = PlayingManager.Instance.train;
            mainGun = PlayingManager.Instance.aimshot;
            subGuns = PlayingManager.Instance.subGuns;
            SetItem();
        }
    }
}
