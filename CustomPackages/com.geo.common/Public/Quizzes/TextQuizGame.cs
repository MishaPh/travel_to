using Geo.Common.Internal;
using Geo.Common.Public.Screens;
using System.Threading.Tasks;

namespace Geo.Common.Public.QuizGames
{
    public sealed class TextQuizGame : QuizGameBase
    {
        protected override IAssetCacheTag CacheTag => AssetCacheTags.FlagQuizTag;

        public TextQuizGame(IAssetLoader assetLoader, ScreenFactory screenFactory) :
            base(assetLoader, screenFactory)
        {
        }

        protected override async Task<QuizScreenBase> CreateGameScreenAsync()
        {
            return await _screenFactory.CreateTextQuizScreen(CacheTag);
        }

        protected override async Task<QuizFinishScreenBase> CreateFinishScreenAsync()
        {
            return await _screenFactory.CreateTextFinishScreen(CacheTag);
        }
    }
}
