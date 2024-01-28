using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        public void OnStartGame()
        {
            print("Start Game");
            SceneManager.LoadScene(2);
        }

        public void OnQuitGame()
        {
            print("Quit Game");
            Application.Quit();
        }
    }
}