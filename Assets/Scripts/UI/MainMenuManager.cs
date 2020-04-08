using UnityEngine;
using UnityEngine.SceneManagement;

namespace StrategyGame.Assets.Scripts.UI
{
    public class MainMenuManager : MonoBehaviour
    {
        public void LoadGame()
        {
            SceneManager.LoadScene(1);
        }
    }
}