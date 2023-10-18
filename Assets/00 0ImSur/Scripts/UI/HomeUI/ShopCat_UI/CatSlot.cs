using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

namespace Unicorn
{
    enum TypeOpen
    {
        OPEN_BY_AD,
        OPEM_BY_COIN
    }

    public class CatSlot : MonoBehaviour
    {
        public int slotID;
        public TypeTab type;

        [SerializeField] private GameObject gliiter;

        public GameObject cat;

        [SerializeField] private GameObject iuCatSpawnPostion;
        [SerializeField] private GameObject isChosingSlot;
        [SerializeField] private GameObject isLockSlot;
        [SerializeField] private Button mainButton;
        
        [SerializeField] private Image lockImg;
        [SerializeField] private Image frame;
        [SerializeField] private Image back;
        
        public void SpawnCat()
        {
            cat = Instantiate(PlayerDataManager.Instance.catListAsset.data[slotID].CatUI, iuCatSpawnPostion.transform);
        }

        public void Start()
        {

            mainButton.onClick.AddListener(MainButtonOnClick);
        }

       

        private void MainButtonOnClick()
        {
            if (!PlayerDataManager.Instance.GetCatSlotOpen(slotID))
            {
                return;
            }
            
            switch (type)
            {
                case TypeTab.MAIN:
                    PlayerDataManager.Instance.SetMainCatChossen(slotID);
                    break;
                case TypeTab.SUB:
                    PlayerDataManager.Instance.SetSubCatChossen(slotID);
                    break;
            }

            gameObject.GetComponentInParent<TabMainCat>().Init();

            PlayingManager.Instance.DestroyAsset();
            PlayingManager.Instance.LoadAsset();
            
            GameObject glitterUI = SimplePool.Spawn(gliiter,transform.position,Quaternion.identity);
            glitterUI.transform.SetParent(transform);

        }

        public void CheckChosingMain()
        {
            if (PlayerDataManager.Instance.GetMainCatChossen() == slotID) 
            {
                isChosingSlot.SetActive(true);
            }
            else
            {
                isChosingSlot.SetActive(false);
            }
        }

        public void CheckChosingSub()
        {
            if (PlayerDataManager.Instance.GetSubCatChossen() == slotID)
            {
                isChosingSlot.SetActive(true);
            }
            else
            {
                isChosingSlot.SetActive(false);
            }
        }

        public void CheckOpenSlot()
        {
            if (PlayerDataManager.Instance.GetCatSlotOpen(slotID))
            {
                lockImg.DOFade(0, 1f);
                lockImg.transform.DOScale(1.5f, 1f).SetEase(Ease.OutQuint).OnComplete(() =>
                {
                    lockImg.gameObject.SetActive(false);
                });
                back.DOFillAmount(0f, 1f).SetDelay(1f).SetEase(Ease.OutQuint).OnStart(() =>
                {
                    
                }).OnComplete(() =>
                {
                    isLockSlot.SetActive(false);
                });
            }
            else
            {
                isLockSlot.SetActive(true);

            }
        }


    }
}
