using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Core
{
    public static class SceneLoader
    {
        private const string MainMenuScene = "MainMenu";
        private const string GameScene = "Game";

        public static void LoadMainMenuScene()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(MainMenuScene);
        }

        public static void LoadGame()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(GameScene);
        }
    }
}
