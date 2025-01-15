using Geo.Common.Internal;
using Geo.Common.Internal.Quizzes;
using Geo.Common.Internal.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Geo.Common.Public.Screens
{
    public sealed class TextQuizScreen : QuizScreenBase
    {
        private const float WaitBeforeAnswer = 0.3f;

        private IAssetLoader _loader;
        private IImageAssetManager _manager;

        [SerializeField]
        private Image _image;
        [SerializeField]
        private AspectRatioFitter _imageAspectRatioFitter;

        [SerializeField]
        private TextQuizItem[] _items;

        [SerializeField]
        private TextMeshProUGUI _questionText;

        private Action<QuizResult> _onResult;
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
            _onResult = resultCallback;

            var asset = _manager.GetImageReference(data.CustomImageID);
            if (asset != null)
            {
                var sprite = await _loader.LoadAsync(asset, AssetCacheTags.TextQuizTag);
                SetImageSprite(sprite);
            }

            for (var i = 0; i < Mathf.Min(data.Answers.Length, _items.Length); i++)
            {
                if (token.IsCancellationRequested)
                    break;

                var answerIndex = i;
                _items[i].Show(data.Answers[i].Text);
                _items[i].OnClick = () => { AnswerAsync(answerIndex, token).Forget(); };
            }
        }

        private void SetImageSprite(Sprite sprite)
        {
            _image.sprite = null;
            if (sprite == null)
                return;

            _image.sprite = sprite;
            var size = sprite.rect.size;
            _imageAspectRatioFitter.aspectRatio = size.x / size.y;
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

            _onResult?.Invoke(new QuizResult(_data, win));
        }
    }
}
