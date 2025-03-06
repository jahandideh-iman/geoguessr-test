using GeoGuessr.Game;
using System.Collections.Generic;
using UnityEngine;

namespace GeoGuessr.Presentation
{
    public class LevelPresenter : MonoBehaviour
    {
        [SerializeField] BoardPresenter _boardPresenter;
        [SerializeField] PlayerPresenter _playerPresenterPrefab;
        [SerializeField] float _tileSize;

        public BoardPresenter BoardPresenter => _boardPresenter;
        private Dictionary<Player, PlayerPresenter> _playerPresenters = new();

        public void Setup(LevelController levelController)
        {
            _boardPresenter.Setup(levelController.Board, GetTileWorldPosition);

            foreach (var player in levelController.Players)
            {
                var playerPresenter = Instantiate(_playerPresenterPrefab, transform);
                playerPresenter.Setup(player, levelController.Board.GetPlayerTile(player), GetTileWorldPosition);
                _playerPresenters.Add(player, playerPresenter);
            }
        }

        public PlayerPresenter GetPlayerPresenter(Player player)
        {
            return _playerPresenters[player];
        }

        private Vector3 GetTileWorldPosition(BoardTile tile)
        {
            return new Vector3(tile.X * _tileSize, 0, tile.Y * _tileSize);
        }
    }
}