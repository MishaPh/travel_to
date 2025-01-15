using System.Collections.Generic;
using UnityEngine;

namespace Geo.Common.Internal.Boards
{

    [CreateAssetMenu(fileName = "BoardData", menuName = "ScriptableObjects/Geo/BoardData", order = 1)]
    public sealed class BoardData : ScriptableObject
    {
        public List<Vector2> Points;
        public List<SpaceInfo> Spaces;

        private void OnValidate()
        {
            if (Points.Count < 4)
            {
                Debug.LogError($"Not enough points in board data {name}");
            }

            if (Spaces.Count < 4)
            {
                Debug.LogError($"Not enough defiend spaces in board data {name}");
            }
        }
    }
}
