using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Unicorn
{
    public class SettingScreen : MonoBehaviour
    {
        [SerializeField] private Button backgroundButton;
        [SerializeField] private Button backButton;
        [SerializeField] private Button musicOnBtn;
        [SerializeField] private Button musicOffBtn;
        [SerializeField] private Button soundOnBtn;
        [SerializeField] private Button soundOffBtn;
        [SerializeField] private Button vibrationOnBtn;
        [SerializeField] private Button vibrationOffBtn;
        [SerializeField] private GameObject tab;
        public bool isMusicOn;
        public bool isSoundOn;
        public bool isVibrationOn;

        private bool isClosing;
        
        private void Start()
        {
            backgroundButton.onClick.AddListener(BackgroundButtonOnClick);
            backButton.onClick.AddListener(BackgroundButtonOnClick);
            musicOffBtn.onClick.AddListener(MusicButtonOffOnClick);
            musicOnBtn.onClick.AddListener(MusicButtonOnOnClick);
            soundOnBtn.onClick.AddListener(SoundOnOnClick);
            soundOffBtn.onClick.AddListener(SoundOffOnClick);
            vibrationOnBtn.onClick.AddListener(VibrationOnOnClick);
            vibrationOffBtn.onClick.AddListener(VibrationOffOnClick);
        }

        private void OnEnable()
        {
            isClosing = false;
            GameManager.Instance.HomeController.uiHome.HomeUIDisPlay(false);
            //music preset
            isMusicOn = PlayerDataManager.Instance.GetMusicSetting();
            SoundManager.Instance.SettingMusic(isMusicOn);
            if (isMusicOn)
            {
                musicOffBtn.gameObject.SetActive(false);
                musicOnBtn.gameObject.SetActive(true);
            }
            else
            {
                musicOffBtn.gameObject.SetActive(true);
                musicOnBtn.gameObject.SetActive(false);
            }
            
            //sound preset
            isSoundOn = PlayerDataManager.Instance.GetSoundSetting();
            SoundManager.Instance.SettingFxSound(isSoundOn);
            if (isSoundOn)
            {
                soundOnBtn.gameObject.SetActive(true);
                soundOffBtn.gameObject.SetActive(false);
            }
            else
            {
                soundOnBtn.gameObject.SetActive(false);
                soundOffBtn.gameObject.SetActive(true);
            }

            isVibrationOn = PlayerDataManager.Instance.GetVibrationSetting();
            if (isVibrationOn)
            {
                vibrationOnBtn.gameObject.SetActive(true);
                vibrationOffBtn.gameObject.SetActive(false);
            }
            else
            {
                vibrationOnBtn.gameObject.SetActive(false);
                vibrationOffBtn.gameObject.SetActive(true);
            }
        }

        private void OnDisable()
        {
            GameManager.Instance.HomeController.uiHome.HomeUIDisPlay(true);
        }

        private void VibrationOffOnClick()
        {
            StoredVibrationSetting();
            vibrationOnBtn.gameObject.SetActive(true);
            vibrationOffBtn.gameObject.SetActive(false);
        }

        private void VibrationOnOnClick()
        {            
            StoredVibrationSetting();
            vibrationOnBtn.gameObject.SetActive(false);
            vibrationOffBtn.gameObject.SetActive(true);
        }

        private void SoundOffOnClick()
        {
            StoreSoundSetting();
            soundOffBtn.gameObject.SetActive(false);
            soundOnBtn.gameObject.SetActive(true);
        }

        private void SoundOnOnClick()
        {
            StoreSoundSetting();
            soundOnBtn.gameObject.SetActive(false);
            soundOffBtn.gameObject.SetActive(true);
        }

        private void MusicButtonOnOnClick()
        {
            StoredMusicSetting();
            musicOffBtn.gameObject.SetActive(true);
            musicOnBtn.gameObject.SetActive(false);
        }

        private void MusicButtonOffOnClick()
        {
            StoredMusicSetting();
            musicOffBtn.gameObject.SetActive(false);
            musicOnBtn.gameObject.SetActive(true);
        }

        private void BackgroundButtonOnClick()
        {
            if(isClosing) return;
            
            isClosing = true;
            GameManager.Instance.Resume();
            tab.transform.DOScale(0.3f, 0.75f).SetEase(Ease.InBack).OnComplete(() =>
            {
                gameObject.SetActive(false);
                tab.transform.localScale = Vector3.one;
            });
        }

        private void StoredMusicSetting()
        {
            isMusicOn = !isMusicOn;
            PlayerDataManager.Instance.SetMusicSetting(isMusicOn);
            SoundManager.Instance.SettingMusic(isMusicOn);
        }

        private void StoreSoundSetting()
        {
            isSoundOn = !isSoundOn;
            PlayerDataManager.Instance.SetSoundSetting(isSoundOn);
            SoundManager.Instance.SettingFxSound(isSoundOn);
        }

        private void StoredVibrationSetting()
        {
            isVibrationOn = !isVibrationOn;
            PlayerDataManager.Instance.SetVibrationSetting(isVibrationOn);
        }
    }
}
