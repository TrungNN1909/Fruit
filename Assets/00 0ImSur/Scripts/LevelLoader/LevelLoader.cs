using System.Collections;
using System.Collections.Generic;
using Unicorn.Utilities;
using UnityEngine;

namespace Unicorn
{

    [System.Serializable]
    public class Phase
    {
        public int numberOfBird;
        public int numberOfSnail;
        public int numberOfFlySlime;
        public int numberOfGroundSlime;
        public int numberOfFlyFly;
        public int numberOfFidd;
        public int numberOfPlantZombie;
        public int numberOfHuggyBoss;
        public int numberOfGreenBoss;
        public int numberOfBlueBoss;

    }


    public class LevelLoader : MonoBehaviour
    {
        public List<Phase> phaseList;
        public List<GameObject> listCurrentEnemy;

        private int currentNumberOfEnemy;
        
        public int index; // count phase;
        public int currentPhaseCount; // countStage

        public Coroutine tutorial;
        
        public void init()
        {
            listCurrentEnemy = new List<GameObject>();

            index = 0;
            phaseList = StageManager.instance.listStage[PlayerPrefs.GetInt("Stage")].phases;
            currentPhaseCount = phaseList.Count;
        }

        public void LoadGame()
        {
            if (PlayerDataManager.Instance.GetStage() == 0)
            {
                GameManager.Instance.GamePlayController.uiPlaying.skillsTab.gameObject.SetActive(false);
            }

            PlayingManager.Instance.isPhaseEnd = false;
            PlayingManager.Instance.isPlaying = true;
            PlayingManager.Instance.carMoveOutAction?.Invoke();
            PlayingManager.Instance.newPhaseStart?.Invoke();
            PlayingManager.Instance.backgroundSpeedUp?.Invoke();
            
            if (index < phaseList.Count)
            {
                if (index != 0)
                {
                    //if not first phase gas station move out
                    PlayingManager.Instance.gasStationMoveOutAction?.Invoke(40f);
                }
                else if (index == 0)
                {
                    PlayingManager.Instance.garageMoveOutAction?.Invoke(40f);
                }
                
                LoadPhase(phaseList[index]);
                tutorial = StartCoroutine(DelayStartTutorial());

                index++;

                if (index == phaseList.Count)
                {
                    PlayingManager.Instance.isStageEnd = true;
                }
            }
        }
        
        
        public void PhaseFinish()
        {
            //tutorial
            Debug.Log("Tutut");
            if (PlayerDataManager.Instance.GetStage() == 0 && index == 2)
            {
                PlayingManager.Instance.isPlaying = true;
                PlayingManager.Instance.isPhaseEnd = false;
                PlayingManager.Instance.isBackGoundMoving = true;
                PlayingManager.Instance.isCarMoving = true;
                Invoke(nameof(tutorialExtraPhase),2f);
                return;
            }
            
            PlayingManager.Instance.StoredNumberOfPhase();
            PlayingManager.Instance.gasFillerCrossOverAction?.Invoke();
            PlayingManager.Instance.carMoveInAction?.Invoke();
            PlayingManager.Instance.backgroundSlowdown?.Invoke();
            PlayingManager.Instance.skillMoveOutAction?.Invoke();
            PlayingManager.Instance.isPlaying = false;
            PlayingManager.Instance.EarnCoin();
            StopCoroutine(tutorial);
            // PlayingManager.Instance.gasStationMoveInAction?.Invoke();
            Invoke(nameof( GasStationAction),0.5f);
            SoundManager.Instance.StopShotSound();
            StartCoroutine(WaitToStartNewPhaseScreen());
        }

        private void GasStationAction()
        {
            PlayingManager.Instance.gasStationMoveInAction?.Invoke();
        }
        private void tutorialExtraPhase()
        {
            
            LoadPhase(phaseList[index]);
            index++;
        }
        
        public void StageFinish()
        {
            //TutorialUpgrande
            if(PlayerDataManager.Instance.GetStage() == 0)
                PlayerPrefs.SetInt("Tutorial", 1);
            
            //LOOP level
            PlayerDataManager.Instance.SetStage(PlayerDataManager.Instance.GetStage()+1);
            if (PlayerDataManager.Instance.GetStage() == 100)
                PlayerDataManager.Instance.SetStage(99);
            
            StopCoroutine(tutorial);
            index = 0;
            PlayingManager.Instance.StoredNumberOfPhase();
            PlayingManager.Instance.garageMoveInAction?.Invoke();
            PlayingManager.Instance.carMoveInAction?.Invoke();
            PlayingManager.Instance.backgroundSlowdown?.Invoke();
            PlayingManager.Instance.skillMoveOutAction?.Invoke();

            StartCoroutine(WaitToEndStage());
        }

        private BaseEnemy _baseEnemy;
        
        public void LoadPhase(Phase phase)
        {
            //preset
            if (phase.numberOfHuggyBoss != 0 ||
                phase.numberOfBlueBoss != 0 ||
                phase.numberOfGreenBoss != 0)
            {
                PlayingManager.Instance.bossStageAction?.Invoke(); 
            }
            
            PlayingManager.Instance.currentEnemies.Clear();
            
            // spawn monster
            for (int i = 0; i < phase.numberOfFlyFly; i++)
            {
                _baseEnemy = Pools.Instance.GetObjectFromPool("FlyFly").GetComponent<FlyFly>();
                Vector3 defaultPosition  =  _baseEnemy.spriteHolder.transform.position;
                _baseEnemy.spriteHolder.transform.position = new Vector3(defaultPosition.x, defaultPosition.y, i);
                PlayingManager.Instance.currentEnemies.Add(_baseEnemy);
            }

            for (int i = 0; i < phase.numberOfFidd; i++)
            {
                PlayingManager.Instance.currentEnemies.Add(Pools.Instance.GetObjectFromPool("Fidd").GetComponent<Fidd>());
            }
            
            for (int i = 0; i < phase.numberOfPlantZombie; i++)
            {
                PlayingManager.Instance.currentEnemies.Add(Pools.Instance.GetObjectFromPool("PlantZombie").GetComponent<PlantZombie>());
            }
            
            for (int i = 0; i < phase.numberOfBird; i++)
            {
                PlayingManager.Instance.currentEnemies.Add((Pools.Instance.GetObjectFromPool("Bird").GetComponent<Bird>()));
            }
            
            for (int i = 0; i < phase.numberOfSnail; i++)
            {
                PlayingManager.Instance.currentEnemies.Add(Pools.Instance.GetObjectFromPool("Snail").GetComponent<Snail>());
            }
            
            for (int i = 0; i < phase.numberOfGroundSlime; i++)
            {
                PlayingManager.Instance.currentEnemies.Add(Pools.Instance.GetObjectFromPool("GroundSlime").GetComponent<GroundSlime>());
            }

            for (int i = 0; i < phase.numberOfFlySlime; i++)
            {
                PlayingManager.Instance.currentEnemies.Add(Pools.Instance.GetObjectFromPool("FlySlime").GetComponent<FlySlime>());
            }

            // boss
            for (int i = 0; i < phase.numberOfHuggyBoss; i++)
            {
                PlayingManager.Instance.currentEnemies.Add(Pools.Instance.GetObjectFromPool("HuggyBoss").GetComponent<HuggyBoss>());
            }
            
            for (int i = 0; i < phase.numberOfGreenBoss; i++)
            {
                PlayingManager.Instance.currentEnemies.Add(Pools.Instance.GetObjectFromPool("GreenBoss").GetComponent<GreenBoss>());
            }
            
            for (int i = 0; i < phase.numberOfBlueBoss; i++)
            {
                PlayingManager.Instance.currentEnemies.Add(Pools.Instance.GetObjectFromPool("BlueBoss").GetComponent<BlueBoss>());
            }
            
        }

        public int TotalNumber()
        {
            int total = 0;
            foreach (Phase phase in phaseList)
            {
                total +=
                (
                    phase.numberOfBird + phase.numberOfFidd + phase.numberOfSnail + phase.numberOfFlyFly +
                    phase.numberOfFlySlime + phase.numberOfPlantZombie +phase.numberOfGroundSlime +
                    phase.numberOfBlueBoss + phase.numberOfGreenBoss + phase.numberOfHuggyBoss
                );
            }

            return total;
        }

        private IEnumerator WaitToStartNewPhaseScreen()
        {
            yield return Yielders.Get(3.5f);
            GameManager.Instance.GamePlayController.uiConfetti.SetActive(true);
            yield return Yielders.Get(2.5f);
            PlayingManager.Instance.NewPhase();
        }

        private IEnumerator WaitToEndStage()
        {            
            PlayingManager.Instance.isPlaying = false;
            yield return Yielders.Get(5.5f);
            index = 0;
            PlayingManager.Instance.EndStage(LevelResult.Win);

        }
        
        // tutorial
        private IEnumerator DelayStartTutorial()
        {
            if (index == 0 && PlayerDataManager.Instance.GetStage() != 0)
            {
                GameManager.Instance.GamePlayController.uiTutorial.HandTutorial.gameObject.SetActive(true);
                GameManager.Instance.GamePlayController.uiTutorial.HandTutorialAction();
            }
            
            if (PlayerDataManager.Instance.GetStage() == 0 && index == 1)
                foreach (BaseEnemy be in PlayingManager.Instance.currentEnemies) 
                {
                    be.hp = 500;
                    be.healthBar.slider.maxValue = 500f;
                    be.healthBar.ChangeValue(500f);
                }

            yield return Yielders.Get(3.5f);
            TutorialAction();
        }

        private IEnumerator DelayStartSkillTutorial()
        {
            yield return Yielders.Get(3f);
            PlayerPrefs.SetInt("isTutorial",0);
            GameManager.Instance.Pause();
            GameManager.Instance.GamePlayController.uiTutorial.UsingSkillTutorial.SetActive(true);
        }
        private void TutorialAction()
        {
            if (PlayerDataManager.Instance.GetStage() == 0 && index == 1)
            {
                PlayingManager.Instance.isTutorial = false;
                GameManager.Instance.Pause();
                GameManager.Instance.GamePlayController.uiTutorial.ShootingTutorial.SetActive(true);
            }else if (PlayerDataManager.Instance.GetStage() == 0 && index == 2)
            {
                PlayingManager.Instance.isImmortal = true;
                StartCoroutine(DelayStartSkillTutorial());
            }
            else
            {
                PlayingManager.Instance.isImmortal = false;
            }
        }
       
    }
}
