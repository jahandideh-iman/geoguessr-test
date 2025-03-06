using GeoGuessr.Game;
using Newtonsoft.Json;
using System.Linq;

namespace GeoGuessr.Configuration
{
    public class QuizJsonConfiguration
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

        public static Game.Quiz Load(string json)
        {
            return JsonConvert.DeserializeObject<Quiz>(json).ToGameModel();
        }

    }
}