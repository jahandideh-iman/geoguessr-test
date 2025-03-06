using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GeoGuessr.Utilities;

namespace GeoGuessr.Game
{
    public interface ILevelViewPort
    {
        UniTask StartTurn(Player player);
        IEnumerable<UniTask<BoardTile>> MovePlayer(Player player, IReadOnlyList<BoardTile> path);
        void UpdatePlayerScore(int score, int change);
        void CloseQuiz();
    }

    public class LevelController
    {
        private readonly int EmptyTileScore = 500;
        private readonly int QuizScore = 5000;
        private readonly static TimeSpan QuizDuration = TimeSpan.FromSeconds(10);

        public Board Board { get; }
        public Player[] Players { get; }
        public Player CurrentPlayer => Players[_currentPlayerIndex];
        private QuizDatabase QuizDatabase { get; }
        private ILevelViewPort ViewPort { get; }
        private int _currentPlayerIndex = 0;

        public LevelController(
            IEnumerable<Player> players,
            BoardDefinition boardDefinition,
            QuizDatabase quizDatabase,
            ILevelViewPort viewPort)
        {
            Players = players.ToArray();
            Board = new Board(boardDefinition, Players);
            QuizDatabase = quizDatabase;
            ViewPort = viewPort;
        }

        public void Start()
        {
            ExecuteCurrentPlayerTurn();
        }

        public void GoToNextPlayerTurn()
        {
            _currentPlayerIndex = (_currentPlayerIndex + 1) % Players.Length;
            ExecuteCurrentPlayerTurn();
        }

        private async void ExecuteCurrentPlayerTurn()
        {
            await ViewPort.StartTurn(CurrentPlayer);
            var rollNumber = await CurrentPlayer.Controller.GetRoll();

            await MovePlayer(rollNumber);

            var playerTile = Board.GetPlayerTile(CurrentPlayer);
            if (playerTile.QuizType != null)
            {
                await PlayQuizGame(playerTile.QuizType.Value);
            }

            GoToNextPlayerTurn();
        }

        public async UniTask MovePlayer(int roll)
        {
            var path = Board.DeterminRollPath(roll, CurrentPlayer);
            var lastTile = path[^1];
            foreach (var tileMovement in ViewPort.MovePlayer(CurrentPlayer, path))
            {
                var boardTile = await tileMovement;
                if (boardTile.QuizType == null)
                {
                    AddPlayerScore(EmptyTileScore);
                }
            }
            Board.SetPlayerTile(CurrentPlayer, lastTile);

        }

        private async UniTask PlayQuizGame(QuizType quizType)
        {
            var quizes = QuizDatabase.QuizesOfType(quizType);
            var quiz = quizes.RandomElement();
            var endTime = DateTime.UtcNow + QuizDuration;

            var cancelationSource = new CancellationTokenSource();
            var timeoutTask = UniTask
                .WaitUntil(() => DateTime.UtcNow >= endTime)
                .AttachExternalCancellation(cancelationSource.Token);
            var quizTask = CurrentPlayer.Controller.GetQuizAnswer(quiz, endTime)
                .AttachExternalCancellation(cancelationSource.Token)
                .Preserve();

            await UniTask.WhenAny(timeoutTask, quizTask);

            cancelationSource.Cancel();

            if (quizTask.Status == UniTaskStatus.Succeeded)
            {
                var choice = await quizTask;
                var answerWasCorrect = choice == quiz.Answer;

                if (answerWasCorrect)
                {
                    AddPlayerScore(QuizScore);
                }

                await CurrentPlayer.Controller.ShowQuizResult(quiz, answerWasCorrect);
            }
            else // Quiz time is over
            {
                ViewPort.CloseQuiz();
                await CurrentPlayer.Controller.ShowQuizResult(quiz, false);
            }
        }

        private void AddPlayerScore(int change)
        {
            CurrentPlayer.AddScore(change);
            ViewPort.UpdatePlayerScore(CurrentPlayer.Score, change);
        }
    }
}