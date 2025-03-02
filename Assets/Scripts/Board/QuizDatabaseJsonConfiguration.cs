using GeoGuessr.Game;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace GeoGuessr.Configuration
{
    public class QuizDatabaseJsonConfiguration
    {
        [JsonObject]
        public struct Quiz
        {
            public string Question;
            public string? CustomImageID;
            public Answer[] Answers;
            public int CorrectAnswerIndex;
            public QuizType QuestionType;

            public Game.Quiz ToGameModel()
            {
                var answers = Answers.Select(answ => answ.ToGameModel()).ToArray();
                return new Game.Quiz(
                    QuestionType,
                    Question,
                    CustomImageID,
                    answers,
                    answers[CorrectAnswerIndex]);
            }
        }

        [JsonObject]
        public struct Answer
        {
            public string? ImageID;
            public string? Text;

            public Game.Choice ToGameModel()
            {
                return new Game.Choice(ImageID, Text);
            }
        }

        public static QuizDatabase Load(string flagQuizJson, string questionQuizJson)
        {
            var flagQuiz = JsonConvert.DeserializeObject<Quiz>(flagQuizJson);
            var questionQuiz = JsonConvert.DeserializeObject<Quiz>(questionQuizJson);

            var database = new QuizDatabase();

            database.AddQuiz(flagQuiz.ToGameModel());
            database.AddQuiz(questionQuiz.ToGameModel());

            return database;
        }

    }
}