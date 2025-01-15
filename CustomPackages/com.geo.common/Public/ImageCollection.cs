using System.Collections.Generic;
using UnityEngine;

namespace Geo.Common.Public.Screens
{
    [CreateAssetMenu(fileName = "ImageCollection", menuName = "ScriptableObjects/Geo/ImageCollection")]
    public sealed class ImageCollection : ScriptableObject
    {
        [SerializeField]
        private List<ImageAsset> _images;

        public IReadOnlyList<ImageAsset> GetAll()
        {
            return _images;
        }
    }
}
