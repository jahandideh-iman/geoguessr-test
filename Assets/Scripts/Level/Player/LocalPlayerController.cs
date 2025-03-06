using Cysharp.Threading.Tasks;
using System;

namespace GeoGuessr.Game
{
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
            return UnityEngine.Random.Range(1, MaxDiceRoll + 1);
        }

        public UniTask ShowQuizResult(Quiz quiz, bool answerWasCorrect)
        {
            return _viewPort.ShowPlayerQuizResult(quiz, answerWasCorrect);
        }
    }
}