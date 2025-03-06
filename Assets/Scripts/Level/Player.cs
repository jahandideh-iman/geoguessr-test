using Cysharp.Threading.Tasks;
using System;
namespace GeoGuessr.Game
{
    public interface IPlayerController
    {
        UniTask<int> GetRoll();
        UniTask<Choice> GetQuizAnswer(Quiz quiz, DateTime endTime);

        UniTask ShowQuizResult(Quiz quiz, bool answerWasCorrect);
    }

    public interface IPlayerControllerViewPort
    {
        UniTask ShowAiQuiz(Quiz quiz, DateTime endTime);
        UniTask ShowAiQuizResult(Quiz quiz, bool answerWasCorrect);
        UniTask ShowRollingOption();
        UniTask<Choice> ShowPlayerQuiz(Quiz quiz, DateTime endTime);
        UniTask ShowPlayerQuizResult(Quiz quiz, bool answerWasCorrect);

    }
    

    public class AIPlayerController : IPlayerController
    {
        IPlayerControllerViewPort _viewPort;
        public AIPlayerController(IPlayerControllerViewPort viewPort)
        {
            _viewPort = viewPort;
        }
        public async UniTask<Choice> GetQuizAnswer(Quiz quiz, DateTime endTime)
        {
            await _viewPort.ShowAiQuiz(quiz, endTime);
            return quiz.Choices.RandomElement();
        }

        public async UniTask<int> GetRoll()
        {
            return UnityEngine.Random.Range(1, 4);
        }

        public UniTask ShowQuizResult(Quiz quiz, bool answerWasCorrect)
        {
            return _viewPort.ShowAiQuizResult(quiz, answerWasCorrect);
        }
    }

    public class LocalPlayerController : IPlayerController
    {
        readonly int MaxDiceRoll = 6;

        private readonly IPlayerControllerViewPort _viewPort;

        public LocalPlayerController(IPlayerControllerViewPort viewPort)
        {
            _viewPort = viewPort;
        }

        public UniTask<Choice> GetQuizAnswer(Quiz quiz, DateTime endTime)
        {
            return _viewPort.ShowPlayerQuiz(quiz, endTime);
        }

        public async UniTask<int> GetRoll()
        {
            await _viewPort.ShowRollingOption();
            return 1;
            return UnityEngine.Random.Range(1, MaxDiceRoll+1);
        }

        public UniTask ShowQuizResult(Quiz quiz, bool answerWasCorrect)
        {
            return _viewPort.ShowPlayerQuizResult(quiz, answerWasCorrect);
        }
    }


    public class Player
    {
        public int Score { get; private set; }
        public string Name { get; }
        internal IPlayerController Controller { get; }

        public Player(string name, IPlayerController controller)
        {
            Controller = controller;
            Name = name;
            Score = 0;
        }

        public void AddScore(int value)
        {
            Score += value;
        }
    }
}