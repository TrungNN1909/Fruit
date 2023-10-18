using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unicorn.Utilities;
using System;
using Spine;
using Unicorn.UI;
using UnityEngine.UI;


namespace Unicorn
{
    public class TabGunManager : MonoBehaviour
    {
        [SerializeField] public  List<GunSlot> gunSlots;
        [SerializeField] public List<GameObject> tempList;
        [SerializeField] public GameObject tutorial;
        [SerializeField] public Transform tutorialSlot;
        [SerializeField] public Transform currentTranform;

        [SerializeField] public GameObject handBuy;
        [SerializeField] public GameObject backTabGun;
        
        
        private void Awake()
        {
            for(int i=0; i< PlayerDataManager.Instance.GetStoredSlotCount(); i++)
            {
                gunSlots[i].SetSlotID(i);
                gunSlots[i].SetEmtySlot();
                gunSlots[i].CheckLock();
            }

            GameManager.Instance.HomeController.OnHoldingGunItem = OnHoldingGunItemAction;
            GameManager.Instance.HomeController.OnDropingGunItem = OnDropGunItemAction;
            GameManager.Instance.HomeController.checkHighestGun = CheckHighestGunAction;
        }

        private void OnEnable()
        {
            init();
        }

        public void init()
        {
            GameManager.Instance.HomeController.StoredGunSlot = UpdateTabGunStatusAction;

            List<GunSlot> temp = PlayerDataManager.Instance.GetStoredSlots();
            for (int i=0; i<PlayerDataManager.Instance.GetStoredSlotCount(); i++)
            {
                if (temp[i].GetGunSlotLevel() != -1 && gunSlots[i] != null)
                {
                    SpawnItem(gunSlots[i], (EGunType) temp[i].GetTypeGunSlot(), temp[i].GetGunSlotLevel());
                    gunSlots[i].CheckHighestGunSlot();
                }
            }
        }

        public void OnActionTutorial()
        {
            
            backTabGun.SetActive(false);
            if (PlayerDataManager.Instance.GetStage() == 1 && PlayerPrefs.GetInt("Tutorial", 1) == 5)
            {
                for (int i = 0; i < 3; i++)
                {
                    gunSlots[i].gameObject.transform.SetParent(tutorialSlot);
                }
                for (int i = 0; i < 3; i++)
                {
                    tempList[i].transform.SetAsFirstSibling();
                }
            }
        }
        
        public void OnEndTutorial()
        {
            //give sots were taken for tutorial back and set their position
            gunSlots[0].gameObject.transform.SetParent(currentTranform);
            gunSlots[1].gameObject.transform.SetParent(currentTranform);
            gunSlots[2].gameObject.transform.SetParent(currentTranform);
            gunSlots[2].gameObject.transform.SetAsFirstSibling();
            gunSlots[1].gameObject.transform.SetAsFirstSibling();
            gunSlots[0].gameObject.transform.SetAsFirstSibling();
            
            //push to back 
            for (int i = 0; i < 3; i++)
            {
                tempList[i].transform.SetAsLastSibling();
            }
            tutorial.SetActive(false);
            GameManager.Instance.HomeController.uiTutorial.gameObject.SetActive(false);
            GameManager.Instance.HomeController.uiHome.upgradeScreen.GetComponent<UpgradeScreen>().OnStartBackTutorial();

        }

        public void OnHoldingGunItemAction()
        {
            foreach (GunSlot gl in gunSlots)
            {
                gl.GetComponent<Image>().color = new Color(0, 255, 255, 255);
            }
        }

        public void OnDropGunItemAction()
        {
            foreach (GunSlot gl in gunSlots)
            {
                gl.GetComponent<Image>().color = new Color(255, 255, 255, 255);
            }
        }

        public void CheckHighestGunAction()
        {
            foreach (var VARIABLE in gunSlots)
            {
                VARIABLE.CheckHighestGunSlot();
            }
        }
        public GunSlot GetSlot()
        {
            for(int i=0; i<gunSlots.Count; i++)
            {
                GunSlot slot = gunSlots[i];
                if (!slot.isLock)
                {
                    GunItem currentItem = slot.slotItem.gameObject.GetComponentInChildren<GunItem>();
                    if (currentItem == null)
                    {
                        return slot;
                    }
                }
            }
            return null;
        }

        public void BuyGun(GunSlot slot)
        {
            if (slot == null)
            {
                PopupDialogCanvas.Instance.Show("No available slots");
                return;
            }

            EGunType e = EGunType.MAIN_GUN;

            if (PlayerPrefs.GetInt(Helper.SwitchGunBuy, 0) == 0)
            {

                e = EGunType.MAIN_GUN; 
            }
            else if (PlayerPrefs.GetInt(Helper.SwitchGunBuy) == 1)
            {
                e = EGunType.SUB_GUN;
            }

            if (PlayerDataManager.Instance.GetStage() == 1 && PlayerPrefs.GetInt("Tutorial") < 4)
            {
                e = EGunType.MAIN_GUN;
            }else if (PlayerPrefs.GetInt("Tutorial") == 4)
            {
                e = EGunType.SUB_GUN;
            }
            
            SpawnItem(slot, e, 1);
            GameObject glitterUI = SimplePool.Spawn(slot.glitter,slot.transform.position,Quaternion.identity);
            glitterUI.transform.SetParent(transform);
        }

        public void SpawnItem(GunSlot slot, EGunType type, int level)
        {
            switch (type)
            {
                case EGunType.MAIN_GUN:
                    GameObject spawnedMainGun = Pools.Instance.GetObjectFromPool("GunItem");
                    spawnedMainGun.transform.SetParent(slot.slotItem.transform); // lay vi tri spawn
                    spawnedMainGun.GetComponent<GunItem>().gunInfos = PlayerDataManager.Instance.GetGun(level); // lay sung spawn
                    spawnedMainGun.GetComponent<GunItem>().type = type;
                    spawnedMainGun.GetComponent<GunItem>().init();
                    PlayerPrefs.SetInt(Helper.SwitchGunBuy, 1);
                    break;

                case EGunType.SUB_GUN:
                    GameObject spawnedSubGun = Pools.Instance.GetObjectFromPool("GunItem");
                    spawnedSubGun.transform.SetParent(slot.slotItem.transform);
                    spawnedSubGun.GetComponent<GunItem>().subGunInfo = PlayerDataManager.Instance.GetSubGun(level);
                    spawnedSubGun.GetComponent<GunItem>().type = type;
                    spawnedSubGun.GetComponent<GunItem>().init();
                    PlayerPrefs.SetInt(Helper.SwitchGunBuy, 0);
                    break;
            }

            //store gun infomation to PlayerPref
            slot.SetTypeGunSlot(type);
            slot.SetGunLevel(level);
            slot.SetHighestGunLevel(type, level);
            GameManager.Instance.HomeController.checkHighestGun?.Invoke();
            UpdateTabGunStatusAction();
        }

        public void UpdateTabGunStatusAction()
        {
            
            PlayerDataManager.Instance.SetStoredSlotCount(gunSlots.Count);
            PlayerDataManager.Instance.SetStoredSlots(gunSlots);

        }
        
        private void OnDisable()
        {
            GameManager.Instance.HomeController.StoredGunSlot = null;
            UpdateTabGunStatusAction();
            foreach(GunSlot gunSlot in gunSlots)
            {
                GameObject temp = gunSlot.slotItem.gameObject;
                if (temp.transform.childCount != 0)
                {
                    temp.GetComponentInChildren<GunItem>().Reset();
                }
            }
        }

        private void OnApplicationQuit()
        {
            foreach(GunSlot gunSlot in gunSlots)
            {
                GameObject temp = gunSlot.slotItem.gameObject;
                if (temp.transform.childCount != 0)
                {
                    temp.GetComponentInChildren<GunItem>().Reset();
                }
            }
        }
    }
}
