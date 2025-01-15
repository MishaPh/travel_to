using Geo.Common.Internal.Quizzes;

namespace Geo.Common.Public.Screens
{
    public readonly struct QuizResult
    {
        public readonly QuizData QuizData;
        public readonly bool Win;

        public QuizResult(QuizData quizData, bool win) : this()
        {
            QuizData = quizData;
            Win = win;
        }
    }
}
