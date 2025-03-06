using Arman.Foundation.Core.ServiceLocating;
using Arman.SceneMangement;
using Arman.UIManagement;
using GeoGuessr.Configuration;
using GeoGuessr.Game;
using GeoGuessr.Presentation;
using UnityEngine;

namespace GeoGuessr.Main
{
    public class LevelSceneInitializer : SceneInitilizer
    {

        [SerializeField] LevelPresenter _levelPresenter;
        [SerializeField] LevelWindow _levelWindow;
        [SerializeField] Camera _uiCamera;
        [SerializeField] FollowCamera _followCamera;

        private LevelController _levelController;
        protected override void Init()
        {
            var uiManager = ServiceLocator.Find<UIManager>();
            uiManager.SetMainCamera(_uiCamera);
            uiManager.SetMainWindow(_levelWindow);


            var viewAdapter = new LevelViewAdapter(_levelWindow, _levelPresenter, _followCamera);

            var quizDatabase = ServiceLocator.Find<QuizDatabase>();
            var gameTransitionManager = ServiceLocator.Find<GameTransitionManager>();

            _levelController = gameTransitionManager.RequestionLevelMode.BuildLevelController(
                quizDatabase, 
                viewAdapter,
                viewAdapter);

            _levelPresenter.Setup(_levelController);
            _levelWindow.Setup(gameTransitionManager);

            _levelController.Start();

        }
    }
}