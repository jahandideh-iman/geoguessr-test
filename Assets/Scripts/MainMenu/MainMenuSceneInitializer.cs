using Arman.Foundation.Core.ServiceLocating;
using Arman.SceneMangement;
using Arman.UIManagement;
using GeoGuessr.Game;
using GeoGuessr.Presentation;
using UnityEngine;

namespace GeoGuessr.Main
{
    public class MainMenuSceneInitializer : SceneInitilizer
    {
        [SerializeField] MainMenuWindow _mainMenuWindow;
        [SerializeField] Camera _uiCamera;

        protected override void Init()
        {
            var uiManager = ServiceLocator.Find<UIManager>();
            uiManager.SetMainCamera(_uiCamera);
            uiManager.SetMainWindow(_mainMenuWindow);

            _mainMenuWindow.Setup(
                ServiceLocator.Find<BoardDatabase>(),
                ServiceLocator.Find<GameTransitionManager>());
        }
    }
}
