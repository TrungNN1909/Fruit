using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Unicorn
{
    [System.Serializable]
    public class BackgroundInfo
    {
        public int backgroundID;
        public GameObject backgroundPrefab;
    }

    [CreateAssetMenu(fileName = "BackgroundListAsset", menuName = "Background List Asset")]
    public class BackgroundListAsset : SerializedScriptableObject
    {
        public Dictionary<int, BackgroundInfo> backgrounds; 
    }
}
