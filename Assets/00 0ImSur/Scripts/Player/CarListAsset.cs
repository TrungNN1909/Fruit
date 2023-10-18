using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Unicorn
{
    [System.Serializable]
    public class CarInfo 
    {
        public int level;
        public Sprite avatar;
        public int amountIncreaseHP;
        public int timesToNextLevel;
        public float baseHP;
        public int baseCost;
        public int costIncreasing;
        public GameObject car;
        public GameObject carInTab;
        public Vector3 defaultPositonOnPlaying;
        public Vector3 defaultPositionNotOnPlaying;
    }

    [CreateAssetMenu(fileName = "CarListAsset", menuName = "Car List Asset")]
    public class CarListAsset : SerializedScriptableObject
    {
        public Dictionary<int,CarInfo> data;
    }
}
