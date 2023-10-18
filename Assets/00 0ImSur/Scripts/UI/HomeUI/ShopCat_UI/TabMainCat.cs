using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class TabMainCat : MonoBehaviour
    {
        [SerializeField] public List<CatSlot> listCatSlots;
        [SerializeField] private Transform catSpawnPos;
        public TypeTab type;
        private int currentMaxSlotOpen;
        
        private void Awake()
        {
            PlayerDataManager.Instance.SetCatSlotOpen(0, true);
            PlayerDataManager.Instance.SetCatSlotOpen(1, true);
            for (int i=0; i<listCatSlots.Count; i++)
            {
                listCatSlots[i].slotID = i;
                listCatSlots[i].SpawnCat();
            }

            
        }
        
        public void OnEnable()
        {
            Init();
        }

        public void Init()
        {
            currentMaxSlotOpen = PlayerDataManager.Instance.GetMaxCatSlotOpen();
            MainCatDisplay();
            switch (type)
            {
                case TypeTab.MAIN:
                    for(int i=0; i<listCatSlots.Count; i++)
                    {
                        listCatSlots[i].type = type;
                        listCatSlots[i].CheckChosingMain();
                        listCatSlots[i].CheckOpenSlot();

                    }
                    break;
                case TypeTab.SUB:
                    for (int i = 0; i < listCatSlots.Count; i++)
                    {
                        listCatSlots[i].type = type;
                        listCatSlots[i].CheckChosingSub();
                        listCatSlots[i].CheckOpenSlot();
                    }
                    break;
            }
        }

        public void MainCatDisplay()
        {
            int catID = PlayerDataManager.Instance.GetMainCatChossen();
            if (catSpawnPos.childCount > 0)
            {
                Destroy(catSpawnPos.GetChild(0).gameObject);
                Instantiate(PlayerDataManager.Instance.catListAsset.data[catID].CatUI, catSpawnPos);
            }
            else
            {
                Instantiate(PlayerDataManager.Instance.catListAsset.data[catID].CatUI, catSpawnPos);
            }
        }
        
        public void Buy()
        {
            PlayerDataManager.Instance.SetMaxCatSlotOpen(currentMaxSlotOpen + 1);
            PlayerDataManager.Instance.SetCatSlotOpen(listCatSlots[currentMaxSlotOpen].slotID + 1, true);
            Init();
        }
    }
}
