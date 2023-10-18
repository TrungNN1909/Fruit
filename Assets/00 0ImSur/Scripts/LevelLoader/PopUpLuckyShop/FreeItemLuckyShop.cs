using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class FreeItemLuckyShop : LuckyShopItemButton
    {
        
        protected override void OnClickAction()
        {
            base.OnClickAction();
            switch (itemId)
            {
                case 0:
                    mainGun.dmg += mainGun.dmg * 0.05f;
                    break;
                case 1:
                    mainGun.dmg += mainGun.dmg * 0.05f;
                    break;
                case 2:
                    mainGun.fireRate -= mainGun.fireRate * 0.1f;
                    break;
                case 3:
                    mainGun.fireRate -= mainGun.fireRate * 0.15f;
                    break;
                case 4:
                    train.HP += train.HP * 0.1f;
                    break;
                case 5:
                    train.HP += train.HP * 0.15f;
                    break;
            }
        }

        public override void SetItem()
        {
            base.SetItem();
            itemId = Random.Range(0, 6);
            itemInfo = items[itemId];
            itemImg.sprite = itemInfo.itemAvata;
            description.text = itemInfo.description;
        }
    }
}
