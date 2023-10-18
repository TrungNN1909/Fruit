using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace Unicorn
{
    [System.Serializable]
    public class Stage
    {
        public int stageNumber;
        public List<Phase> phases;

    }

    [CreateAssetMenu(fileName = "StageListAsset", menuName = "Stage List Asset")]
    public class StageListAsset : ScriptableObject
    {
        public List<Stage> data;
    }

}