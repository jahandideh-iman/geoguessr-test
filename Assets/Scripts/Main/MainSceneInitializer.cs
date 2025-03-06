using Arman.Foundation.Core.ServiceLocating;
using Arman.SceneMangement;
using Arman.UIManagement;
using GeoGuessr.Configuration;
using GeoGuessr.Game;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GeoGuessr.Main
{

    public class MainSceneInitializer : SceneInitilizer
    {
        [SerializeField] List<TextAsset> _levelsAssets;
        [SerializeField] List<TextAsset> _quizAssets;
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private List<GameObject> _objectsToKeepAlive;

        protected override void Init()
        {
            ServiceLocator.Init();

            _uiManager.Init();
            ServiceLocator.Register(_uiManager);

            var gameTransitionManager = new GameTransitionManager();
            ServiceLocator.Register(gameTransitionManager);

            var quizDatabase = new QuizDatabase(_quizAssets.Select(asset => QuizJsonConfiguration.Load(asset.text)));
            ServiceLocator.Register(quizDatabase);

            var boardsDatabase = new BoardsDatabase(_levelsAssets.Select(asset => BoardJsonConfiguration.Load(asset.text)));
            ServiceLocator.Register(boardsDatabase);

            foreach (GameObject obj in _objectsToKeepAlive)
            {
                DontDestroyOnLoad(obj);
            }

            gameTransitionManager.GoToMainMenu();
        }
    }
}
