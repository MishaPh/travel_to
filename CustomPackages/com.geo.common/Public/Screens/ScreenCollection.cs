using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Geo.Common.Public
{
    [CreateAssetMenu(fileName = "ScreenCollection", menuName = "ScriptableObjects/Geo/ScreenCollection", order = 1)]
    public sealed class ScreenCollection : ScriptableObject
    {
        [SerializeField]
        private AssetReferenceGameObject _mainScreenRef;

        [SerializeField]
        private AssetReferenceGameObject _flagQuizScreenRef;
        [SerializeField]
        private AssetReferenceGameObject _flagQuizFinishScreenRef;

        [SerializeField]
        private AssetReferenceGameObject _textQuizScreenRef;
        [SerializeField]
        private AssetReferenceGameObject _textQuizFinishScreenRef;

        public AssetReferenceGameObject GetMainScreenRef() => _mainScreenRef;
        public AssetReferenceGameObject GetFlagQuizScreenRef() => _flagQuizScreenRef;
        public AssetReferenceGameObject GetFlagQuizFinishScreenRef() => _flagQuizFinishScreenRef;
        public AssetReferenceGameObject GetTextQuizScreenRef() => _textQuizScreenRef;
        public AssetReferenceGameObject GetTextQuizFinishScreenRef() => _textQuizFinishScreenRef;
    }
}
