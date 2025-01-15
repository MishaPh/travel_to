namespace Geo.Common.Internal
{
    public interface IAssetCacheTag
    {
    }

    public sealed class StringAssetCacheTag : IAssetCacheTag
    {
        public readonly string ID;
        public StringAssetCacheTag(string id)
        {
            ID = id;
        }
    }

    public static class AssetCacheTags
    {
        public static IAssetCacheTag FlagQuizTag = new StringAssetCacheTag("FlagQuizTag");
        public static IAssetCacheTag TextQuizTag = new StringAssetCacheTag("TextQuizTag");
    }
}
