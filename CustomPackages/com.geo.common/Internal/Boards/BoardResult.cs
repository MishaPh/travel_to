
namespace Geo.Common.Internal.Boards
{
    public readonly struct BoardResult
    {
        public readonly int Roll;
        public readonly int[] Indexes;
        public readonly SpaceInfo Info;

        public BoardResult(int roll, int[] indexes, SpaceInfo info) : this()
        {
            Roll = roll;
            Indexes = indexes;
            Info = info;
        }
    }
}
