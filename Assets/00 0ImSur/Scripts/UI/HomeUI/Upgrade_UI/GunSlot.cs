using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unicorn.Utilities;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx.Triggers;

namespace Unicorn
{
    public class GunSlot : MonoBehaviour, IDropHandler
    {
        private int slotID;
        private int typeGunSlot;
        private int lvGunSlot;
        
        public bool isLock;
        [SerializeField] public GameObject LockObject;
        [SerializeField] public GameObject chosenMainSlot;
        [SerializeField] public GameObject chosenSubSlot;
        [SerializeField] public GameObject slotItem;
        [SerializeField] public Button lockButton;
        [SerializeField] public Image watchVideo;
        [SerializeField] public Image frame;
        [SerializeField] public Image back;

        [SerializeField] private Image img;
        [SerializeField] public GameObject glitter;
        public Collider2D imageCollider; 
        private void Awake()
        {
            lockButton.onClick.AddListener(OpenSlotOnClickButton);
        }

        private void OpenSlotOnClickButton()
        {
            UnicornAdManager.ShowAdsReward(OpenGunSlotReward, Helper.OpenGunSlotAd);

        }

        private void OpenGunSlotReward()
        {
            back.DOFillAmount(0f, 1f).SetEase(Ease.OutQuint).OnStart(() =>
            {
                watchVideo.DOFade(0, 1f);
                watchVideo.transform.DOScale(1.5f, 1f).SetEase(Ease.OutQuint);
            }).OnComplete(() =>
            {
                LockObject.SetActive(false);
                isLock = false;
                PlayerDataManager.Instance.SetSlotStatus(slotID, false);
            });
        }
        
        private void OnEnable()
        {
            CheckLock();
        }

        public void CheckLock()
        {
            isLock = PlayerDataManager.Instance.GetSlotStatus(slotID);
            if (isLock)
            {
                LockObject.SetActive(true);
            }
            else
            {
                LockObject.SetActive(false);
            }
        }
        
        
        public GunSlot()
        {
            
        }

        public GunSlot(int slotID, int typeGunSlot, int lvGunSlot)
        {
            this.slotID = slotID;
            this.typeGunSlot = typeGunSlot;
            this.lvGunSlot = lvGunSlot;
        }

        public void OnDrop(PointerEventData eventData)
        {
            //Debug.Log(eventData.pointerDrag.name);
            if (eventData.pointerDrag.name.Equals("Scroll View") || isLock) return;
            

            GameObject dropped = eventData.pointerDrag; // ==> pointer to gameobject is being draged
            GunItem gunItem = dropped.GetComponent<GunItem>();
            GunSlot previousSlot = gunItem.parentAfterDrag.gameObject.GetComponentInParent<GunSlot>(); // ==> pointer to the slot

            if (slotItem.transform.childCount == 0)
            {
                gunItem.parentAfterDrag = slotItem.transform;
                //1. set info for this slot
                SetTypeGunSlot(gunItem.type);
                SetGunLevel(gunItem.level);
                //2. reset slot of previous gun
                previousSlot.SetEmtySlot();
                GameObject glitterUI = SimplePool.Spawn(glitter,transform.position,Quaternion.identity);
                glitterUI.transform.SetParent(transform);
            }
            else if(slotItem.transform.childCount == 1)
            {
                GameObject childItem = slotItem.transform.GetChild(0).gameObject; // item that not being dragged

                if(childItem.GetComponent<GunItem>().level == gunItem.level &&
                    childItem.GetComponent<GunItem>().type == gunItem.type)
                {
                    //TODO merge.
                    gunItem.parentAfterDrag = gunItem.transformPool;
                    gunItem.isMerged = true;
                    childItem.GetComponent<GunItem>().Upgrade();
                    previousSlot.SetEmtySlot();
                    SetTypeGunSlot(childItem.GetComponent<GunItem>().type);
                    SetGunLevel(childItem.GetComponent<GunItem>().level);
                    SetHighestGunLevel((EGunType)typeGunSlot, lvGunSlot);
                    GameObject glitterUI = SimplePool.Spawn(glitter,transform.position,Quaternion.identity);
                    glitterUI.transform.SetParent(transform);
                    SoundManager.Instance.PlayMergeGunSound();
                    if (PlayerPrefs.GetInt("Tutorial", 1) == 5)
                    {
                        try
                        {
                            PlayerPrefs.SetInt("Tutorial",6); 
                            GameManager.Instance.uiHomeController.uiHome.upgradeScreen.GetComponent<UpgradeScreen>().handMerge.SetActive(false);
                            StartCoroutine(DelayForGunMergeTutorial());

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }
                else
                {
                    //TODO swap
                    // 1. change item to onDrag slot and info.
                    childItem.transform.SetParent(gunItem.parentAfterDrag);
                    previousSlot.SetTypeGunSlot((EGunType) typeGunSlot);
                    previousSlot.SetGunLevel(lvGunSlot);

                    //2. set item for this slot and reset info
                    gunItem.parentAfterDrag = slotItem.transform;
                    SetTypeGunSlot(gunItem.type);
                    SetGunLevel(gunItem.level);
                    GameObject glitterUI = SimplePool.Spawn(glitter,transform.position,Quaternion.identity);
                    glitterUI.transform.SetParent(transform);                }
            }
            
            //check highest gunslot level and store info of slot to PlayerDataManager
            GameManager.Instance.HomeController.checkHighestGun?.Invoke();
            GameManager.Instance.HomeController.StoredGunSlot?.Invoke();
        }

        private IEnumerator DelayForGunMergeTutorial()
        {
            yield return Yielders.Get(1f);
            GameManager.Instance.uiHomeController.uiHome.upgradeScreen.GetComponent<UpgradeScreen>().tabGunManager.GetComponent<TabGunManager>().OnEndTutorial();
        }
        public void SetSlotID(int slotID)
        {
            this.slotID = slotID;
        }

        public int GetSlotID()
        {
            return slotID;
        }

        public void SetTypeGunSlot(EGunType type)
        {
            typeGunSlot = (int) type;
        }

        public int GetTypeGunSlot()
        {
            return typeGunSlot;
        }

        public void SetGunLevel(int level)
        {
            lvGunSlot = level;
        }
        public int GetGunSlotLevel()
        {
            return lvGunSlot;
        }

        public void SetEmtySlot()
        {
            lvGunSlot = -1;
            typeGunSlot = -1;
        }

        public void SetHighestGunLevel(EGunType type, int level)
        {
            PlayingManager.Instance.DestroyAsset();
            switch (type)
            {
                case EGunType.MAIN_GUN:
                    if(level > PlayerDataManager.Instance.GetGunHighestLevel())
                    {
                        PlayerDataManager.Instance.SetGunHighestLevel(level);
                    }
                    break;

                case EGunType.SUB_GUN:
                    if(level > PlayerDataManager.Instance.GetSubGunHighestLevel())
                    {
                        PlayerDataManager.Instance.SetSubGunHighestLevel(level);
                    }
                    break;
            }
            PlayingManager.Instance.LoadAsset();
            CheckHighestGunSlot();
        }

        public void CheckHighestGunSlot()
        {
            switch (typeGunSlot)
            {
                case (int) EGunType.MAIN_GUN:
                    if (lvGunSlot == PlayerDataManager.Instance.GetGunHighestLevel())
                    {
                        chosenMainSlot.SetActive(true);
                        chosenSubSlot.SetActive(false);
                    }
                    else
                    {
                        chosenMainSlot.SetActive(false);
                        chosenSubSlot.SetActive(false);
                    }
                    break;

                case (int) EGunType.SUB_GUN:
                    if (lvGunSlot == PlayerDataManager.Instance.GetSubGunHighestLevel())
                    {
                        chosenSubSlot.SetActive(true);
                        chosenMainSlot.SetActive(false);
                    }
                    else
                    {
                        chosenSubSlot.SetActive(false);
                        chosenMainSlot.SetActive(false);
                    }
                    break;
                
                default:
                    chosenMainSlot.SetActive(false);
                    chosenSubSlot.SetActive(false);
                    break;
            }
        }
    }
}
