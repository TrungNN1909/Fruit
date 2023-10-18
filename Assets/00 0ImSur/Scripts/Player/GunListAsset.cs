using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Unicorn
{
    [System.Serializable]
    public class GunInfos 
    {
        public int lv;
        public Sprite sprite;
        public float baseDmg;
        public float spd;
        public GameObject bullet;
        public GameObject Gun;
    }

    

    [CreateAssetMenu(fileName = "GunListAsset", menuName = "Gun List Asset")]
    public class GunListAsset : SerializedScriptableObject
    {
         public Dictionary<int,GunInfos> data;
    }
}
