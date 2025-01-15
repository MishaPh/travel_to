using Geo.Common.Internal.Quizzes;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Geo.Common.Public.Screens
{
    public abstract class QuizScreenBase : ScreenBase
    {
        public abstract Task ShowAsync(QuizData data, Action<QuizGameResult> resultCallback, CancellationToken token);
    }
}
