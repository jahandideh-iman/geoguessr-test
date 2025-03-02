
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GeoGuessr.Game
{
    public interface ILevelViewPort
    {
        UniTask StartTurn(Player player);
        UniTask MovePlayer(Player player, IReadOnlyList<BoardTile> path);
    }

    public class LevelController
    {
        enum LevelState
        {
            Idle,
            WaitingForRoll,
            Moving,
        }

        public Board Board { get; }
        public Player[] Players { get; }
        private ILevelViewPort ViewPort { get; }

        private LevelState CurrentState { get; set; }

        public Player CurrentPlayer { get; set; }

        public LevelController(BoardDefinition boardDefinition, ILevelViewPort viewPort)
        {
            Players = new Player[] { new Player(0) };
            Board = new Board(boardDefinition, Players);

            ViewPort = viewPort;

            CurrentPlayer = Players[^1];
            CurrentState = LevelState.Idle;
        }

        public void Start()
        {
            StartNextPlayerTurn();
        }

        public async void Roll()
        {
            if (CurrentState != LevelState.WaitingForRoll)
            {
                Debug.LogError("The level state is not accepting rolls");
                return;
            }

            CurrentState = LevelState.Moving;

            var steps = UnityEngine.Random.Range(1, 7);
            var path = Board.DeterminRollPath(steps, CurrentPlayer);
            await ViewPort.MovePlayer(CurrentPlayer, path);
            SetPlayerTile(CurrentPlayer, path[^1]);

            StartNextPlayerTurn();

        }

        private async void StartNextPlayerTurn()
        {
            CurrentState = LevelState.Idle;
            CurrentPlayer = Players[(CurrentPlayer.Index + 1 )% Players.Length];
            await ViewPort.StartTurn(CurrentPlayer);
            GoToWaitingForRollState();

        }

        private void SetPlayerTile(Player player, BoardTile tile)
        {
            Board.SetPlayerTile(player, tile);
        }

        private void GoToWaitingForRollState()
        {
            CurrentState = LevelState.WaitingForRoll;
        }

    }
}