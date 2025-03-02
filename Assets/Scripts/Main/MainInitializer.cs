using Arman.Foundation.Core.ServiceLocating;
using Arman.SceneMangement;

namespace GeoGuessr.Main
{
    public class MainInitializer : SceneInitilizer
    {
        protected override void Init()
        {
            ServiceLocator.Init();
        }
    }
}