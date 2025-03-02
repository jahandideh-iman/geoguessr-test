using Arman.Foundation.Core.PersistentDataManagement;
using Arman.Foundation.Core.ServiceLocating;
using Arman.Foundation.Unity.PersistentDataManagement;
using Arman.SceneMangement;
using Arman.UIManagement;
using System.Collections.Generic;
using UnityEngine;

namespace BalloonPop
{

    public class MainSceneInitializer : SceneInitilizer
    {
        [SerializeField]
        private string startingSceneName;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private List<GameObject> objectsToKeepAlive;

        protected override void Init()
        {
            ServiceLocator.Init();
            ServiceLocator.Register(new SceneManager());
            ServiceLocator.Register(uiManager);

            foreach (GameObject obj in objectsToKeepAlive)
            {
                DontDestroyOnLoad(obj);
            }

            ServiceLocator.Find<SceneManager>().Open(startingSceneName);
        }
    }
}
