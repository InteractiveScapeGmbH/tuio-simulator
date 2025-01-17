using System.Text;
using OSC.NET;

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

        public static string Print(this OSCBundle bundle)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("OSCBundle[");
    
            // Add timestamp if present
            sb.Append($"Timestamp: {bundle.getTimeStamp()}");
    
            // Add bundle elements (messages and nested bundles)
            if (bundle.Values != null && bundle.Values.Count > 0)
            {
                sb.Append(", Elements: [");
                for (int i = 0; i < bundle.Values.Count; i++)
                {
                    if (i > 0) sb.Append(", ");
            
                    // Handle null elements
                    if (bundle.Values[i] == null)
                    {
                        sb.Append("null");
                        continue;
                    }
            
                    // Add each element's string representation
                    sb.Append(bundle.Values[i].ToString());
                }
                sb.Append("]");
            }
    
            sb.Append("]");
            return sb.ToString();
        }
    }
}