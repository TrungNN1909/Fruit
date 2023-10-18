using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class AchivementManager : MonoBehaviour
    {
        public static AchivementManager Instance;
        public List<ArchiveCard> mainGuns;
        public List<ArchiveCard> supportGun;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            mainGuns = ArchivementListAsset().mainGunsData;
            supportGun = ArchivementListAsset().supportGunData;
        }

        private ArchivementListAsset ArchivementListAsset()
        {
            var asset = Resources.Load<ArchivementListAsset>($"Stage Data/{nameof(ArchivementListAsset)}");
            return asset;
        }
    }
}
