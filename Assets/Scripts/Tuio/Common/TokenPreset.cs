using UnityEngine;

namespace TuioSimulator.Tuio.Common
{
    [CreateAssetMenu(fileName = "TokenPreset", menuName = "TuioSimulator/New Token Preset", order = 0)]
    public class TokenPreset : ScriptableObject
    {
        /// <summary>
        /// Defines the radius of the token in mm.
        /// </summary>
        [field:SerializeField] public string Name { get; set; }
        [Tooltip("The radius of the token in mm.")]
        [field:SerializeField] public float Radius { get; set; }
        [field:SerializeField] public Sprite Icon { get; set; }
    }
}