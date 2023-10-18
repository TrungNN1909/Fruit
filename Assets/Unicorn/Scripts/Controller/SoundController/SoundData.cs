using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundData : MonoBehaviour
{
    public AudioClip CarStart;
    public AudioClip CarBrake;
    public AudioClip CarRun;

    public AudioClip CarHit;
    public AudioClip CatHit;

    public AudioClip AudioShoot;
    public AudioClip AudioSupportGun;
    
    public AudioClip SkillTornado;
    public AudioClip SkillFirePillar;
    public AudioClip SkillMateoroidAppear;
    public AudioClip SkillMateoroiImpact;
    public AudioClip SkillMagicNovation;

    public AudioClip CatDie;
    public AudioClip MonsterDie;

    public AudioClip UpgradeCar;
    public AudioClip MergeGun;
    public AudioClip Reward;
    public AudioClip CatOpen;

    public AudioClip MonsterAttack;
    //PREVIOUS CODE
    public AudioClip AudioClickBtn;

    public AudioClip AudioRevive;
    public AudioClip AudioReward;
    public AudioClip AudioSpinWheel;
    public AudioClip AudioStartCrewmate;
    public AudioClip AudioStartImpostor;
    public AudioClip AudioWin;
    public AudioClip AudioLose;
    public AudioClip AudioClockTick;
    public AudioClip AudioRewardClick;

    public AudioClip[] AudiosLobby;
    public AudioClip[] AudioBgs;
    public AudioClip AudioOverTime;

    [Title("Collectibles")]
    public List<AudioClip> ListAudioCollects;




}
