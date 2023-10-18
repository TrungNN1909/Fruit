using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    [System.Serializable]
    public class ArchiveCard
    {
        public Sprite Sprite;
        public string text;
        public int coin;
        public int cardNumber;
    }

    [CreateAssetMenu(fileName = "ArchivementListAsset", menuName = "Archivement List Asset")]
    public class ArchivementListAsset : ScriptableObject
    {
        public List<ArchiveCard> mainGunsData;
        public List<ArchiveCard> supportGunData;
    }
}
