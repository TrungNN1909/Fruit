using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Unicorn
{
    public class PhaseTransation : MonoBehaviour
    {
        [SerializeField] public Image top;
        [SerializeField] public Image bot;
        
        public void FillIn()
        {
            top.DOFillAmount(1, 1.5f);
            bot.DOFillAmount(1, 1.5f);
        }

        public void FillOut()
        {
            top.DOFillAmount(0, 1.5f).SetDelay(0.3f);
            bot.DOFillAmount(0, 1.5f).SetDelay(0.3f);
        }
    }
    
    
}
