using GeoGuessr.Game;
using UnityEngine;

namespace GeoGuessr.Presentation
{
    public class BoardPresenter : MonoBehaviour
    {
        [SerializeField] BoardTilePresenter _tilePresenterPrefab;
        public void Setup(Board board)
        {
            foreach (var tile in board.Definition.BoardTiles)
            {
                var tilePresenter = Instantiate(_tilePresenterPrefab, transform);
                tilePresenter.Setup(tile);
                tilePresenter.transform.localPosition = new Vector3(tile.X, 0, tile.Y);

            }
        }
    }
}