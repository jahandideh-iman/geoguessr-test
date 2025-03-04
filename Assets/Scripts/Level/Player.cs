using Cysharp.Threading.Tasks;
using System;

namespace GeoGuessr.Game
{
    public interface IPlayerController
    {
        UniTask<int> GetRoll();
        UniTask<Choice> GetQuizAnswer(Quiz quiz, DateTime endTime);
    }

    public interface IAiPlayerControllerViewPort
    {
        public void ShowQuiz(Quiz quiz, DateTime endTime);
    }
    

    public class AIPlayerController : IPlayerController
    {
        IAiPlayerControllerViewPort _viewPort;
        public AIPlayerController(IAiPlayerControllerViewPort viewPort)
        {
            _viewPort = viewPort;
        }
        public async UniTask<Choice> GetQuizAnswer(Quiz quiz, DateTime endTime)
        {
            _viewPort.ShowQuiz(quiz, endTime);
            await UniTask.WaitForSeconds(1);
            return quiz.Choices.RandomElement();
        }

        public async UniTask<int> GetRoll()
        {
            await UniTask.WaitForSeconds(3);
            return UnityEngine.Random.Range(0, 4);
        }
    }


    public interface ILocalPlayerControllerViewPort
    {
        UniTask ShowRollingOption();
        UniTask<Choice> ShowQuiz(Quiz quiz, DateTime endTime);
    }

    public class LocalPlayerController : IPlayerController
    {
        readonly int MaxDiceRoll = 6;

        private readonly ILocalPlayerControllerViewPort _viewPort;

        public LocalPlayerController(ILocalPlayerControllerViewPort viewPort)
        {
            _viewPort = viewPort;
        }

        public UniTask<Choice> GetQuizAnswer(Quiz quiz, DateTime endTime)
        {
            return _viewPort.ShowQuiz(quiz, endTime);
        }

        public async UniTask<int> GetRoll()
        {
            await _viewPort.ShowRollingOption();
            return UnityEngine.Random.Range(1, MaxDiceRoll+1);
        }
    }


    public class Player
    {
        public int Score { get; private set; }
        public string Name { get; }
        private readonly IPlayerController _playerController;

        public Player(string name, IPlayerController playerController)
        {
            _playerController = playerController;
            Name = name;
            Score = 0;
        }

        public int Index { get; }

        public void AddScore(int value)
        {
            Score += value;
        }

        public UniTask<int> GetRoll()
        {
            return _playerController.GetRoll();
        }

        public UniTask<Choice> GetQuizAnswer(Quiz quiz, DateTime endTime)
        {
            return _playerController.GetQuizAnswer(quiz, endTime);
        }
    }
}