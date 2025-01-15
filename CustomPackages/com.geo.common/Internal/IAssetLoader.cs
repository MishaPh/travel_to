using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Geo.Common.Internal
{
    public interface IAssetLoader
    {
        Task<TObject> LoadAsync<TObject>(AssetReferenceT<TObject> reference, params IAssetCacheTag[] tags) where TObject : Object;
        void ClearCacheForTags(params IAssetCacheTag[] tags);
    }

}
