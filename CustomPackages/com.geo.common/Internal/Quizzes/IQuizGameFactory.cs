
namespace Geo.Common.Internal.Quizzes
{

    public interface IQuizGameFactory
    {
        T CreateQuiz<T>() where T: IQuizGame;
    }
}
