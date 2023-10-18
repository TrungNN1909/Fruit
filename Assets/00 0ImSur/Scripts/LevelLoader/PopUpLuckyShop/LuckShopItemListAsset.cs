using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Unicorn
{
    public class LuckyShopItemInfo
    {
        public int itemID;
        public Sprite itemAvata;
        public int coin;
        public string description;
    }
    
    [CreateAssetMenu(fileName = "LuckShopItemListAsset", menuName = "Lucky Shop Item List Asset")]
    public class LuckShopItemListAsset : SerializedScriptableObject
    {
        public List<LuckyShopItemInfo> data;
    }
}
