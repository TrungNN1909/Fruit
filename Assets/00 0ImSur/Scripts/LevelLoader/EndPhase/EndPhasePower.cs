//using System;
using System;
using System.Collections;
using System.Collections.Generic;
using Unicorn.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unicorn
{


    public class EndPhasePower : MonoBehaviour
    {
        private List<Power> powers;
        [SerializeField] List<ButtonPower> buttons;
        private Dictionary<int, int> randomPowerID;
        

        public void Init()
        {
            powers = PowerManager.Instance.powersList;
        }

        private void OnEnable()
        {
            SpawnPower();
            PlayingManager.Instance.CameraZoomIn();
        }

        //create powers in scroll view
        private void SpawnPower()
        {

            int i = 0;
            GenerateUniqueRandomNumbers(3, 0, 33);
            foreach (var pair in randomPowerID)
            {
                buttons[i].id = pair.Key;
                buttons[i].img.sprite = powers[pair.Value].avatar;
                buttons[i].frame.sprite = powers[pair.Value].frame;
                buttons[i].img.SetNativeSize();
                buttons[i].descriptionText.text = powers[pair.Value].description;
                i++;
            }
            
            //tutorial
            if (PlayerDataManager.Instance.GetStage() == 0)
            {
                if (PlayingManager.Instance.GetCurrentPhaseNumber() == 0)
                {
                    buttons[2].id = 2;
                    buttons[2].img.sprite = powers[8].avatar;
                    buttons[2].frame.sprite = powers[8].frame;
                    buttons[2].img.SetNativeSize();
                    buttons[2].descriptionText.text = powers[8].description;
                }
                else if(PlayingManager.Instance.GetCurrentPhaseNumber() == 1)
                {
                    buttons[0].id = 10;
                    buttons[0].img.sprite = powers[33].avatar;
                    buttons[0].frame.sprite = powers[33].frame;
                    buttons[0].img.SetNativeSize();
                    buttons[0].descriptionText.text = powers[33].description;
                }else if (PlayingManager.Instance.GetCurrentPhaseNumber() >= 2)
                {
                    buttons[1].id = 10;
                    buttons[1].img.sprite = powers[33].avatar;
                    buttons[1].frame.sprite = powers[33].frame;
                    buttons[1].img.SetNativeSize();
                    buttons[1].descriptionText.text = powers[33].description;
                }
            }else if (PlayerDataManager.Instance.GetStage() == 1)
            {
                if (PlayingManager.Instance.GetCurrentPhaseNumber() == 0)
                {
                    buttons[0].id = 8;
                    buttons[0].img.sprite = powers[29].avatar;
                    buttons[0].frame.sprite = powers[29].frame;
                    buttons[0].img.SetNativeSize();
                    buttons[0].descriptionText.text = powers[29].description;
                }
                if (PlayingManager.Instance.GetCurrentPhaseNumber() == 1)
                {
                    buttons[1].id = 9;
                    buttons[1].img.sprite = powers[31].avatar;
                    buttons[1].frame.sprite = powers[31].frame;
                    buttons[1].img.SetNativeSize();
                    buttons[1].descriptionText.text = powers[31].description;
                }
                if (PlayingManager.Instance.GetCurrentPhaseNumber() == 0)
                {
                    buttons[2].id = 5;
                    buttons[2].img.sprite = powers[13].avatar;
                    buttons[2].frame.sprite = powers[13].frame;
                    buttons[2].img.SetNativeSize();
                    buttons[2].descriptionText.text = powers[13].description;
                }
            }

        }

        public void ResetButtonOnClick()
        {
            SoundManager.Instance.PlaySoundButton();
            UnicornAdManager.ShowAdsReward(SpawnPower, Helper.ResetButtonAd);
        }

        public void CollectAllButton()
        {
            UnicornAdManager.ShowAdsReward(CollectAll, Helper.CollectAllButtonAd);
        }

        private void CollectAll()
        {
            foreach (var bt in buttons)
            {
                bt.Init();
                bt.SetPower();
            }
            SoundManager.Instance.PlaySoundReward();
            PlayingManager.Instance.LoadPhase();
            GameManager.Instance.GamePlayController.OpenUINewPhase(false);

        }

        private void GenerateUniqueRandomNumbers(int count, int minValue, int maxValue)
        {
            randomPowerID = new Dictionary<int, int>();
            while (randomPowerID.Count < count)
            {
                int randomNumber = Random.Range(minValue, maxValue + 1);
                
                if (!randomPowerID.ContainsKey(powers[randomNumber].id))
                {
                    randomPowerID.Add(powers[randomNumber].id, randomNumber);
                }
            }
        }
    }
}

