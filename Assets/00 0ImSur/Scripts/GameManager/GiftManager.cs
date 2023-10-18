using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class GiftManager : MonoBehaviour
    {
        public static GiftManager Instance;
        public List<GiftInfo> list;


        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            list = GiftListAsset().data;
        }

        private GiftListAsset GiftListAsset()
        {
            var asset = Resources.Load<GiftListAsset>($"Stage Data/{nameof(GiftListAsset)}");
            return asset;
        }
    }
}
