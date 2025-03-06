using Cysharp.Threading.Tasks;
using DG.Tweening;
using GeoGuessr.Game;
using UnityEngine;

namespace GeoGuessr.Presentation
{
    public class PlayerPresenter : MonoBehaviour
    {
        public void Setup(Player player, BoardTile tile)
        {
            transform.localPosition = PositionFromTile(tile);
        }

        public UniTask MoveTo(BoardTilePresenter tilePresenter)
        {
            return transform
                .DOLocalMove(PositionFromTile(tilePresenter.Tile), 0.5f)
                .AsyncWaitForCompletion()
                .AsUniTask();
        }

        Vector3 PositionFromTile(BoardTile tile)
        {
            return new Vector3(tile.X, 0, tile.Y);
        }
    }
}