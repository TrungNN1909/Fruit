using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace Unicorn
{
    /// <summary>
    /// Class sida phết, nếu dùng được thì dùng nhé, không có thì thôi (✿◡‿◡)
    /// dùng được mà :-(
    /// </summary>
    [Singleton("SoundManager", true)]
    public class SoundManager : Singleton<SoundManager>
    {
        public enum GameSound
        {
            BGM,
            Footstep,
            Spin,
            Lobby,
            ClockTick,
            RewardClick,
            Ingame,
            Win,
            Lose
        }
        
        [SerializeField] public SoundData soundData;

        public AudioMixer audioMixer;
        public AudioSource bgMusic;
        public AudioSource fxSound;
        public AudioSource fxSoundFootStep;
        public AudioSource clockTickFast;
        public AudioSource coffinSource;
        public AudioSource carRunSound;
        public AudioSource shootSound;
        
        private float bgVol;
        private bool isPlayFootStep;

        #region UNITY METHOD

        private void Start()
        {
            SettingFxSound(PlayerDataManager.Instance.GetSoundSetting());
            SettingMusic(PlayerDataManager.Instance.GetMusicSetting());
            isPlayFootStep = false;
        }

        #endregion

        #region PUBLIC METHOD

        public void PlayFxSound(Enum soundEnum)
        {
            switch (soundEnum)
            {
                case LevelResult levelResult:
                {
                    switch (levelResult)
                    {
                        case LevelResult.Win:
                            PlaySoundWin();
                            break;
                        case LevelResult.Lose:
                            PlaySoundLose();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                }
                case GameSound gameSound:
                {
                    switch (gameSound)
                    {
                        case GameSound.BGM:
                            PlayBGM(Random.Range(0, soundData.AudioBgs.Length));
                            break;
                        case GameSound.Footstep:
                            PlayFootStep();
                            break;
                        case GameSound.Spin:
                            PlaySoundSpin();
                            break;
                        case GameSound.Lobby:
                            PlayBGM(soundData.AudiosLobby[0]);
                            break;
                        case GameSound.ClockTick:
                            PlayFxSound(soundData.AudioClockTick);
                            break;
                        case GameSound.RewardClick:
                            PlayFxSound(soundData.AudioRewardClick);
                            break;
                        case GameSound.Ingame:
                            PlayBGM(soundData.AudioBgs[Random.Range(0, soundData.AudioBgs.Length)]);
                            break;
                        case GameSound.Win:
                            PlaySoundWin();
                            break;
                        case GameSound.Lose:
                            PlaySoundLose();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                }
                case TypeSoundIngame collectibleSound:
                {
                    PlaySoundCollectible(collectibleSound);
                    break;
                }
                default:
                    PlayFxSound(soundEnum, fxSound);
                    break;
            }
        }

        public void PlayFxSound(Enum soundEnum, AudioSource audioSource)
        {
            switch (soundEnum)
            {
                default:
                    throw new InvalidEnumArgumentException($"{soundEnum} is not supported");
            }
        }

        public void StopSound(Enum soundEnum)
        {
            
            switch (soundEnum)
            {
                case GameSound gameSound:
                {
                    switch (gameSound)
                    {
                        case GameSound.Lobby:
                            StopBGM();
                            break;
                        case GameSound.BGM:
                            StopBGM();
                            break;
                        case GameSound.Footstep:
                            StopFootStep();
                            break;
                        case GameSound.Spin:
                            StopFxSound();
                            break;
                        case GameSound.ClockTick:
                            StopFxSound();
                            Vibration.Vibrate(10);
                            break;
                        case GameSound.RewardClick:
                            StopFxSound();
                            break;
                        case GameSound.Ingame:
                            StopBGM();
                            break;
                        case GameSound.Win:
                            StopBGM();
                            break;
                        case GameSound.Lose:
                            StopBGM();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                }
                case LevelResult levelResult:
                    switch (levelResult)
                    {
                        case LevelResult.Win:
                            StopBGM();
                            break;
                        case LevelResult.Lose:
                            StopBGM();

                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                case TypeSoundIngame collectibleSound:
                {
                    StopFxSound();
                    break;
                }
                default:
                    throw new InvalidEnumArgumentException($"{soundEnum} is not supported");
            }
        }

        public void SettingFxSound(bool isOn)
        {
            var vol = isOn ? 1 : 0;
            fxSound.volume = vol;
            fxSoundFootStep.volume = vol;
            fxSound.mute = !isOn;
            fxSoundFootStep.mute = !isOn;
            coffinSource.volume = vol;
            coffinSource.mute = !isOn;
            carRunSound.volume = vol;
            carRunSound.mute = !isOn;
            shootSound.volume = vol;
            shootSound.mute = !isOn;
        }

        public void SettingMusic(bool isOn)
        {
            bgVol = isOn ? 1 : 0;
            bgMusic.volume = bgVol;
            bgMusic.mute = !isOn;
        }

        #endregion

        public void PlayCarStartRunSound()
        {
            PlayFxSound(soundData.CarStart);
        }
        
        public void PlayCarRunSound()
        {
            carRunSound.loop = true;
            carRunSound.clip = soundData.CarRun;
            carRunSound.Play();
            carRunSound.volume = 0.3f;
        }

        public void StopCarRunSound()
        {
            carRunSound.DOFade(0, 0.75f).OnComplete(() =>
            {
                carRunSound.Stop();
                carRunSound.volume = 0.3f;
            });
        }
        
        public void PlayShotSupportSound()
        {
            shootSound.PlayOneShot(soundData.AudioSupportGun);
        }
        
        public void PlayCarBrakeSound()
        {
            PlayFxSound(soundData.CarBrake);
        }

        public void PlayCarHitSound()
        {
            PlayFxSound(soundData.CarHit);
        }

        public void PlayCatHitSound()
        {
            PlayFxSound(soundData.CatHit);
        }

        public void PlayShotSound()
        {
            shootSound.PlayOneShot(soundData.AudioShoot);
        }

        public void StopShotSound()
        {
            shootSound.Stop();
        }

        public void PlaySKillTornadoSound()
        {
            PlayFxSound(soundData.SkillTornado);
        }

        public void PlaySkillFirePillar()
        {
            PlayFxSound(soundData.SkillFirePillar);
        }

        public void PlaySkillMeteoroidAppear()
        {
            PlayFxSound(soundData.SkillMateoroidAppear);
        }

        public void PlaySkillMateoroidImpact()
        {
            PlayFxSound(soundData.SkillMateoroiImpact);
        }

        public void PlayMagicNovationImpact()
        {
            PlayFxSound(soundData.SkillMagicNovation);
        }

        public void PlaySoundMonsterAttac()
        {
            PlayFxSound(soundData.MonsterAttack);
        }
        public void PlayCatDieSound()
        {
            PlayFxSound(soundData.CatDie);
        }

        public void PlayMonsterDieSound()
        {
            PlayFxSound(soundData.MonsterDie);
        }

        public void PlayUpgradeCarSound()
        {
            PlayFxSound(soundData.UpgradeCar);
        }

        public void PlayMergeGunSound()
        {
            PlayFxSound(soundData.MergeGun);
        }

        public void PlayBuyCatSound()
        {
            PlayFxSound(soundData.CatOpen);
        }

        public void PlayRewardSound()
        {
            PlayFxSound(soundData.Reward);
        }

        public void PlayVibration()
        {
            if(PlayerDataManager.Instance.GetVibrationSetting())
                Vibration.Vibrate();
        }

        #region PRIVATE METHOD

        private void PlayFxSound(AudioClip clip, AudioSource audioSource)
        {
            audioSource.PlayOneShot(clip);
        }

        public bool IsOnVibration
        {
            get { return PlayerPrefs.GetInt("OnVibration", 1) == 1 ? true : false; }
        }

        private void PlayBGM(int index)
        {
            var backgroundMusics = soundData.AudioBgs;
            PlayBGM(backgroundMusics[index]);

        }

        private void PlayBGM(AudioClip audioClip)
        {
            StartCoroutine(WaitForPreviousSound(audioClip));
        }

        private IEnumerator WaitForPreviousSound(AudioClip audioClip)
        {
            yield return Yielders.Get(0.3f);
            bgMusic.loop = true;
            bgMusic.clip = audioClip;
            bgMusic.volume = 0;
            bgMusic.DOKill();
            bgMusic.DOFade(bgVol, 0.5f);
            bgMusic.Play();
        }
        

        private void StopBGM()
        {
            bgMusic.DOFade(0, 1f).OnComplete(action: () => bgMusic.Stop());
        }
        
        private void PlayClockTick(AudioClip clip)
        {
            clockTickFast.clip = clip;
            clockTickFast.Play();
        }

        private void PlayFxSound(AudioClip clip)
        {
            fxSound.PlayOneShot(clip);
        }

        private void StopFxSound()
        {
            fxSound.Stop();
        }

        public void PlayCoffinTheme(bool isPlaying, float delay = 0)
        {
            if (isPlaying)
            {
                audioMixer.DOSetFloat("BGMVol", -80, delay / 3 * 2).SetEase(Ease.InSine).SetDelay(delay / 3);
                audioMixer.DOSetFloat("FXVol", -80, delay / 3 * 2).SetEase(Ease.InSine).SetDelay(delay / 3)
                    .OnComplete(() => coffinSource.Play());
            }
            else
            {
                audioMixer.SetFloat("BGMVol", 0);
                audioMixer.SetFloat("FXVol", 0);
                coffinSource.Stop();
            }
        
        }

        public void PlaySoundButton()
        {
            coffinSource.PlayOneShot(soundData.AudioClickBtn);
        }

        public void PlaySoundSpin()
        {
            PlayFxSound(soundData.AudioSpinWheel);
        }

        public void PlaySoundRevive()
        {
            PlayFxSound(soundData.AudioRevive);
        }

        public void PlaySoundReward()
        {
            PlayFxSound(soundData.AudioReward);
        }

        public void PlaySoundStartCrewmate()
        {
            PlayFxSound(soundData.AudioStartCrewmate);
        }

        public void PlaySoundStartImpostor()
        {
            PlayFxSound(soundData.AudioStartImpostor);
        }

        public void PlaySoundWin()
        {
            PlayBGM(soundData.AudioWin);
        }

        public void PlaySoundLose()
        {
            PlayBGM(soundData.AudioLose);
        }

        public void PlaySoundCollectible(TypeSoundIngame typeSound)
        {
            PlayFxSound(soundData.ListAudioCollects[(int) typeSound - 1]);
        }

        public void PlayFootStep()
        {
            if (isPlayFootStep)
                return;

            isPlayFootStep = true;
            fxSoundFootStep.Play();

            Analytics.LogFirstLogJoystick();
        }

        public void StopFootStep()
        {
            fxSoundFootStep.Stop();
            isPlayFootStep = false;
        }

        public void PlaySoundOverTime()
        {
            fxSound.PlayOneShot(soundData.AudioOverTime);
        }

        #endregion
    }

}