using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Geo.Common.Internal;
using UnityEngine.Pool;

namespace Geo.Common.Public.Core
{
    public sealed class AssetLoader : IAssetLoader
    {
        private readonly Dictionary<string, ICachedAsset> _cache = new();

        public Task<TObject> LoadAsync<TObject>(AssetReferenceT<TObject> reference, params IAssetCacheTag[] tags) where TObject : Object
        {
            UnityEngine.Assertions.Assert.IsNotNull(reference);

            if (_cache.TryGetValue(reference.AssetGUID, out var result))
            { 
                return ((CachedAsset<TObject>)result).GetAsync();
            }

            var cachedAsset = new CachedAsset<TObject>(reference);
            cachedAsset.AddTags(tags);
            _cache.Add(reference.AssetGUID, cachedAsset);
            return cachedAsset.GetAsync();
        }

        public void ClearCacheForTags(params IAssetCacheTag[] tags)
        {
            var toRemove = ListPool<string>.Get();

            foreach (var item in _cache)
            {
                if (tags.Length > 0 && !item.Value.HasAnyOfTags(tags))
                    continue;

                item.Value.Release();
                toRemove.Add(item.Key);
            }
            toRemove.ForEach(key => _cache.Remove(key));

            ListPool<string>.Release(toRemove);
        }
    }

}
