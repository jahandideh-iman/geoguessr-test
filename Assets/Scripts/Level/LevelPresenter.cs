using GeoGuessr.Game;
using UnityEngine;

namespace GeoGuessr.Presentation
{
    public class LevelPresenter : MonoBehaviour 
    {
        [SerializeField]
        BoardPresenter _boardPresenter;
        public void Setup(LevelController levelController)
        {

            _boardPresenter.Setup(levelController.Board);
        }
    }
}