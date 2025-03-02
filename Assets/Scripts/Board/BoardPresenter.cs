using GeoGuessr.Game;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GeoGuessr.Presentation.SerializableDictionaryUtilities;

namespace GeoGuessr.Presentation
{

    public class BoardPresenter : MonoBehaviour
    {
        [SerializeField]
        BoardTilePresenter _emptyTilePresenterPrefab;

        [SerializeField] 
        List<SerializableDictionaryKeyValue<QuizType, BoardTilePresenter>> _quizTilePresenterPrefabs;

        List<BoardTilePresenter> _tilePresenters = new();

        public void Setup(Board board)
        {
            var tilePresenterPrefabByQuizType = _quizTilePresenterPrefabs.ToDictionary();

            foreach (var tile in board.Definition.BoardTiles)
            {
                var tilePresenter = Instantiate(GetTilePresenterPrefab(tile), transform);
                tilePresenter.Setup(tile);
                tilePresenter.transform.localPosition = new Vector3(tile.X, 0, tile.Y);
                _tilePresenters.Add(tilePresenter);
            }

            BoardTilePresenter GetTilePresenterPrefab(BoardTile tile)
            {
                if(tile.QuizType == null)
                {
                    return _emptyTilePresenterPrefab;
                }

                return tilePresenterPrefabByQuizType[tile.QuizType.Value];

            }
        }

        internal BoardTilePresenter GetTilePresenter(BoardTile tile)
        {
            return _tilePresenters[tile.Index];
        }
    }
}