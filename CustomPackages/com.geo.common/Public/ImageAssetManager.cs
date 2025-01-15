using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Geo.Common.Public.Screens
{
    public sealed class ImageAssetManager : IImageAssetManager
    {
        private readonly static IEnumerable AssetKey = new[] { "ImageCollection" };

        private readonly Dictionary<string, ImageAsset> _flags = new();

        public async Task LoadAllCollectionsAsync()
        {
            var resourceLocations = await Addressables.LoadResourceLocationsAsync(AssetKey, Addressables.MergeMode.Intersection, typeof(ImageCollection)).Task;

            foreach(var location in resourceLocations)
            {
                var result = await Addressables.LoadAssetAsync<ImageCollection>(location).Task;
                AddCollection(result);
            }
        }

        private void AddCollection(ImageCollection collection)
        {
            foreach(var flagAsset in collection.GetAll())
            {
                _flags.TryAdd(flagAsset.ImageID, flagAsset);
            }
        }

        public AssetReferenceSprite GetImageReference(string imageID)
        {
            if (_flags.TryGetValue(imageID, out var result))
                return result.Sprite;

            Debug.LogError($"imageID \"{imageID}\" asset does not exist");
            return null;
        }
    }
}
