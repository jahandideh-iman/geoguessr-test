using Cysharp.Threading.Tasks;
using GeoGuessr.Game;
using GeoGuessr.Presentation;
using System;
using System.Collections.Generic;

namespace GeoGuessr.Main
{
    public class LevelViewAdapter : ILevelViewPort
    {
        private readonly LevelWindow _levelWindow;
        private readonly LevelPresenter _levelPresenter;

        public LevelViewAdapter(LevelWindow levelWindow, LevelPresenter levelPresenter)
        {
            _levelWindow = levelWindow;
            _levelPresenter = levelPresenter;
        }

        public async UniTask MovePlayer(Player player, IReadOnlyList<BoardTile> path)
        {
            await _levelWindow.SetupPlayerMovement(player, path);
            var playerPresenter = _levelPresenter.GetPlayerPresenter(player);

            foreach (var tile in path)
            {
                var tilePresenter = _levelPresenter.BoardPresenter.GetTilePresenter(tile);
                await playerPresenter.MoveTo(tilePresenter);
            }
        }

        public UniTask StartTurn(Player player)
        {
            return _levelWindow.SetupPlayerTurn(player);
        }
    }
}