using Geo.Common.Internal;
using Geo.Common.Internal.Quizzes;
using Geo.Common.Internal.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace Geo.Common.Public.Screens
{

    public sealed class FlagQuizScreen : QuizScreenBase
    {
        private const float WaitBeforeAnswer = 0.3f;

        private IAssetLoader _loader;
        private IImageAssetManager _manager;

        [SerializeField]
        private FlagQuizItem[] _items;

        [SerializeField]
        private TextMeshProUGUI _questionText;

        private Action<QuizResult> _onAnswer;
        private QuizData _data;

        [Inject]
        private void Construct(IAssetLoader loader, IImageAssetManager manager)
        {
            _loader = loader;
            _manager = manager;
        }

        public override async Task ShowAsync(QuizData data, Action<QuizResult> resultCallback, CancellationToken token)
        {
            _data = data;
            _questionText.text = data.Question;
            _onAnswer = resultCallback;

            for (var i = 0; i < Mathf.Min(data.Answers.Length, _items.Length); i++)
            {
                if (token.IsCancellationRequested)
                    break;

                var answerIndex = i;
                var flagAsset = _manager.GetImageReference(data.Answers[i].ImageID);
                var sprite = await _loader.LoadAsync(flagAsset, AssetCacheTags.FlagQuizTag);
                _items[i].Show(sprite);
                _items[i].OnClick = () => { AnswerAsync(answerIndex, token).Forget(); };
            }
        }

        private async Task AnswerAsync(int value, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return;

            for (var i = 0; i < _items.Length; i++)
            {
                _items[i].OnClick = null;
            }

            var win = value == _data.CorrectAnswerIndex;
            if (win)
                _items[value].ShowSucced();
            else
                _items[value].ShowFail();

            await Task.Delay(Mathf.RoundToInt(1000 * WaitBeforeAnswer), token);

            _onAnswer?.Invoke(new QuizResult(_data, win));
        }
    }
}
