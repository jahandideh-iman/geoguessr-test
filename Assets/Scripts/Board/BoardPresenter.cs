using GeoGuessr.Game;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GeoGuessr.Presentation
{
    public class BoardPresenter : MonoBehaviour
    {
        [SerializeField] BoardTilePresenter _tilePresenterPrefab;

        List<BoardTilePresenter> _tilePresenters = new();

        public void Setup(Board board)
        {
            foreach (var tile in board.Definition.BoardTiles)
            {
                var tilePresenter = Instantiate(_tilePresenterPrefab, transform);
                tilePresenter.Setup(tile);
                tilePresenter.transform.localPosition = new Vector3(tile.X, 0, tile.Y);
                _tilePresenters.Add(tilePresenter);
            }
        }

        internal BoardTilePresenter GetTilePresenter(BoardTile tile)
        {
            return _tilePresenters[tile.Index];
        }
    }
}