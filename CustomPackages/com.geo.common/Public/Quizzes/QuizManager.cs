using Geo.Common.Internal.Boards;
using Geo.Common.Internal.Quizzes;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Geo.Common.Public.Screens
{
    public sealed class QuizManager : IQuizManager
    {
        private static readonly IEnumerable AssetLabels = new[] { "QuizCollection" };

        private readonly List<QuizData> _flags = new();
        private readonly List<QuizData> _texts = new();

        public IReadOnlyList<QuizData> Flags => _flags;
        public IReadOnlyList<QuizData> Textes => _texts;

        public QuizData GetRandomQuizData(SpaceType spaceType)
        {
            if (spaceType == SpaceType.FlagQuiz)
                return _flags[Random.Range(0, _flags.Count)];

            return _texts[Random.Range(0, _texts.Count)];
        }

        public async Task LoadAllCollectionsAsync()
        {
            var resourceLocations = await Addressables.LoadResourceLocationsAsync(AssetLabels, Addressables.MergeMode.Intersection, typeof(QuizCollection)).Task;

            foreach (var location in resourceLocations)
            {
                var result = await Addressables.LoadAssetAsync<QuizCollection>(location).Task;
                AddCollection(result);
            }
        }

        private void AddCollection(QuizCollection collection)
        {
            foreach (var asset in collection.GetAll())
            {
                var quiz = JsonUtility.FromJson<QuizData>(asset.text);
                if (quiz.QuestionType == 1)
                    _flags.Add(quiz);
                else
                    _texts.Add(quiz);
            }
        }
    }
}
