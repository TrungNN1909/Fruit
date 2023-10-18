using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Unicorn
{
    [System.Serializable]
    public class GiftInfo
    {
        public Sprite spriteGift;
        public string textGift;
        public int giftNumber;
        public int coin;
        public int subGunLv;
    }

    [CreateAssetMenu(fileName = "GiftListAsset", menuName = "Gift List Asset")]
    public class GiftListAsset : SerializedScriptableObject
    {
        public List<GiftInfo> data;
    }
}
