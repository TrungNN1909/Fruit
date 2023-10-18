using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unicorn.UI.Shop;
using Unicorn.Utilities;
using UnityEngine;
using UnityEngine.Lumin;
using Random = UnityEngine.Random;

namespace Unicorn
{
    /// <summary>
    /// Lưu giữ toàn bộ data game. 
    /// </summary>
    public class PlayerDataManager : MonoBehaviour, IDataSkin
    {
        public static PlayerDataManager Instance;

        public Action<TypeItem> actionUITop;
        public DataTexture DataTexture;
        public DataTextureSkin DataTextureSkin;
        public DataRewardEndGame DataRewardEndGame;
        public DataLuckyWheel DataLuckyWheel;

        public GunListAsset GunListAsset;
        public SubGunListAsset subGunListAsset;
        public CarListAsset CarListAsset;
        public BackgroundListAsset backgroundListAsset;
        public CatListAsset catListAsset;
        public GiftListAsset giftListAsset;
        public ArchivementListAsset archivementListAsset;
        public LuckShopItemListAsset luckShopItemListAsset;
        
        
        private IDataLevel unicornDataLevel;
        //private Dictionary<int, float> redLightTimes;


        private void Awake()
        {
            Instance = this;
            unicornDataLevel = null;
        }

        public void SetStage(int stageNumber)
        {
            PlayerPrefs.SetInt(Helper.Stage,stageNumber);
        }

        public void SetCurrentNumberOfPhase(int index)
        {
            PlayerPrefs.SetInt(Helper.NumberOfPhase,index);            
        }

        public int GetCurrentNumberOfPhase()
        {
            return PlayerPrefs.GetInt(Helper.NumberOfPhase,0);
        }

        public int GetStage()
        {
            return PlayerPrefs.GetInt((Helper.Stage),0);
        }
        
        //car
        public void SetCurrentCar(int carLevel)
        {
            PlayerPrefs.SetInt(Helper.CarLevel, carLevel);
        }

        public CarInfo GetCurrentCar(int carlevel)
        {
            CarInfo car = CarListAsset.data[carlevel];
            return car;
        }

        public void SetCarHighestLevel(int level)
        {
            PlayerPrefs.SetInt(Helper.CarHighestLevel, level);
        }

        public int GetCarHighestLevel()
        {
            return PlayerPrefs.GetInt(Helper.CarHighestLevel,0);
        }

        public void SetCarCurrentHP(float hp)
        {
            PlayerPrefs.SetFloat(Helper.CarCurrentHP,hp);
        }

        public float GetCarCurrentHP()
        {
            return PlayerPrefs.GetFloat(Helper.CarCurrentHP,200);
        }

        public void SetTimeToNextLevel(int times)
        {
            times = GetTimeToNextLevel() + times;
            PlayerPrefs.SetInt(Helper.TimeToNextLevel,times);
        }

        public int GetTimeToNextLevel()
        {
            return PlayerPrefs.GetInt(Helper.TimeToNextLevel,0);
        }
        //gun
        public void SetCurrentGun(int gunLevel)
        {
            PlayerPrefs.SetInt(Helper.GunLevel, gunLevel);
        }

        public GunInfos GetGun(int gunLevel)
        {
            GunInfos gi = GunListAsset.data[gunLevel];
            return gi;
        }

        public void SetCurrentSubGun(int subGunLevel)
        {
            PlayerPrefs.SetInt(Helper.SubGunLevel, subGunLevel);
        }

        public SubGunInfo GetSubGun(int subGunLevel)
        {
            SubGunInfo gi = subGunListAsset.data[subGunLevel];
            return gi;
        }

        public void SetDataLevel(IDataLevel unicornDataLevel)
        {
            this.unicornDataLevel = unicornDataLevel;
            PlayerPrefs.SetString(Helper.DataLevel, JsonConvert.SerializeObject(unicornDataLevel));
        }


        // Upgrade data

        public void SetSlotStatus(int id, bool isLock)
        {
            if(isLock) PlayerPrefs.SetInt(Helper.GunLockSlot + id,1); // lock == 1;
            else
            {
                PlayerPrefs.SetInt(Helper.GunLockSlot + id,0);// lockn't == 0;
            }
        }

        public bool GetSlotStatus(int id)
        {
            if (id <= 9) return false;
            
            if (PlayerPrefs.GetInt(Helper.GunLockSlot + id, 1) == 1) return true; // locked --> false
            return false ; // unlock --> true;
        }
        
        public void SetStoredSlotCount(int slotCount)
        {
            PlayerPrefs.SetInt(Helper.TotalStoredGunCount, slotCount);
        }

        public int GetStoredSlotCount()
        {
            return PlayerPrefs.GetInt(Helper.TotalStoredGunCount,18);
        }

        // store gun slot ==> slot ID, level of gun, type of gun
        public void SetStoredSlots(List<GunSlot> slots)
        {
            foreach(GunSlot gunSlot in slots)
            {
                PlayerPrefs.SetInt(Helper.GunSlot + gunSlot.GetSlotID(), gunSlot.GetSlotID());
                PlayerPrefs.SetInt(Helper.GunSlotLevel + gunSlot.GetSlotID(), gunSlot.GetGunSlotLevel());
                PlayerPrefs.SetInt(Helper.GunSlotType + gunSlot.GetSlotID() , gunSlot.GetTypeGunSlot());
            }
        }

        public List<GunSlot> GetStoredSlots()
        {
            List<GunSlot> list = new List<GunSlot>();

            for(int i=0; i<GetStoredSlotCount(); i++)
            {
                list.Add(new GunSlot(
                    PlayerPrefs.GetInt(Helper.GunSlot + i, i),
                    PlayerPrefs.GetInt(Helper.GunSlotType + i, -1),
                    PlayerPrefs.GetInt(Helper.GunSlotLevel + i, -1)
                    ));
            }
            return list;
        }

        public void SetGunHighestLevel(int level)
        {
            PlayerPrefs.SetInt(Helper.GunHightestLevel, level);
        }

        public void SetSubGunHighestLevel(int level)
        {
            PlayerPrefs.SetInt(Helper.SubGunHighestLevel, level);
        }

        public int GetGunHighestLevel()
        {
            return PlayerPrefs.GetInt(Helper.GunHightestLevel,1);
        }

        public int GetSubGunHighestLevel()
        {
            return PlayerPrefs.GetInt(Helper.SubGunHighestLevel,1);
        }


        //archivement data
        public void SetMainGunCardTaken(int CardNumber, bool isTaken)
        {
            if (isTaken) // earned
                PlayerPrefs.SetInt(Helper.MainGunCardTaken + CardNumber, 1);
            else
                PlayerPrefs.SetInt(Helper.MainGunCardTaken + CardNumber, 0);
        }

        public bool GetMainGunCardTaken(int CardNumber)
        {
            if (PlayerPrefs.GetInt(Helper.MainGunCardTaken + CardNumber,0) == 1)
                return true;
            return false;
        }

        public void SetSupGunCardTaken(int CardNumber, bool isTaken)
        {
            if (isTaken) // earned
                PlayerPrefs.SetInt(Helper.SupGunCardTaken + CardNumber, 1);
            else
                PlayerPrefs.SetInt(Helper.SupGunCardTaken + CardNumber, 0);
        }

        public bool GetSuPGunCardTaken(int CardNumber)
        {
            if (PlayerPrefs.GetInt(Helper.SupGunCardTaken + CardNumber,0) == 1)
                return true;
            return false;
        }

        //GiftData
        public void SetGiftCardTaken(int cardNumber, bool isTaken)
        {
            if (isTaken)
            {
                PlayerPrefs.SetInt(Helper.GiftCardTaken + cardNumber, 1);
            }
            else
            {
                PlayerPrefs.SetInt(Helper.GiftCardTaken + cardNumber, 0);
            }
        }

        public bool GetGiftCardTaken(int cardNumber)
        {
            return PlayerPrefs.GetInt(Helper.GiftCardTaken + cardNumber,0) == 1;
        }

        public void SetTotalGiftGunStored(int index)
        {
            int total = GetTotalGiftGunStored() + index;
            PlayerPrefs.SetInt(Helper.TotalGiftGunStoreList, total);
        }

        public int GetTotalGiftGunStored()
        {
            return PlayerPrefs.GetInt(Helper.TotalGiftGunStoreList,0);
        }

        public void SetGiftGunLevel(int index, int level)
        {
            PlayerPrefs.SetInt(Helper.GunStored + index, level);
        }

        public int GetGiftGunLevel(int index)
        {
            return PlayerPrefs.GetInt(Helper.GunStored + index,1);
        }

        public void SetGiftIndex(int index)
        {
            index = index + GetGiftIndex();
            PlayerPrefs.SetInt(Helper.GiftIndex, index);
        }

        public int GetGiftIndex()
        {
            return PlayerPrefs.GetInt(Helper.GiftIndex, 0);
        }

        //ShopCat

        public void SetCatSlotOpen(int slotID, bool isOpen)
        {
            if (isOpen)
            {
                PlayerPrefs.SetInt(Helper.CatSlotOpen + slotID, 1);
            }
            else
            {
                PlayerPrefs.SetInt(Helper.CatSlotOpen + slotID, 0);

            }
        }

        public bool GetCatSlotOpen(int slotID)
        {
            if (PlayerPrefs.GetInt(Helper.CatSlotOpen + slotID,0) == 1)
            {
                return true; 
            }
            return false;
        }

        public void SetMainCatChossen(int slotID)
        {
            PlayerPrefs.SetInt(Helper.MainCatChosingID, slotID);
        }

        public int GetMainCatChossen()
        {
            return PlayerPrefs.GetInt(Helper.MainCatChosingID,0);
        }

        public void SetSubCatChossen(int slotID)
        {
            PlayerPrefs.SetInt(Helper.SubCatChosingID, slotID);
        }

        public int GetSubCatChossen()
        {
            return PlayerPrefs.GetInt(Helper.SubCatChosingID,0);
        }

        public void SetMaxCatSlotOpen(int maxSlot)
        {
            PlayerPrefs.SetInt(Helper.CatSlotMaxOpen, maxSlot); 
        }

        public int GetMaxCatSlotOpen()
        {
            return PlayerPrefs.GetInt(Helper.CatSlotMaxOpen, 1);
        }
        // COIN
        public void SetCoin(int coin)
        {
            coin += GetCoin();
            PlayerPrefs.SetInt(Helper.Coin, coin);
        }

        public int GetCoin()
        {
            return PlayerPrefs.GetInt(Helper.Coin, 10000);
        }

        public void SetTimeEarnPopUoCoin()
        {
            PlayerPrefs.SetString(Helper.PopUpCoinDayCheck,DateTime.Now.ToString());
            int times = GetTimeEarnPopUpCoin();
            
            PlayerPrefs.SetInt(Helper.TimesPopUpCoin, ++times);

        }

        public int GetTimeEarnPopUpCoin()
        {
            if (Helper.CheckNewDay(PlayerPrefs.GetString(Helper.PopUpCoinDayCheck, "")))
            {
                
                return 0;
            }
            
            return PlayerPrefs.GetInt(Helper.TimesPopUpCoin, 0);
        }

        public void SetTimeShowPopUpCoin(string time)
        {
            PlayerPrefs.SetString(Helper.PopUpCoinDayCheck,time);
        }

        public string GetTimesShowPopUpCoin()
        {
            return PlayerPrefs.GetString(Helper.PopUpCoinDayCheck, "");
        }

        //previous code
        public IDataLevel GetDataLevel(LevelConstraint levelConstraint)
        {
            var dataLevelJson = PlayerPrefs.GetString(Helper.DataLevel, string.Empty);

            unicornDataLevel = dataLevelJson == string.Empty
                ? new UnicornDataLevel(levelConstraint)
                : JsonConvert.DeserializeObject<UnicornDataLevel>(dataLevelJson);

            return unicornDataLevel ?? new UnicornDataLevel(levelConstraint);
        }

        public int GetMaxLevelReached()
        {
            return PlayerPrefs.GetInt(Helper.DataMaxLevelReached, 1);
        }

        public bool GetUnlockSkin(TypeEquipment type, int id)
        {
            return PlayerPrefs.GetInt(Helper.DataTypeSkin + type + id, 0) == 0 ? false : true;
        }

        public void SetUnlockSkin(TypeEquipment type, int id)
        {
            PlayerPrefs.SetInt(Helper.DataTypeSkin + type + id, 1);
            SetIdEquipSkin(type, id);
        }

        public int GetIdEquipSkin(TypeEquipment type)
        {
            return PlayerPrefs.GetInt(Helper.DataEquipSkin + type, -1);
        }

        public void SetIdEquipSkin(TypeEquipment type, int id)
        {
            PlayerPrefs.SetInt(Helper.DataEquipSkin + type, id);
        }

        public int GetVideoSkinCount(TypeEquipment type, int id)
        {
            return PlayerPrefs.GetInt(Helper.DataNumberWatchVideo + type + id, 0);
        }

        public void SetVideoSkinCount(TypeEquipment type, int id, int number)
        {
            PlayerPrefs.SetInt(Helper.DataNumberWatchVideo + type + id, number);
        }

        public int GetGold()
        {
            return PlayerPrefs.GetInt(Helper.GOLD, 0);
        }

        public void SetGold(int _count)
        {
            PlayerPrefs.SetInt(Helper.GOLD, _count);
        }

        public int GetKey()
        {
            return PlayerPrefs.GetInt(Helper.KEY, 0);
        }

        public void SetKey(int _count)
        {
            PlayerPrefs.SetInt(Helper.KEY, _count);
        }

        public int GetCurrentIndexRewardEndGame()
        {
            return PlayerPrefs.GetInt(Helper.CurrentRewardEndGame, 0);
        }

        public void SetCurrentIndexRewardEndGame(int index)
        {
            PlayerPrefs.SetInt(Helper.CurrentRewardEndGame, index);
        }

        public int GetProcessReceiveRewardEndGame()
        {
            return PlayerPrefs.GetInt(Helper.ProcessReceiveEndGame, 0);
        }

        public void SetProcessReceiveRewardEndGame(int number)
        {
            PlayerPrefs.SetInt(Helper.ProcessReceiveEndGame, number);
        }


        public int GetNumberWatchDailyVideo()
        {
            return PlayerPrefs.GetInt("NumberWatchDailyVideo", DataLuckyWheel.NumberSpinDaily);
        }

        public void SetNumberWatchDailyVideo(int number)
        {
            PlayerPrefs.SetInt("NumberWatchDailyVideo", number);
        }

        public bool GetFreeSpin()
        {
            return PlayerPrefs.GetInt("FreeSpin", 1) > 0 ? true : false;
        }

        public void SetFreeSpin(bool isFree)
        {
            int free = isFree ? 1 : 0;
            PlayerPrefs.SetInt("FreeSpin", free);
        }

        public int GetNumberWatchVideoSpin()
        {
            return PlayerPrefs.GetInt("NumberWatchVideoSpin", 0);

        }

        public void SetNumberWatchVideoSpin(int count)
        {
            PlayerPrefs.SetInt("NumberWatchVideoSpin", count);
        }

        public string GetTimeLoginSpinFreeWheel()
        {
            return PlayerPrefs.GetString("TimeSpinFreeWheel", "");
        }

        public void SetTimeLoginSpinFreeWheel(string time)
        {
            PlayerPrefs.SetString("TimeSpinFreeWheel", time);
        }

        public string GetTimeLoginSpinVideo()
        {
            return PlayerPrefs.GetString("TimeLoginSpinVideo", "");
        }

        public void SetTimeLoginSpinVideo(string time)
        {
            PlayerPrefs.SetString("TimeLoginSpinVideo", time);
        }

        public void SetSoundSetting(bool isOn)
        {
            PlayerPrefs.SetInt(Helper.SoundSetting, isOn ? 1 : 0);
        }

        public bool GetSoundSetting()
        {
            return PlayerPrefs.GetInt(Helper.SoundSetting, 1) == 1;
        }

        public void SetMusicSetting(bool isOn)
        {
            PlayerPrefs.SetInt(Helper.MusicSetting, isOn ? 1 : 0);
        }

        public bool GetMusicSetting()
        {
            return PlayerPrefs.GetInt(Helper.MusicSetting, 1) == 1;

        }

        public void SetVibrationSetting(bool isOn)
        {
            PlayerPrefs.SetInt(Helper.VibrationSetting, isOn ? 1: 0);
        }

        public bool GetVibrationSetting()
        {
            return PlayerPrefs.GetInt(Helper.VibrationSetting, 1) == 1;
        }
        
        public bool IsNoAds()
        {
            return PlayerPrefs.GetInt("NoAds", 0) == 1;
        }

        public void SetNoAds()
        {
            PlayerPrefs.SetInt("NoAds", 1);
        }

        private List<int> listIdSkin = new List<int>();

        public int GetIdSkinOtherPlayer()
        {
            if (listIdSkin.Count == 0)
            {
                for (int i = 1; i < Enum.GetNames(typeof(Skin)).Length; i++)
                {
                    listIdSkin.Add(i);
                }
            }

            var index = Random.Range(0, listIdSkin.Count);
            int id = listIdSkin[index];
            listIdSkin.RemoveAt(index);

            return id;
        }

        public void ClearListIdSkin()
        {
            if (listIdSkin.Count > 0)
                listIdSkin.Clear();
        }

        public void SetNumberPlay(int num)
        {
            PlayerPrefs.SetInt("NumberPlay", num);
        }

        public int GetNumberPlay()
        {
            return PlayerPrefs.GetInt("NumberPlay", 0);
        }

        public string GetTimeLoginOpenGift()
        {
            return PlayerPrefs.GetString("TimeLoginOpenGift", "");
        }

        public void SetTimeLoginOpenGift(string time)
        {
            PlayerPrefs.SetString("TimeLoginOpenGift", time);
        }

        public void SetTimeUseFreeSkills(string time)
        {
            PlayerPrefs.SetString(Helper.TimeUseFreeSkill,time);
        }

        public string GetTimeUseFreeSkills()
        {
            return PlayerPrefs.GetString(Helper.TimeUseFreeSkill,"");
        }
        
        
    }

}