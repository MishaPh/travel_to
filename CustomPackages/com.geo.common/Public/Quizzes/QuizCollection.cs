using System.Collections.Generic;
using UnityEngine;

namespace Geo.Common.Public.Screens
{
    [CreateAssetMenu(fileName = "QuizCollection", menuName = "ScriptableObjects/Geo/QuizCollection")]
    public sealed class QuizCollection : ScriptableObject
    {
        [SerializeField]
        private List<TextAsset> _quizes;
        public IReadOnlyList<TextAsset> GetAll => _quizes;
    }
}
