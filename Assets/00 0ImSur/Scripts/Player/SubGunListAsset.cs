using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Unicorn
{

    [System.Serializable]
    public class SubGunInfo
    {
        public int lv;
        public Sprite sprite;
        public float baseDmg;
        public float spd;
        public float fireRate;
        public GameObject bullet;
        public GameObject subGun;
    }



    [CreateAssetMenu(fileName = "SubGunListAsset", menuName = "SubGun List Asset")]
    public class SubGunListAsset : SerializedScriptableObject
    {
        public Dictionary<int, SubGunInfo> data;
    }

}
