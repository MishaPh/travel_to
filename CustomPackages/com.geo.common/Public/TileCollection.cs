using Geo.Common.Internal.Boards;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Geo.Common.Public
{
    [CreateAssetMenu(fileName = "TileCollection", menuName = "ScriptableObjects/Geo/TileCollection", order = 1)]
    public sealed class TileCollection : ScriptableObject
    {
        [Serializable]
        public struct Tile
        {
            public SpaceType SpaceType;
            public TileBase Prefab;
        }

        public List<Tile> Tiles = new();

        public TileBase GetTile(SpaceType type)
        {
            return Tiles
                .Where(tile => tile.SpaceType == type)
                .Select(tile => tile.Prefab)
                .First();
        }
    }
}
