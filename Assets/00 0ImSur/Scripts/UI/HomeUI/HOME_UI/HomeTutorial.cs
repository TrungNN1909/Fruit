using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Unicorn
{
    public class HomeTutorial : MonoBehaviour
    {
        [SerializeField] private Button upgradeButton;
        [SerializeField] private GameObject background;
        private void Start()
        {
            upgradeButton.onClick.AddListener(UpgradeButtonOnCLick);
        }

        private void UpgradeButtonOnCLick()
        {
            GameManager.Instance.HomeController.uiHome.upgradeScreen.SetActive(true);
            background.SetActive(false);
        }
    }
}
