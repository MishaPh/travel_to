using System.Collections.Generic;

namespace Geo.Common.Internal.Utils
{
    public static class CollectionExtention
    {
        public static void Shuffle<T>(this T[] value)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var tmp = value[i];
                int r = UnityEngine.Random.Range(i, value.Length);
                value[i] = value[r];
                value[r] = tmp;
            }
        }

        public static void Shuffle<T>(this IList<T> value)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var tmp = value[i];
                int r = UnityEngine.Random.Range(i, value.Count);
                value[i] = value[r];
                value[r] = tmp;
            }
        }
    }
}
