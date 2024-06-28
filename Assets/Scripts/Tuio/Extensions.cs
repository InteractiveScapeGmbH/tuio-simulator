using UnityEngine;

namespace TuioSimulator.Tuio
{
    public static class Extensions
    {
        public static System.Numerics.Vector2 FromUnity(this Vector2 vec)
        {
            return new System.Numerics.Vector2(vec.x, vec.y);
        }

        public static Vector2 ToUnity(this System.Numerics.Vector2 vec)
        {
            return new Vector2(vec.X, vec.Y);
        }
    }
}