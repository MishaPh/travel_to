
namespace Geo.Common.Internal
{
    public interface ICachedAsset
    {
        void AddTags(params IAssetCacheTag[] tags);
        bool HasAnyOfTags(params IAssetCacheTag[] tags);
        void Release();
    }
}
