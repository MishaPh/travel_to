using Geo.Common.Internal;
using Geo.Common.Internal.Quizzes;
using Geo.Common.Internal.Utils;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace Geo.Common.Public.Screens
{
    public sealed class FlagQuizScreen : QuizScreenBase
    {
        private IAssetLoader _loader;
        private IImageAssetManager _manager;

        [SerializeField]
        private List<FlagQuizItem> _items;

        [SerializeField]
        private TextMeshProUGUI _questionText;

        private Action<QuizGameResult> _onAnswer;
        private QuizData _data;

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
            _onAnswer = resultCallback;
            _items.Shuffle();

            var tasks = new List<Task>();

            for (var i = 0; i < Mathf.Min(data.Answers.Length, _items.Count); i++)
            {
                tasks.Add(SetItemSpriteAsync(_items[i], data.Answers[i].ImageID, i, token));
            }

            await Task.WhenAll(tasks);

            StartTimer();
        }

        private async Task SetItemSpriteAsync(FlagQuizItem item, string imageID, int answerIndex, CancellationToken token)
        {
            var asset = _manager.GetImageReference(imageID);
            if (asset == null || token.IsCancellationRequested)
                return;

            var sprite = await _loader.LoadAsync(asset, AssetCacheTags.FlagQuizTag);
            item.Show(sprite, () => { UpdateAnswer(answerIndex); });
        }

        private void UpdateAnswer(int value)
        {
            var win = value == _data.CorrectAnswerIndex;
            if (win)
                _items[value].ShowSucced();
            else
                _items[value].ShowFail();

            StopTimer();
            SendResult(win);
        }

        protected override void OnTimeOut()
        {
            SendResult(false);
        }

        private void SendResult(bool win)
        {
            _items.ForEach(item => item.DisableClick());
            _onAnswer?.Invoke(new QuizGameResult(_data, win));
            _onAnswer = null;
        }
    }
}
