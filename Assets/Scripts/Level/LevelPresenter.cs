using GeoGuessr.Game;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GeoGuessr.Presentation
{
    public class LevelPresenter : MonoBehaviour
    {
        [SerializeField] BoardPresenter _boardPresenter;
        [SerializeField] PlayerPresenter _playerPresenterPrefab;

        public BoardPresenter BoardPresenter => _boardPresenter;


        private Dictionary<Player, PlayerPresenter> _playerPresenters = new();   

        public void Setup(LevelController levelController)
        {
            _boardPresenter.Setup(levelController.Board);

            foreach (var player in levelController.Players)
            {
                var playerPresenter = Instantiate(_playerPresenterPrefab, transform);
                playerPresenter.Setup(player, levelController.Board.GetPlayerTile(player));
                _playerPresenters.Add(player, playerPresenter);
            }

        }

        internal PlayerPresenter GetPlayerPresenter(Player player)
        {
            return _playerPresenters[player];
        }
    }
}