
using Arman.Foundation.Core.ServiceLocating;
using System;
using System.Collections.Generic;
using System.Linq;


namespace GeoGuessr.Game
{
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
}