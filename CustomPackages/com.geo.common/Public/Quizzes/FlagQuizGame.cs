using Geo.Common.Internal;
using Geo.Common.Public.Screens;
using System.Threading.Tasks;

namespace Geo.Common.Public.QuizGames
{
    public sealed class FlagQuizGame : QuizGameBase
    {
        protected override IAssetCacheTag CacheTag => AssetCacheTags.FlagQuizTag;

        public FlagQuizGame(IAssetLoader assetLoader, ScreenFactory screenFactory) :
            base(assetLoader, screenFactory)
        {
        }

        protected override async Task<QuizScreenBase> CreateGameScreenAsync()
        {
            var screen = await _screenFactory.CreateFlagQuizScreen(CacheTag);
            return screen;
        }

        protected override async Task<QuizFinishScreenBase> CreateFinishScreenAsync()
        {
            var screen = await _screenFactory.CreateFlagFinishScreen(CacheTag);
            return screen;
        }
    }
}
