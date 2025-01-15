namespace Geo.Common.Internal.Boards
{
    public static class SpaceTypeHelper
    {
        public static SpaceType GetRndSpecial(float percentage01)
        {
            if (percentage01 < UnityEngine.Random.Range(0.0f, 1.0f))
                return SpaceType.Empty;

            return 0.5f < UnityEngine.Random.Range(0.0f, 1.0f) ? SpaceType.FlagQuiz : SpaceType.TextQuiz;
        }
    }
}
