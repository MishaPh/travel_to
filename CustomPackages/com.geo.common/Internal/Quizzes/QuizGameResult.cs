
namespace Geo.Common.Internal.Quizzes
{
    public readonly struct QuizGameResult
    {
        public readonly QuizData QuizData;
        public readonly bool Win;

        public QuizGameResult(QuizData quizData, bool win) : this()
        {
            QuizData = quizData;
            Win = win;
        }
    }
}
