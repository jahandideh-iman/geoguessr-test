#nullable enable

namespace GeoGuessr.Game
{
    public enum QuizType
    {
        Question = 0,
        Flag = 1,
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

}