using Geo.Common.Internal.Boards;
using Geo.Common.Internal.Quizzes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Geo.Common.Public.Screens
{
    public interface IQuizManager
    {
        IReadOnlyList<QuizData> Flags { get; }
        IReadOnlyList<QuizData> Textes { get; }
        Task LoadAllCollectionsAsync();
        QuizData GetRandomQuizData(SpaceType spaceType);
    }
}
