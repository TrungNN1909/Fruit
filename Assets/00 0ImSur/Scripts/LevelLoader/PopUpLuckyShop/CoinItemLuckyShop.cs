using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unicorn.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unicorn
{
    public class CoinItemLuckyShop : LuckyShopItemButton
    {
        [SerializeField] public TextMeshProUGUI coin;

        [SerializeField] public TextMeshProUGUI coinDisplay;
        [SerializeField] public GameObject popupCoin;
        public override void SetItem()
        {
            base.SetItem();
            itemId = Random.Range(0, 101);
            if (itemId < 100)
            {
                itemId = 7;
            }else if (itemId >= 25 && itemId < 50)
            {
                itemId = 7;
            }else if (itemId >= 50 && itemId < 75)
            {
                itemId = 8;
            }
            else
            {
                itemId = 9;
            }
            itemInfo = items[itemId];
            itemImg.sprite = itemInfo.itemAvata;
            description.text = itemInfo.description;
            coin.text = itemInfo.coin.ToString();
            coinDisplay.text = PlayerDataManager.Instance.GetCoin().ToString();

        }

        public bool CheckShowPopUp()
        {
            string time = PlayerDataManager.Instance.GetTimesShowPopUpCoin();
            if (time == "") return true;
            
            DateTime timeOld = DateTime.Parse(time);
            DateTime timeNow = DateTime.Now;

            long tickTimeNow = timeNow.Ticks;
            long tickTimeOld = timeOld.Ticks;
            long elapsedTicks = tickTimeNow - tickTimeOld;
            TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
            float totalSeconds = (float)elapsedSpan.TotalSeconds;
            Debug.Log(30 * 60 - totalSeconds <= 0);
            return 30 * 60 - totalSeconds <= 0;
        }
        
        protected override void OnClickAction()
        {
  
            if (PlayerDataManager.Instance.GetCoin() < itemInfo.coin)
            {
                if (CheckShowPopUp() && PlayerDataManager.Instance.GetTimeEarnPopUpCoin() < 3)
                {
                    Instantiate(popupCoin,GameManager.Instance.uiGamePlayController.transform);
                    PlayerDataManager.Instance.SetTimeShowPopUpCoin(DateTime.Now.ToString());
                }
                else
                {
                    PopupDialogCanvas.Instance.Show("Not enough coin");
                }
                
                lockItem.SetActive(true);
                return;
            }

            base.OnClickAction();
            switch (itemId)
            {
                case 6:
                    PlayingManager.Instance.isActiveBuffAtk = true;
                    break;
                case 7:
                    PlayingManager.Instance.isActiveBuffFireRate = true;
                    break;
                case 8:
                    mainGun.critRate += 5;
                    break;
                case 9:
                    mainGun.critRate += 10;
                    break;
            }
            
            PlayerDataManager.Instance.SetCoin(-itemInfo.coin);
            coinDisplay.text = PlayerDataManager.Instance.GetCoin().ToString();
        }
    }
}
