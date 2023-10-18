using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Unicorn
{

    public class EnemySpawner : MonoBehaviour
    {
        private Vector2 defaultPos;
        private void Awake()
        {
            defaultPos = new Vector2(25, -5);
        }

        public GameObject Spawn(GameObject enemyPrefab)
        {
            return Instantiate(enemyPrefab, new Vector3(defaultPos.x, defaultPos.y, 0), Quaternion.identity);
        }
    }

}