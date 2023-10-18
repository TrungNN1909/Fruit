using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
namespace Unicorn
{
    public class Pools : MonoBehaviour
    {
        public static Pools Instance;
        public List<GameObject> listObjectToPool = new List<GameObject>();      // list prefab for pooling (number of pools)
        public int size;        // size of pool
        Dictionary<string, List<GameObject>> poolDict = new Dictionary<string, List<GameObject>>();

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
        }

        public void Init()
        {
            for (int j = 0; j < listObjectToPool.Count; j++)
            {
                string key = listObjectToPool[j].name;

                for (int i = 0; i < size; i++)
                {
                    GameObject obj = Instantiate(listObjectToPool[j], transform );
                    obj.SetActive(false);


                    if (!poolDict.ContainsKey(key))
                    {
                        poolDict.Add(key, new List<GameObject>());
                    }
                    poolDict[key].Add(obj);
                }
            }
        }
        // Clone game objects.
        // private void Awake()
        // {
        //     if(Instance == null)
        //     {
        //         Instance = this;
        //     }
        //
        //     for (int j = 0; j < listObjectToPool.Count; j++)
        //     {
        //         string key = listObjectToPool[j].name;
        //
        //         for (int i = 0; i < size; i++)
        //         {
        //             GameObject obj = Instantiate(listObjectToPool[j], transform );
        //             obj.SetActive(false);
        //
        //
        //             if (!poolDict.ContainsKey(key))
        //             {
        //                 poolDict.Add(key, new List<GameObject>());
        //             }
        //
        //             poolDict[key].Add(obj);
        //         }
        //     }
        // }
        public GameObject GetObjectFromPool(string key) // 
        {
            if (poolDict.ContainsKey(key))
            {
                for (int i = 0; i < poolDict[key].Count; i++)
                {
                    int temp = i;
                    if (!poolDict[key][temp].activeInHierarchy)
                    {
                        poolDict[key][temp].SetActive(true);
                        return poolDict[key][temp];
                    }
                }
                int _index = -1;
                //Spawn new object for adding to pool
                for (int i = 0; i < listObjectToPool.Count; i++)
                {
                    if (listObjectToPool[i].name.Equals(key))
                    {
                        _index = i;
                        break;
                    }
                }

                
                GameObject newObj = Instantiate(listObjectToPool[_index], transform);
                newObj.SetActive(true);
                poolDict[key].Add(newObj);
                return newObj;
            }
            return null;
        }


    }
}
