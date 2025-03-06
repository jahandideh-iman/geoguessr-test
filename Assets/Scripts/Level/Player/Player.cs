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