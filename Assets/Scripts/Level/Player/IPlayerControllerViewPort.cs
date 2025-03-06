using Cysharp.Threading.Tasks;
using System;

namespace GeoGuessr.Game
{
    public interface IPlayerControllerViewPort
    {
        UniTask ShowAiQuiz(Quiz quiz, DateTime endTime);
        UniTask ShowAiQuizResult(Quiz quiz, bool answerWasCorrect);
        UniTask ShowRollingOption();
        UniTask<Choice> ShowPlayerQuiz(Quiz quiz, DateTime endTime);
        UniTask ShowPlayerQuizResult(Quiz quiz, bool answerWasCorrect);
    }
}