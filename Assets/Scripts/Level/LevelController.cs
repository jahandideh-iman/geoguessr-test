
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


namespace GeoGuessr.Game
{
    public interface ILevelViewPort
    {
        UniTask StartTurn(Player player);
        UniTask MovePlayer(Player player, IReadOnlyList<BoardTile> path);
        UniTask<Choice> ShowQuiz(Quiz quiz, DateTime endTime);
        UniTask ShowQuizResult(Quiz quiz, bool answerWasCorrect);
        void CloseQuiz();
    }

    public class QuizDatabase
    {
        private Dictionary<QuizType, List<Quiz>> quizesByType = new();

        public QuizDatabase()
        {
            foreach (var quizType in Enum.GetValues(typeof(QuizType)).Cast<QuizType>())
            {
                quizesByType[quizType] = new List<Quiz>();
            }

        }

        public void AddQuiz(Quiz quiz)
        {
            quizesByType[quiz.Type].Add(quiz);
        }

        internal IReadOnlyList<Quiz> QuizesOfType(QuizType quizType)
        {
            return quizesByType[quizType];
        }
    }

    public record Quiz(QuizType Type, string Question, string? CustomImageID, Choice[] Choices, Choice Answer)
    {
        public QuizType Type { get; } = Type;
        public string Question { get; } = Question;
        public string? CustomImageID { get; } = CustomImageID;
        public Choice[] Choices { get; } = Choices;
        public Choice Answer { get; } = Answer;
    }

    public record Choice(string? ImageID, string? Text)
    {
        public string? ImageID { get; } = ImageID;
        public string? Text { get; } = Text;
    }

    public enum QuizType
    {
        Question = 0,
        Flag = 1,
    }

    public class LevelController
    {
        private readonly static TimeSpan QuizDuration = TimeSpan.FromSeconds(10);

        enum LevelState
        {
            Idle,
            WaitingForRoll,
            Moving,
        }

        public Board Board { get; }
        public Player[] Players { get; }
        private QuizDatabase QuizDatabase { get; }
        private ILevelViewPort ViewPort { get; }

        private LevelState CurrentState { get; set; }

        public Player CurrentPlayer { get; set; }

        public LevelController(BoardDefinition boardDefinition, QuizDatabase quizDatabase, ILevelViewPort viewPort)
        {
            Players = new Player[] { new Player(0) };
            Board = new Board(boardDefinition, Players);
            QuizDatabase = quizDatabase;
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
            var lastTile = path[^1];
            await ViewPort.MovePlayer(CurrentPlayer, path);
            SetPlayerTile(CurrentPlayer, lastTile);

            if (lastTile.QuizType != null)
            {
                await PlayQuizGame(lastTile.QuizType.Value);
            }

            StartNextPlayerTurn();

        }

        private async Task PlayQuizGame(QuizType quizType)
        {
            var quizes = QuizDatabase.QuizesOfType(quizType);
            var quiz = quizes.RandomElement();
            var endTime = DateTime.UtcNow + QuizDuration;

            var cancelationSource = new CancellationTokenSource();
            var timeoutTask = UniTask
                .WaitUntil(() => DateTime.UtcNow >= endTime)
                .AttachExternalCancellation(cancelationSource.Token);
            var quizTask = ViewPort.ShowQuiz(quiz, endTime)
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
                    CurrentPlayer.AddScore(1);
                }

                await ViewPort.ShowQuizResult(quiz, answerWasCorrect);
            }
            else // Quiz time is over
            {
                ViewPort.CloseQuiz();
                await ViewPort.ShowQuizResult(quiz, false);
            }

        }

        private async void StartNextPlayerTurn()
        {
            CurrentState = LevelState.Idle;
            CurrentPlayer = Players[(CurrentPlayer.Index + 1) % Players.Length];
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