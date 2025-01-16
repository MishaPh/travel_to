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
            return await ScreenFactory.CreateFlagQuizScreen(CacheTag);
        }

        protected override async Task<QuizFinishScreenBase> CreateFinishScreenAsync()
        {
            return await ScreenFactory.CreateFlagFinishScreen(CacheTag);
        }
    }
}
