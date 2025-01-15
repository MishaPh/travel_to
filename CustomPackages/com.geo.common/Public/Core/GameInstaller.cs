using Geo.Common.Internal;
using Geo.Common.Internal.Quizzes;
using Geo.Common.Public.QuizGames;
using Geo.Common.Public.Screens;
using UnityEngine;
using Zenject;

namespace Geo.Common.Public
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField]
        private TileCollection _tileCollection;
        [SerializeField]
        private ScreenCollection _screenCollection;

        public override void InstallBindings()
        {
            Container.Bind<IAssetLoader>().To<AssetLoader>().AsSingle();
            Container.Bind<IImageAssetManager>().To<ImageAssetManager>().AsSingle();
            Container.Bind<IQuizManager>().To<QuizManager>().AsSingle();
            Container.Bind<IQuizGameFactory>().To<QuizGameFactory>().AsSingle();
            Container.Bind<ScreenFactory>().AsSingle();
            
            Container.BindInstance(_tileCollection).IfNotBound();
            Container.BindInstance(_screenCollection).IfNotBound();
        }
    }
}
