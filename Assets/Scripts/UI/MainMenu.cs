using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        public void OnStartGame()
        {
            print("Start Game");
            SceneManager.LoadScene(1);
        }

        public void OnQuitGame()
        {
            print("Quit Game");
            Application.Quit();
        }

        public void OnBackMainMenu()
        {
            print("Back Menu");
            SceneManager.LoadScene(0);
        }
    }
}