namespace Utils
{
    public static class VectorExtensions
    {
        public static UnityEngine.Vector2 ToUnity(this System.Numerics.Vector2 vector)
        {
            return new UnityEngine.Vector2(vector.X, vector.Y);
        }

        public static System.Numerics.Vector2 FromUnity(this UnityEngine.Vector2 vector)
        {
            return new System.Numerics.Vector2(vector.x, vector.y);
        }
    }
}