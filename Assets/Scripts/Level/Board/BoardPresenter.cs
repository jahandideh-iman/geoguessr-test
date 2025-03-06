using GeoGuessr.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GeoGuessr.Utilities.SerializableDictionaryUtilities;

namespace GeoGuessr.Presentation
{
    public class BoardPresenter : MonoBehaviour
    {
        [SerializeField]
        BoardTilePresenter _emptyTilePresenterPrefab;

        [SerializeField]
        List<SerializableDictionaryEntry<QuizType, BoardTilePresenter>> _quizTilePresenterPrefabs;

        List<BoardTilePresenter> _tilePresenters = new();
        Func<BoardTile, Vector3> _tileWorldPositionGetter;

        public void Setup(Board board, Func<BoardTile, Vector3> tileWorldPositionGetter)
        {
            _tileWorldPositionGetter = tileWorldPositionGetter;
            var tilePresenterPrefabByQuizType = _quizTilePresenterPrefabs.ToDictionary();

            foreach (var tile in board.Definition.BoardTiles)
            {
                var tilePresenter = Instantiate(GetTilePresenterPrefab(tile), transform);
                tilePresenter.Setup(tile);
                tilePresenter.transform.position = _tileWorldPositionGetter(tile);
                _tilePresenters.Add(tilePresenter);
            }

            BoardTilePresenter GetTilePresenterPrefab(BoardTile tile)
            {
                if (tile.QuizType == null)
                {
                    return _emptyTilePresenterPrefab;
                }

                return tilePresenterPrefabByQuizType[tile.QuizType.Value];
            }
        }

        public BoardTilePresenter GetTilePresenter(BoardTile tile)
        {
            return _tilePresenters[tile.Index];
        }
    }
}