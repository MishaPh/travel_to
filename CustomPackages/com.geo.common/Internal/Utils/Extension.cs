using System;
using UnityEngine;

namespace Geo.Common.Internal
{

    public static class Extension
    {
        public static Vector3 ToVector3XZ(this Vector2 value)
        {
            return new Vector3(value.x, 0, value.y);
        }

        public static Vector2 ToVector2XZ(this Vector3 value, float step)
        {
            return new Vector2(RoundToStep(value.x, step), RoundToStep(value.z, step));
        }

        public static float RoundToStep(this float value, float step)
        {
            return (float)Math.Round(value / step) * step;
        }

        public static int SecondsToTicks(this float seconds)
        {
            return Mathf.RoundToInt(seconds * 1000);
        }
    }
}
