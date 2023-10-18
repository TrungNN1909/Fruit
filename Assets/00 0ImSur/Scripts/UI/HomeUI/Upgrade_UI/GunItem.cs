using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Unicorn
{
    public enum EGunType
    {
        MAIN_GUN = 0,
        SUB_GUN = 1
    }

    public class GunItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [HideInInspector] public Transform parentAfterDrag;
        [SerializeField] private Image img;

        [SerializeField] private Image priteHolder;
        [HideInInspector] public int level;

        [SerializeField] public GameObject sparkel;
        
        public Transform transformPool;
        public bool isMerged;

        public EGunType type;
        public GunInfos gunInfos; // will set when spawn
        public SubGunInfo subGunInfo;



        private void Start()
        {
            transformPool = GameObject.FindGameObjectWithTag("TabGun").transform;
        }

        // runs on updates
        public void OnBeginDrag(PointerEventData eventData)
        {
            parentAfterDrag = transform.parent;// the button that item stayed in before been dragged;
            transform.SetAsLastSibling(); // set as last gameobject of the parent.
            if(PlayerPrefs.GetInt("Tutorial",1) == 1) transform.SetParent(GameObject.FindGameObjectWithTag("TabGun").transform);
            img.raycastTarget = false;
            transform.DOScale(1.3f, 0.2f);
            GameManager.Instance.HomeController.OnHoldingGunItem?.Invoke();
            sparkel.SetActive(true);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
            transform.SetParent(GameObject.FindGameObjectWithTag("TabGun").transform);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("ON EndDrag");
            transform.SetParent(parentAfterDrag);
            img.raycastTarget = true;
            sparkel.SetActive(false);
            transform.DOScale(1f, 0.5f);
            GameManager.Instance.HomeController.OnDropingGunItem?.Invoke();
            if (isMerged)
            {
                gameObject.SetActive(false);
                isMerged = false;
            }

        }

        public void init()
        {
            switch (type)
            {
                case EGunType.MAIN_GUN:
                    priteHolder.sprite = gunInfos.sprite;
                    level = gunInfos.lv;
                    break;

                case EGunType.SUB_GUN:
                    priteHolder.sprite = subGunInfo.sprite;
                    level = subGunInfo.lv;
                    break;
            }

            gameObject.transform.localScale = Vector3.one;

            gameObject.transform.DOScale(1.2f, 0.25f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                gameObject.transform.DOScale(1f, 0.25f);
            });
        }

        public void Upgrade()
        {
            if(type == EGunType.MAIN_GUN)
            {
                int currentGunLevel = gunInfos.lv;
                gunInfos = PlayerDataManager.Instance.GetGun(++currentGunLevel);
            }
            else if (type == EGunType.SUB_GUN)
            {
                int currentSubGunLevel = subGunInfo.lv;
                subGunInfo = PlayerDataManager.Instance.GetSubGun(++currentSubGunLevel);
            }

            init();
        }

        public void Reset()
        {
            gameObject.SetActive(false);
            gameObject.transform.SetParent(GameManager.Instance.HomeController.transform);
        }

    }
}
  