using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Geo.Common.Internal;

namespace Geo.Common.Public
{
    internal class CachedAsset<TObject> : ICachedAsset where TObject : Object
    {
        private readonly AsyncOperationHandle<TObject> _handle;
        private readonly HashSet<IAssetCacheTag> _tags = new();

        public CachedAsset(AssetReferenceT<TObject> reference)
        {
            _handle = Addressables.LoadAssetAsync<TObject>(reference);
        }

        public void AddTags(params IAssetCacheTag[] tags)
        {
            _tags.UnionWith(tags);
        }

        public Task<TObject> GetAsync()
        {
            if (_handle.IsDone)
                return Task.FromResult(_handle.Result);

            return _handle.Task;
        }

        public bool HasAnyOfTags(params IAssetCacheTag[] tags)
        {
            return _tags.Overlaps(tags);
        }

        public void Release()
        {
            Addressables.Release(_handle);
        }
    }

}
