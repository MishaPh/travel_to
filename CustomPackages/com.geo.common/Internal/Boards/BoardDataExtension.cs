namespace Geo.Common.Internal.Boards
{
    public static class BoardDataExtension
    {
        public static BoardResult CalculateResult(this BoardData boardData, int from, int dice)
        {
            var lastIndex = from;
            var indexes = new int[dice];
            for (var i = 0; i < dice; i++)
            {
                lastIndex = (lastIndex + 1) % boardData.Spaces.Count;
                indexes[i] = lastIndex;
            }

            return new BoardResult(indexes, boardData.Spaces[lastIndex]);
        }
    }
}
