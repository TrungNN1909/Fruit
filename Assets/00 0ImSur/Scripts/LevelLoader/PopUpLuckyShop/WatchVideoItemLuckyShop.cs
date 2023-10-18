using System.Collections;
using System.Collections.Generic;
using Unicorn.Utilities;
using UnityEngine;

namespace Unicorn
{
    public class WatchVideoItemLuckyShop : LuckyShopItemButton
    {
        
        public override void SetItem()
        {
            base.SetItem();
            itemId = Random.Range(0, 101);

            if (itemId < 33)
            {
                itemId = 10; //double
            }else if (itemId >=67)
            {
                itemId = 11; // +1 life
            }
            else
            {
                itemId = 12;// split
            }
            
            itemInfo = items[itemId];
            itemImg.sprite = itemInfo.itemAvata;
            description.text = itemInfo.description;
        }

        private void ActivaItem()
        {
            switch (itemId)
            {
                case 10:
                {
                    if (mainGun.type == ShootingType.Normal)
                    {
                        mainGun.type= ShootingType.Double;

                    }else if (mainGun.type == ShootingType.Double)
                    {
                        mainGun.type = ShootingType.Triple;
                    }else if (mainGun.type == ShootingType.Split)
                    {
                        mainGun.type = ShootingType.Quadra;
                    }else if (mainGun.type == ShootingType.Multi)
                    {
                        mainGun.type = ShootingType.DoubleMulti;
                    }
                    break;
                }
                case 11:
                    train.life++;
                    break;
                case 12:
                    if ( mainGun.type == ShootingType.Normal)
                    {
                        mainGun.type = ShootingType.Split;
                    }
                    else if ( mainGun.type == ShootingType.Double)
                    {
                        mainGun.type = ShootingType.Quadra;
                    }
                    else if ( mainGun.type == ShootingType.Split)
                    {
                        mainGun.type = ShootingType.Multi;
                    }else if ( mainGun.type == ShootingType.Triple)
                    {
                        mainGun.type = ShootingType.DoubleMulti;
                    }
                    break;
            }
            
            SimplePool.Spawn(SparkleFx);
            mainButton.interactable = false;
            lockItem.SetActive(true);
            donePerchase.SetActive(true);
        }

        protected override void MainButtonOnClick()
        {
            UnicornAdManager.ShowAdsReward(ActivaItem, Helper.PopUpLuckShop);
        }
    }
}
