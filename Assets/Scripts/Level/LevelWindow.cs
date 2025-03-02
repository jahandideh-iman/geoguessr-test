using Arman.UIManagement;
using Cysharp.Threading.Tasks;
using GeoGuessr.Game;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GeoGuessr.Presentation
{
    public class LevelWindow : MainWindow
    {
        [SerializeField] Announcement _announcement;
        [SerializeField] RollPanel _rollPanel;

        internal void Setup(LevelController levelController)
        {
            _announcement.Setup();
            _rollPanel.Setup(onRoll: levelController.Roll);
        }

        public async UniTask SetupPlayerTurn(Player player)
        {
            _rollPanel.MoveIn();
            await _announcement.Announce($"Player {player.Index+1} turn");
        }

        internal async UniTask SetupPlayerMovement(Player player, IReadOnlyList<BoardTile> path)
        {
            _rollPanel.MoveOut();
            await _announcement.Announce($"Player {player.Index+1} roll {path.Count}");
        }
    }
}