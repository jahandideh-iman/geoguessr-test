using Cysharp.Threading.Tasks;
using System;
using GeoGuessr.Utilities;

namespace GeoGuessr.Game
{
    public class AiPlayerController : IPlayerController
    {
        IPlayerControllerViewPort _viewPort;
        public AiPlayerController(IPlayerControllerViewPort viewPort)
        {
            _viewPort = viewPort;
        }

        public async UniTask<Choice> GetQuizAnswer(Quiz quiz, DateTime endTime)
        {
            await _viewPort.ShowAiQuiz(quiz, endTime);
            return quiz.Choices.RandomElement();
        }

        public UniTask<int> GetRoll()
        {
            return UniTask.FromResult(UnityEngine.Random.Range(1, 4));
        }

        public UniTask ShowQuizResult(Quiz quiz, bool answerWasCorrect)
        {
            return _viewPort.ShowAiQuizResult(quiz, answerWasCorrect);
        }
    }
}