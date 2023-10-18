using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Unicorn
{
    public class EnemiesHpbar : MonoBehaviour
    {
        [SerializeField] public Slider slider;
        
        
        public void ChangeValue(float value)
        {
            slider.value = value;
        }
    }
}
