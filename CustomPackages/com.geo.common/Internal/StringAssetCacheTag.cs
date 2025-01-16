namespace Geo.Common.Internal
{
    public sealed class StringAssetCacheTag : IAssetCacheTag
    {
        public readonly string ID;

        public StringAssetCacheTag(string id)
        {
            ID = id;
        }
    }
}
