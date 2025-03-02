using Arman.Foundation.Core.ServiceLocating;
using Arman.SceneMangement;
using Arman.UIManagement;
using GeoGuessr.Configuration;
using GeoGuessr.Game;
using GeoGuessr.Presentation;
using Newtonsoft.Json;
using UnityEngine;

namespace GeoGuessr.Main
{
    public class LevelSceneInitializer : SceneInitilizer
    {
        [SerializeField] TextAsset _boardConfig;
        [SerializeField] LevelPresenter _levelPresenter;
        [SerializeField] LevelWindow _levelWindow;
        [SerializeField] Camera _camera;

        private LevelController _levelController;
        protected override void Init()
        {
            var uiManager = ServiceLocator.Find<UIManager>();
            uiManager.SetMainCamera(_camera);
            uiManager.SetMainWindow(_levelWindow);

            var boardDefinition = JsonConvert
                .DeserializeObject<BoardJsonConfiguration>(_boardConfig.text)
                .ToBoardDefinition();

            var viewPort = new LevelViewAdapter(_levelWindow, _levelPresenter);

            _levelController = new LevelController(boardDefinition, viewPort);

            _levelPresenter.Setup(_levelController);
            _levelWindow.Setup(_levelController);

            _levelController.Start();

        }
    }
}