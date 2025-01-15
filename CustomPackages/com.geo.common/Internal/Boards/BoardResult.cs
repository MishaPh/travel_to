using System.Collections.Generic;

namespace Geo.Common.Internal.Boards
{
    public readonly struct BoardResult
    {
        public readonly int Roll;
        public readonly int[] Indexes;
        public readonly SpaceInfo HitInfo;

        public BoardResult(int roll, int[] indexes, SpaceInfo hitInfo) : this()
        {
            Roll = roll;
            Indexes = indexes;
            HitInfo = hitInfo;
        }
    }
}
