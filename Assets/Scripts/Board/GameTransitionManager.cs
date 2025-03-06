using Arman.Foundation.Core.ServiceLocating;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GeoGuessr.Game
{
    public class BoardsDatabase : Service
    {
        public BoardDefinition[] BoardDefinitions { get; }
        public BoardsDatabase(IEnumerable<BoardDefinition> boardDefinition) {

            BoardDefinitions = boardDefinition.ToArray();
        }


    }
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