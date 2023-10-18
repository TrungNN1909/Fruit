using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class PowerManager : MonoBehaviour
    {
        public static PowerManager Instance;
        public List<Power> powersList;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }else if(Instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            powersList = LoadPower().data;
        }

        private PowerListAsset LoadPower()
        {
            var asset = Resources.Load<PowerListAsset>($"Stage Data/{nameof(PowerListAsset)}");
            return asset;
        }
    }
}
