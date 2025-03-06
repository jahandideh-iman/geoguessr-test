#nullable enable
using Arman.Foundation.Core.ServiceLocating;

namespace GeoGuessr.Game
{
    public class GameTransitionManager : Service
    {
        private const string MainMenuScene = "MainMenu";
        private const string LevelScene = "Level";

        public LevelMode? RequestionLevelMode { get; private set; }

        public void GoToMainMenu()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(MainMenuScene);
        }

        public void GoToLevel(LevelMode levelMode)
        {
            RequestionLevelMode = levelMode;
            UnityEngine.SceneManagement.SceneManager.LoadScene(LevelScene);
        }
    }
}