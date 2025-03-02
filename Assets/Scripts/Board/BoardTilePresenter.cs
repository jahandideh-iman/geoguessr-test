using GeoGuessr.Game;
using UnityEngine;

namespace GeoGuessr.Presentation
{
    public class BoardTilePresenter : MonoBehaviour
    {
        public BoardTile Tile { get; private set; }
        public void Setup(BoardTile tile)
        {
            Tile = tile;
        }
    }
}