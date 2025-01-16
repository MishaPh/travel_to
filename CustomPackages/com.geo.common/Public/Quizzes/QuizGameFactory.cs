using Geo.Common.Internal.Quizzes;
using Zenject;

namespace Geo.Common.Public.QuizGames
{

    public sealed class QuizGameFactory : IQuizGameFactory
    {
        private readonly DiContainer _diContainer;

        public QuizGameFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public T CreateQuiz<T>() where T : IQuizGame
        {
            return _diContainer.Instantiate<T>();
        }
    }
}
