using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Geo.Common.Public.Screens
{
    public interface IImageAssetManager
    {
        Task LoadAllCollectionsAsync();
        AssetReferenceSprite GetImageReference(string imageID);
    }
}
