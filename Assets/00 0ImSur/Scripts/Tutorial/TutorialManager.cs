using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class TutorialManager : MonoBehaviour
    {
        // Update is called once per frame
        private void Update()
        {
            if (GameManager.Instance.GameStateController.CurrentGameState == GameState.IN_GAME)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
