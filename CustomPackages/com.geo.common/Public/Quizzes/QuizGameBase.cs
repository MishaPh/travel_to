using Geo.Common.Internal;
using Geo.Common.Internal.Quizzes;
using Geo.Common.Internal.Utils;
using Geo.Common.Public.Screens;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Geo.Common.Public.QuizGames
{
    public abstract class QuizGameBase : IQuizGame
    {
        private const float WaitBeforeAnswer = 0.3f;

        private readonly CancellationTokenSource _cancellationTokenSource = new();

        protected readonly IAssetLoader _loader;
        protected readonly ScreenFactory _screenFactory;

        protected abstract IAssetCacheTag CacheTag { get;}

        private Action<QuizGameResult> _onClose;
        private QuizScreenBase _gameScreen;
        private QuizGameResult _result;

        protected QuizGameBase(IAssetLoader assetLoader, ScreenFactory screenFactory)
        {
            _loader = assetLoader;
            _screenFactory = screenFactory;
        }

        public async Task LoadAsync()
        {
            _gameScreen = await CreateGameScreenAsync();
        }

        public void PlayQuiz(QuizData quizData, Action<QuizGameResult> finished)
        {
            _onClose = finished;
            _gameScreen.ShowAsync(quizData, OnFinisQuiz, _cancellationTokenSource.Token).Forget();
        }

        public void Close()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _onClose?.Invoke(_result);
        }

        protected abstract Task<QuizScreenBase> CreateGameScreenAsync();
        protected abstract Task<QuizFinishScreenBase> CreateFinishScreenAsync();

        private void OnFinisQuiz(QuizGameResult result)
        {
            _result = result;
            ShowFinisQuizAsync(result, _cancellationTokenSource.Token).Forget();
        }

        private async Task ShowFinisQuizAsync(QuizGameResult result, CancellationToken token)
        {
            await Task.Delay(WaitBeforeAnswer.SecondsToTicks(), token);

            var finishScreen = await CreateFinishScreenAsync();

            await finishScreen.ShowResultAsync(result, token);

            UnityEngine.Object.Destroy(_gameScreen.gameObject);

            while (!finishScreen.Completed)
            {
                await Task.Yield();
                if (token.IsCancellationRequested)
                    break;
            }

            UnityEngine.Object.Destroy(finishScreen.gameObject);
            _loader.ClearCacheForTags(CacheTag);
            Close();
        }
    }
}
