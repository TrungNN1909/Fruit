using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool instance;
        [SerializeField] public GameObject go;


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }



        public List<GameObject> CreatingObjects(GameObject prefab, int amount)
        {
            List<GameObject> list = new List<GameObject>();
            for (int i = 0; i < amount; i++)
            {
                GameObject obj = Instantiate(prefab, transform);
                obj.SetActive(false);
                list.Add(obj);
            }
            return list;
        }

        public GameObject GetPoolObject(List<GameObject> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (!list[i].activeInHierarchy) // ko hoat dong trong hierarchy
                {
                    return list[i];
                }
            }

            GameObject obj = Instantiate(go, transform);
            obj.SetActive(false);
            list.Add(obj);
            return obj;
        }
    }
}


