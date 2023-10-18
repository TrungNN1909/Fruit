using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{

    public class StageManager : MonoBehaviour
    {
        public static StageManager instance;

        public List<Stage> listStage;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);

            }
        }

        private void Start()
        {

            listStage = StageListAsset().data;

        }

        private StageListAsset StageListAsset()
        {
            var asset = Resources.Load<StageListAsset>($"Stage Data/{nameof(StageListAsset)}");
            return asset;
        }
    }
}