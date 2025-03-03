﻿using Arman.Foundation.Core.ServiceLocating;
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
        [SerializeField] TextAsset _boardConfig;
        [SerializeField] TextAsset _flagQuizesConfig;
        [SerializeField] TextAsset _questionQuizesConfig;
        [SerializeField] LevelPresenter _levelPresenter;
        [SerializeField] LevelWindow _levelWindow;
        [SerializeField] Camera _camera;
        [SerializeField] FollowCamera _followCamera;

        private LevelController _levelController;
        protected override void Init()
        {
            var uiManager = ServiceLocator.Find<UIManager>();
            uiManager.SetMainCamera(_camera);
            uiManager.SetMainWindow(_levelWindow);

            var boardDefinition = BoardJsonConfiguration.Load(_boardConfig.text);
            var quizDatabase = QuizDatabaseJsonConfiguration.Load(_flagQuizesConfig.text, _questionQuizesConfig.text);

            var viewAdapter = new LevelViewAdapter(_levelWindow, _levelPresenter, _followCamera);

            var levelMode = new LevelMode()
                .SetBoard(boardDefinition)
                .AddLocalPlayer("Player1")
                .AddLocalPlayer("Player2")
                .AddAiPlayer();

            _levelController = levelMode.BuildLevelController(quizDatabase, viewAdapter, viewAdapter, viewAdapter);

            _levelPresenter.Setup(_levelController);
            _levelWindow.Setup(_levelController);

            _levelController.Start();

        }
    }
}