using Arman.SceneMangement;
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

        private LevelController _levelController;
        protected override void Init()
        {
            var boardDefinition = JsonConvert
                .DeserializeObject<BoardJsonConfiguration>(_boardConfig.text)
                .ToBoardDefinition();
            _levelController = new LevelController(boardDefinition);

            _levelPresenter.Setup(_levelController);

        }
    }
}