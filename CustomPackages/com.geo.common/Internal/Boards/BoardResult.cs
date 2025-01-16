
namespace Geo.Common.Internal.Boards
{
    public readonly struct BoardResult
    {
        public readonly int[] Indexes;
        public readonly SpaceInfo Info;

        public BoardResult(int[] indexes, SpaceInfo info) : this()
        {
            Indexes = indexes;
            Info = info;
        }
    }
}
