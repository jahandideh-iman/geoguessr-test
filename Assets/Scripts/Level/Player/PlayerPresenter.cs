using Cysharp.Threading.Tasks;
using DG.Tweening;
using GeoGuessr.Game;
using System;
using UnityEngine;

namespace GeoGuessr.Presentation
{
    public class PlayerPresenter : MonoBehaviour
    {
        Func<BoardTile, Vector3> _tileWorldPositionGetter;
        public void Setup(Player player, BoardTile tile, Func<BoardTile,Vector3> tileWorldPositionGetter)
        {
            _tileWorldPositionGetter = tileWorldPositionGetter;
            transform.position = _tileWorldPositionGetter(tile);
        }

        public UniTask MoveTo(BoardTilePresenter tilePresenter)
        {
            return transform
                .DOMove(_tileWorldPositionGetter(tilePresenter.Tile), 0.5f)
                .AsyncWaitForCompletion()
                .AsUniTask();
        }

    }
}