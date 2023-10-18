using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Unicorn
{
    public class TutorialFirstPhase : MonoBehaviour
    {
        [SerializeField] public List<GameObject> backs;
        [SerializeField] public List<GameObject> hands;
        [SerializeField] public List<Button> buttons;
        private void OnEnable()
        {
            switch (PlayingManager.Instance.GetCurrentPhaseNumber())
            {
                case 0:
                    backs[0].SetActive(false);
                    hands[0].SetActive(true);
                    backs[1].SetActive(true);
                    hands[1].SetActive(false);
                    backs[2].SetActive(true);
                    hands[2].SetActive(false);
                    buttons[1].interactable = false;
                    buttons[2].interactable = false;
                    buttons[0].interactable = true;
                    break;
                
                case 1:
                    backs[0].SetActive(true);
                    hands[0].SetActive(false);
                    backs[1].SetActive(false);
                    hands[1].SetActive(true);
                    backs[2].SetActive(true);
                    hands[2].SetActive(false);
                    buttons[1].interactable = true;
                    buttons[2].interactable = false;
                    buttons[0].interactable = false;
                    break;
                
                case 2:
                    backs[0].SetActive(true);
                    hands[0].SetActive(false);
                    backs[1].SetActive(true);
                    hands[1].SetActive(false);
                    backs[2].SetActive(false);
                    hands[2].SetActive(true);
                    buttons[1].interactable = false;
                    buttons[2].interactable = true;
                    buttons[0].interactable = false;
                    break;
                default:
                    backs[0].SetActive(true);
                    hands[0].SetActive(false);
                    backs[1].SetActive(true);
                    hands[1].SetActive(false);
                    backs[2].SetActive(false);
                    hands[2].SetActive(true);
                    buttons[1].interactable = false;
                    buttons[2].interactable = true;
                    buttons[0].interactable = false;
                    break;
            }
        }
    }
}
