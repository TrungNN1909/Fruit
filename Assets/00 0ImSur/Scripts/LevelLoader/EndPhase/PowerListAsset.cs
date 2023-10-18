using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Unicorn
{
    [System.Serializable]
    public class Power
    {
        public int id;
        public Sprite avatar;
        public Sprite frame;
        public string description;
    }

    [CreateAssetMenu(fileName = "PowerListAsset", menuName = "Power List Asset")]
    public class PowerListAsset : ScriptableObject
    {
        public List<Power> data;
    }
}
