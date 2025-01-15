using Geo.Common.Internal;
using Geo.Common.Internal.Quizzes;
using Geo.Common.Internal.Utils;
using Geo.Common.Public.Screens;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Geo.Common.Public.QuizGames
{
    public abstract class QuizGameBase : IQuizGame
    {
        protected readonly IAssetLoader _loader;
        protected readonly ScreenFactory _screenFactory;

        private readonly CancellationTokenSource _cancellationTokenSource = new();

        protected abstract IAssetCacheTag CacheTag { get;}

        private Action _onClose;
        private QuizScreenBase _gameScreen;

        protected QuizGameBase(IAssetLoader assetLoader, ScreenFactory screenFactory)
        {
            _loader = assetLoader;
            _screenFactory = screenFactory;
        }

        public async Task LoadAsync()
        {
            _gameScreen = await CreateGameScreenAsync();
        }

        public void PlayQuiz(QuizData quizData, Action finished)
        {
            _onClose = finished;
            _gameScreen.ShowAsync(quizData, OnFinisQuiz, _cancellationTokenSource.Token).Forget();
        }

        public void Close()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _onClose?.Invoke();
        }

        protected abstract Task<QuizScreenBase> CreateGameScreenAsync();
        protected abstract Task<QuizFinishScreenBase> CreateFinishScreenAsync();

        private void OnFinisQuiz(QuizResult result)
        {
            ShowFinisQuizAsync(result, _cancellationTokenSource.Token).Forget();
        }

        private async Task ShowFinisQuizAsync(QuizResult result, CancellationToken token)
        {
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
