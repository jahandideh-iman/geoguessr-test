
using Arman.Foundation.Core.ServiceLocating;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace GeoGuessr.Game
{
    public interface ILevelViewPort
    {
        UniTask StartTurn(Player player);
        IEnumerable<UniTask<BoardTile>> MovePlayer(Player player, IReadOnlyList<BoardTile> path);

        void UpdatePlayerScore(int score, int change);
        void CloseQuiz();
    }

    public class QuizDatabase : Service
    {
        private Dictionary<QuizType, List<Quiz>> quizesByType = new();

        public QuizDatabase(IEnumerable<Quiz> quizes)
        {
            foreach (var quizType in Enum.GetValues(typeof(QuizType)).Cast<QuizType>())
            {
                quizesByType[quizType] = new List<Quiz>();
            }

            foreach (var quiz in quizes)
            {
                quizesByType[quiz.Type].Add(quiz);
            }
        }

        public IReadOnlyList<Quiz> QuizesOfType(QuizType quizType)
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

    public class LevelMode
    {
        BoardDefinition? _boardDefinition;

        List<string> _localPlayerNames = new();
        List<string> _aiPlayerNames = new();

        public LevelMode()
        {
        }

        public LevelMode SetBoard(BoardDefinition boardDefinition)
        {
            _boardDefinition = boardDefinition;
            return this;
        }

        public LevelMode AddLocalPlayer(string name)
        {
            _localPlayerNames.Add(name);
            return this;
        }

        public LevelMode AddAiPlayer()
        {
            _aiPlayerNames.Add($"AiPlayer{_aiPlayerNames.Count}");
            return this;
        }

        public LevelController BuildLevelController(
            QuizDatabase quizDatabase,
            ILevelViewPort levelViewPort,
            IPlayerControllerViewPort playerViewPort)
        {
            UnityEngine.Debug.Assert(_boardDefinition != null, "No board is set for the level");
            var players = _localPlayerNames
                .Select(name => new Player(name, new LocalPlayerController(playerViewPort)))
                .Concat(_aiPlayerNames.Select(name => new Player(name, new AIPlayerController(playerViewPort))));

            return new LevelController(players, _boardDefinition, quizDatabase, levelViewPort);
        }
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
            foreach(var tileMovement in ViewPort.MovePlayer(CurrentPlayer, path))
            {
                var boardTile = await tileMovement;
                if (boardTile.QuizType == null)
                {
                    AddPlayerScore(EmptyTileScore);
                }
            }
            Board.SetPlayerTile(CurrentPlayer, lastTile);

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
            ViewPort.UpdatePlayerScore(CurrentPlayer.Score,change);
        }
    }
}