using Geo.Common.Internal.Boards;
using Geo.Common.Internal.Quizzes;

namespace Geo.Common.Public.QuizGames
{
    public static class QuizGameFactoryExtension
    {
        public static IQuizGame CreateQuiz(this IQuizGameFactory factory, SpaceType type)
        {
            return (type == SpaceType.FlagQuiz) ? factory.CreateQuiz<FlagQuizGame>() : factory.CreateQuiz<TextQuizGame>();
        }
    }
}
