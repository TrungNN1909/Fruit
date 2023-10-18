using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unicorn.Utilities
{
    /// <summary>
    /// Key và Analytic strings để hết ở đây
    /// </summary>
    /// 
    public static class Helper
    {

        //template
        public const string DataLevel = "DataLevel";
        public const string DataMaxLevelReached = "DataMaxLevelReached";
        public const string DataTypeSkin = "DataTypeSkin";
        public const string DataTypePet = "DataTypePet";
        public const string DataTypeHat = "DataTypeHat";
        public const string DataEquipHat = "DataEquipHat";
        public const string DataEquipSkin = "DataEquipSkin";
        public const string DataEquipPet = "DataEquipPet";
        public const string DataNumberWatchVideo = "DataNumberWatchVideo";
        public const string GOLD = "GOLD";

        public const string KEY = "KEY";
        public const string CurrentRewardEndGame = "CurrentRewardEndGame";
        public const string ProcessReceiveEndGame = "ProcessReceiveEndGame";

        public const string inter_end_game_lose = "inter_end_game_lose";
        public const string inter_end_game_win = "inter_end_game_win";

        public const string video_shop_general = "video_shop_{0}";
        public const string video_reward_end_game = "video_reward_end_game";
        public const string video_reward_chest_key = "video_reward_chest_key";
        public const string video_reward_lucky_wheel = "video_reward_lucky_wheel";
        public const string video_reward_revive = "video_reward_revive";
        public const string video_reward_x3_gold_end_game = "video_x3_gold_end_game";
        public const string video_reward_gift_box = "video_reward_gift_box";


        public const string SoundSetting = "SoundSetting";
        public const string MusicSetting = "MusicSetting";

        //extra code
        public const string Stage = "Stage";
        
        public const string GunLevel = "GunLevel";
        public const string SubGunLevel = "SubGunLevel";

        public const string GunLockSlot = "GunSlotLock_";
        public const string SwitchGunBuy = "SwitchGunBuy";
        public const string TotalStoredGunCount = "TotalStoredGunCount";
        public const string GunSlot = "GunSlot_";
        public const string GunSlotLevel = "GunSlotLevel_";
        public const string GunSlotType = "GunSlotType_";
        public const string GunHightestLevel = "GunHightestLevel";
        public const string SubGunHighestLevel = "SubGunHighestLevel";
        public const string MainGunCardTaken = "MainGunCardTaken_";
        public const string SupGunCardTaken = "SupGunCardTaken_";

        public const string Coin = "COIN";
        public const string TimesPopUpCoin = "TimesPopUpCoin";
        public const string PopUpCoinDayCheck = "PopUpCoinDayCheck";
        public const string PopUpCoinShowTimeCheck = "PopUpCoinShowTimeCheck";
        
        public const string GiftCardTaken = "GiftCardTaken_";
        public const string TotalGiftGunStoreList = "TotalGiftGunStoreList";
        public const string GunStored = "GunStored_";
        public const string GiftIndex = "GiftIndex";

        public const string CarLevel = "CarLevel";
        public const string CarHighestLevel = "CarHighestLevel";
        public const string CarCurrentHP = "CarCurrentHP";
        public const string TimeToNextLevel = "TimeToNextLevel";

        public const string CatSlotID = "CatSlotID_";
        public const string CatSlotOpen = "CatSlotOpenID_";
        public const string MainCatChosingID = "MainCatChosingID";
        public const string SubCatChosingID = "SubCatChosingID";
        public const string CatSlotMaxOpen = "CatSLotMaxOpen";
        public const string VibrationSetting = "VibrationSetting";
        
        public const string NumberOfPhase = "NumberOfPhase";

        public const string TimeUseFreeSkill = "TimeUseFreeSkill";
        //watch ad
        public const string buyGunAd = "BuyGunReward";
        public const string forceBarRewardAd = "ForceBarRewardReward";
        public const string skillAd = "SkillReward";
        public const string ReviveAd = "ReviveReward";
        public const string EndStage = "EndStageAdInter";
        public const string EndStageWinNothanks = "EndStageWinNoThankInter";
        public const string EndStageLoseNothanks = "EndStageWinNoThankInter";
        public const string ResetButtonAd = "ResetButtonRewawrd";
        public const string CollectAllButtonAd = "CollectAllButtonReward";
        public const string DailyGiftAd = "DailyGiftReward";
        public const string ArchivementAd = "ArchivementReward";
        public const string OpenGunSlotAd = "OpenGunSlotReward";
        public const string PopUpLuckShop = "PopupLuckShopReward";
        public const string PopupCoin = "PopupCoin";
        
        public static string FormatTime(int minute, int second, bool isSpaceSpecial = false)
        {
            StringBuilder sb = new StringBuilder();
            if (minute < 10)
            {
                sb.Append("0");
            }

            sb.Append(minute);
            if (isSpaceSpecial)
            {
                sb.Append("M");
                sb.Append(" ");
            }
            else
            {
                sb.Append(":");
            }

            if (second < 10)
            {
                sb.Append("0");
            }

            sb.Append(second);
            if (isSpaceSpecial)
            {
                sb.Append("S");
            }

            return sb.ToString();
        }

        public static int GetRandomGoldReward()
        {
            return Random.Range(100, 250);
        }

        public static bool CheckNewDay(string stringTimeCheck)
        {
            if (string.IsNullOrEmpty(stringTimeCheck))
            {
                return true;
            }

            try
            {
                DateTime timeNow = DateTime.Now;
                DateTime timeOld = DateTime.Parse(stringTimeCheck);
                DateTime timeOldCheck = new DateTime(timeOld.Year, timeOld.Month, timeOld.Day, 0, 0, 0);
                long tickTimeNow = timeNow.Ticks;
                long tickTimeOld = timeOldCheck.Ticks;

                long elapsedTicks = tickTimeNow - tickTimeOld;
                TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
                double totalDay = elapsedSpan.TotalDays;

                if (totalDay >= 1)
                {
                    return true;
                }
            }
            catch
            {
                return true;
            }

            return false;
        }

        public static void SetTextNumber(TextMeshProUGUI txt, int fromNum, int toNum, string unit = "", float duration = 0.3f, Action callback = null)
        {
            DOTween.To(() => fromNum, x =>
            {
                txt.text = $"{x.ToString()} {unit}";
            }, toNum, duration)
            .OnStart(() => txt.transform.localScale = Vector3.one * 1.2f)
            .OnComplete(() => txt.transform.DOScale(Vector3.one * 1.2f, 0.35f).From()
                                .SetEase(Ease.InBack)
                                .OnComplete(() => callback?.Invoke()));
        }
    }

}