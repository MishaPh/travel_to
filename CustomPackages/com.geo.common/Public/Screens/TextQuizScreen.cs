using Geo.Common.Internal;
using Geo.Common.Internal.Quizzes;
using Geo.Common.Internal.Utils;
using System;
using System.Collections.Generic;
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
        private IAssetLoader _loader;
        private IImageAssetManager _manager;

        [SerializeField]
        private Image _image;
        [SerializeField]
        private AspectRatioFitter _imageAspectRatioFitter;

        [SerializeField]
        private List<TextQuizItem> _items;

        [SerializeField]
        private TextMeshProUGUI _questionText;

        private Action<QuizGameResult> _onResult;
        private QuizData _data;
        private int _selectedAnswer = -1;

        [Inject]
        private void Construct(IAssetLoader loader, IImageAssetManager manager)
        {
            _loader = loader;
            _manager = manager;
        }

        public override async Task ShowAsync(QuizData data, Action<QuizGameResult> resultCallback, CancellationToken token)
        {
            _data = data;
            _questionText.text = data.Question;
            _onResult = resultCallback;
            _items.Shuffle();

            var asset = _manager.GetImageReference(data.CustomImageID);
            if (asset != null)
            {
                var sprite = await _loader.LoadAsync(asset, AssetCacheTags.TextQuizTag);
                SetImageSprite(sprite);
            }

            for (var i = 0; i < Mathf.Min(data.Answers.Length, _items.Count); i++)
            {
                if (token.IsCancellationRequested)
                    break;

                var answerIndex = i;
                _items[i].Show(data.Answers[i].Text, () => { ReceiveAnswer(answerIndex); });
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

        private void ReceiveAnswer(int value)
        {
            if (_selectedAnswer != -1)
                return;

            _selectedAnswer = value;

            var win = value == _data.CorrectAnswerIndex;
            if (win)
                _items[value].ShowSucced();
            else
                _items[value].ShowFail();

            _onResult?.Invoke(new QuizGameResult(_data, win));
        }
    }
}
