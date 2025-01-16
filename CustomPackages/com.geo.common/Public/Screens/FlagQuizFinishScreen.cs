using Geo.Common.Internal;
using Geo.Common.Internal.Quizzes;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Geo.Common.Public.Screens
{
    public sealed class FlagQuizFinishScreen : QuizFinishScreenBase
    {
        [SerializeField]
        private FlagQuizItem _item;

        private IAssetLoader _loader;
        private IImageAssetManager _manager;

        [Inject]
        private void Construct(IAssetLoader loader, IImageAssetManager manager)
        {
            _loader = loader;
            _manager = manager;
        }

        public override async Task ShowResultAsync(QuizGameResult quizResult, CancellationToken token)
        {
            var answer = quizResult.QuizData.Answers[quizResult.QuizData.CorrectAnswerIndex];

            Completed = false;

            _winText.SetActive(quizResult.Win);
            _loseText.SetActive(!quizResult.Win);
            _answerText.text = answer.Text;

            var reference = _manager.GetImageReference(answer.ImageID);
            var sprite = reference != null ? await _loader.LoadAsync(reference, AssetCacheTags.FlagQuizTag) : null;
            _item.Show(sprite, null);

            await WaitForClickAsync(token);
        }
    }
}
