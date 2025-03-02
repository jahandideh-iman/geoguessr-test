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
        private readonly FollowCamera _followCamera;

        public LevelViewAdapter(LevelWindow levelWindow, LevelPresenter levelPresenter, FollowCamera followCamera)
        {
            _levelWindow = levelWindow;
            _levelPresenter = levelPresenter;
            _followCamera = followCamera;
        }

        public async UniTask MovePlayer(Player player, IReadOnlyList<BoardTile> path)
        {
            var playerPresenter = _levelPresenter.GetPlayerPresenter(player);

            _followCamera.SetTarget(playerPresenter.transform);
            await _levelWindow.SetupPlayerMovement(player, path);

            foreach (var tile in path)
            {
                var tilePresenter = _levelPresenter.BoardPresenter.GetTilePresenter(tile);
                await playerPresenter.MoveTo(tilePresenter);
            }

            _followCamera.ClearTarget();
        }

        public UniTask StartTurn(Player player)
        {
            return _levelWindow.SetupPlayerTurn(player);
        }
    }
}