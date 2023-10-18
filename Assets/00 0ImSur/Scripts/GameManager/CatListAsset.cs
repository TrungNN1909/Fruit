using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Unicorn
{
    [System.Serializable]
    public class CatInfo
    {
        public int catID;
        public GameObject Cat;
        public GameObject CatUI;
    }

    [CreateAssetMenu(fileName = "CatListAsset", menuName = "Cat List Asset")]
    public class CatListAsset : SerializedScriptableObject
    {
        public Dictionary<int, CatInfo> data;
    }
}
