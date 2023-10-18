using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using DG.Tweening;

namespace Unicorn
{

    public enum ETypeOfTab
    {
        Main,
        Support
    }
    public class ArchivementScreen : MonoBehaviour
    {
        public ETypeOfTab type;
        private List<GameObject> spawnedCardList;

        [SerializeField] private GameObject mainGunCard;
        [SerializeField] private GameObject supportGunCard;
        
        [SerializeField] private Button mainGunBackButton;
        [SerializeField] private Button backgroundButton;
        [SerializeField] private Button mainGunButtonOff;
        [SerializeField] private Button mainGunButtonOn;
        [SerializeField] private Button supportGunButtonOff;
        [SerializeField] private Button supportGunButtonOn;
        
        [SerializeField] private Transform cardSpawnPos;

        [SerializeField] private GameObject tab;

        [SerializeField] private ScrollRect _scrollRect;
        //get infomations from scriptable objects data
        private List<ArchiveCard> mainGunCards;
        private List<ArchiveCard> supportGunCards;

        private bool isSpawned = true;
        private bool isClosing;
        private void Start()
        {
            type = ETypeOfTab.Main;
            
            mainGunButtonOff.onClick.AddListener(MainGunButtonOnClick);
            supportGunButtonOff.onClick.AddListener(SupportGunButtonOnClick);
            
            backgroundButton.onClick.AddListener(BackgroundButtonOnClick);
            mainGunBackButton.onClick.AddListener(BackgroundButtonOnClick);
            
            
            PlayerPrefs.SetInt("MainGunNumber", 0);
            PlayerPrefs.SetInt("SupportGun", 0);

        }

        private void SupportGunButtonOnClick()
        {
            if(type == ETypeOfTab.Support) return;

            
            supportGunButtonOn.gameObject.SetActive(true);
            supportGunButtonOff.gameObject.SetActive(false);
            mainGunButtonOff.gameObject.SetActive(true);
            mainGunButtonOn.gameObject.SetActive(false);
            
            type = ETypeOfTab.Support;
            
            ClearTab();
            SpawnCard();
            
            supportGunButtonOn.gameObject.transform.DOLocalMoveX(25f, 0.3f).SetEase(Ease.OutBounce).OnStart(ResetPosition);
            mainGunButtonOff.gameObject.transform.DOLocalMoveX(-25f, 0.3f).SetEase(Ease.OutBounce).OnStart(ResetPosition);;
        }
        private void MainGunButtonOnClick()
        {
            if(type == ETypeOfTab.Main) return;
            
            supportGunButtonOn.gameObject.SetActive(false);
            supportGunButtonOff.gameObject.SetActive(true);
            mainGunButtonOff.gameObject.SetActive(false);
            mainGunButtonOn.gameObject.SetActive(true);
            
            type = ETypeOfTab.Main;
            
            ClearTab();
            SpawnCard();
            
            
            supportGunButtonOff.gameObject.transform.DOLocalMoveX(-25f, 0.3f).SetEase(Ease.OutBounce).OnStart(ResetPosition);
            mainGunButtonOn.gameObject.transform.DOLocalMoveX(25f,0.3f).SetEase(Ease.OutBounce).OnStart(ResetPosition);
        }

        private void BackgroundButtonOnClick()
        {
            if(isClosing) return;
            isClosing = true;
            
            tab.transform.DOScale(0.3f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                tab.gameObject.transform.localScale = Vector3.one;
                gameObject.SetActive(false);
            });
        }

        public void Init()
        {
            mainGunCards = PlayerDataManager.Instance.archivementListAsset.mainGunsData;
            supportGunCards = PlayerDataManager.Instance.archivementListAsset.supportGunData;
        }
            
        
        private void OnEnable()
        {
            isClosing = false;
            Init();
            GameManager.Instance.HomeController.uiHome.HomeUIDisPlay(false);
            spawnedCardList = new List<GameObject>();
            
            
            supportGunButtonOn.gameObject.SetActive(false);
            supportGunButtonOff.gameObject.SetActive(true);
            mainGunButtonOff.gameObject.SetActive(false);
            mainGunButtonOn.gameObject.SetActive(true);

            type = ETypeOfTab.Main;
            StartCoroutine(DelayForScrollRect());
            SpawnCard();
        }

        private IEnumerator DelayForScrollRect()
        {            
            _scrollRect.horizontal = false;
            yield return Yielders.Get(0.6f);
            _scrollRect.horizontal = true;
        }
        private void SpawnCard()
        {
            switch (type)
            {
                case ETypeOfTab.Main:
                {
                    foreach(ArchiveCard ac in mainGunCards)
                    {
                        GameObject go = SimplePool.Spawn(mainGunCard);
                        go.transform.SetParent(cardSpawnPos);
                        go.transform.localScale = Vector3.one;
                        
                        go.GetComponent<MainGunButton>().mainImg.sprite = ac.Sprite;
                        go.GetComponent<MainGunButton>().mainText.text = ac.text;
                        go.GetComponent<MainGunButton>().cardNumber = ac.cardNumber;
                        go.GetComponent<MainGunButton>().coin = ac.coin;
                        spawnedCardList.Add(go);
                
                    }
                    break;
                }
                
                case ETypeOfTab.Support:
                {
                    foreach (ArchiveCard ac in supportGunCards)
                    {
                        GameObject go = SimplePool.Spawn(supportGunCard,cardSpawnPos.position,quaternion.identity);
                        go.transform.SetParent(cardSpawnPos);
                        go.transform.localScale = Vector3.one;

                        go.GetComponent<SupportGunButton>().mainImg.sprite = ac.Sprite;
                        go.GetComponent<SupportGunButton>().mainText.text = ac.text;
                        go.GetComponent<SupportGunButton>().coin = ac.coin;
                        go.GetComponent<SupportGunButton>().cardNumber = ac.cardNumber;
                        spawnedCardList.Add(go);
                    }
                    break;
                }
            }
        }

        private void OnDisable()
        {
            ClearTab();
            GameManager.Instance.HomeController.uiHome.HomeUIDisPlay(true);
        }

        private void ClearTab()
        {
            foreach (var VARIABLE in spawnedCardList)
            {
                SimplePool.Despawn(VARIABLE);
            }
            
            spawnedCardList.Clear();
        }

        private void ResetPosition()
        {
            supportGunButtonOn.gameObject.transform.localPosition = new Vector3(0, -100, 0);
            mainGunButtonOff.gameObject.transform.localPosition = new Vector3(0, 100, 0);
            supportGunButtonOff.gameObject.transform.localPosition = new Vector3(0, -100, 0);
            mainGunButtonOn.gameObject.transform.localPosition = new Vector3(0, 100, 0);
        }

        public bool Check()
        {
            foreach (ArchiveCard ac in mainGunCards)
            {
                if (!PlayerDataManager.Instance.GetMainGunCardTaken(ac.cardNumber) && ac.cardNumber <= PlayerDataManager.Instance.GetGunHighestLevel())
                {
                    return true;
                }
            }

            foreach (ArchiveCard ac in supportGunCards)
            {
                if (!PlayerDataManager.Instance.GetSuPGunCardTaken(ac.cardNumber) && ac.cardNumber <= PlayerDataManager.Instance.GetSubGunHighestLevel())
                {
                    return true;
                }
            }
            
            return false;
        }

    }
}
