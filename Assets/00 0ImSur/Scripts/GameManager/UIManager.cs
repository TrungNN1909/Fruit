using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Unicorn
{
    public class UIManager : UICanvas
    {

        [SerializeField] GameObject EndPhaseScreen;
        [SerializeField] GameObject WinScreen;
        [SerializeField] GameObject LoseScreen;
        [SerializeField] GameObject PlayingScreen;
        [SerializeField] GameObject HomeScreen;

        

        public void TouchToPlayOnClick()
        {
            HomeScreen.SetActive(false);
            //EndPhaseScreen.SetActive(true);
            PlayingManager.Instance.LoadStage();
            GameManager.Instance.StartLevel();
        }


    }
}

