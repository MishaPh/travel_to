using Geo.Common.Internal;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Geo.Common.Public.Screens
{
    public sealed class ScreenFactory
    {
        private ScreenCollection _screenCollection;
        private IAssetLoader _assetLoader;
        private DiContainer _diContainer;
        private Canvas _canvas;

        [Inject]
        public void Construct(
            IAssetLoader assetLoader,
            IImageAssetManager imageAssetManager,
            ScreenCollection screenCollection, 
            DiContainer diContainer,
            Canvas canvas)
        {
            _assetLoader = assetLoader;
            _screenCollection = screenCollection;
            _diContainer = diContainer;
            _canvas = canvas;
        }

        public async Task<FlagQuizScreen> CreateFlagQuizScreen(IAssetCacheTag cacheTag)
        {
            var prefab = await _assetLoader.LoadAsync(_screenCollection.GetFlagQuizScreenRef(), cacheTag);
            var screen = _diContainer.InstantiatePrefab(prefab, _canvas.transform).GetComponent<FlagQuizScreen>();
            return screen;
        }

        public async Task<FlagQuizFinishScreen> CreateFlagFinishScreen(IAssetCacheTag cacheTag)
        {
            var prefab = await _assetLoader.LoadAsync(_screenCollection.GetFlagQuizFinishScreenRef(), cacheTag);
            var screen = _diContainer.InstantiatePrefab(prefab, _canvas.transform).GetComponent<FlagQuizFinishScreen>();
            return screen;
        }

        public async Task<TextQuizScreen> CreateTextQuizScreen(IAssetCacheTag cacheTag)
        {
            var prefab = await _assetLoader.LoadAsync(_screenCollection.GetTextQuizScreenRef(), cacheTag);
            var screen = _diContainer.InstantiatePrefab(prefab, _canvas.transform).GetComponent<TextQuizScreen>();
            return screen;
        }

        public async Task<TextQuizFinishScreen> CreateTextFinishScreen(IAssetCacheTag cacheTag)
        {
            var prefab = await _assetLoader.LoadAsync(_screenCollection.GetTextQuizFinishScreenRef(), cacheTag);
            var screen = _diContainer.InstantiatePrefab(prefab, _canvas.transform).GetComponent<TextQuizFinishScreen>();
            return screen;
        }

        public async Task<MainScreen> CreateMainScreen()
        {
            var prefab = await _assetLoader.LoadAsync(_screenCollection.GetMainScreenRef());
            return _diContainer.InstantiatePrefab(prefab, _canvas.transform).GetComponent<MainScreen>();
        }
    }
}
