using System;
using System.Threading.Tasks;

namespace Geo.Common.Internal.Quizzes
{
    public interface IQuizGame
    {
        Task LoadAsync();
        void PlayQuiz(QuizData quizData, Action<QuizGameResult> finished);
        void Close();
    }
}
