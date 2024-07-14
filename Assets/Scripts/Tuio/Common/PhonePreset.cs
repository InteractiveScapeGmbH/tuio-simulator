using UnityEngine;

namespace TuioSimulator.Tuio.Common
{
    [CreateAssetMenu(fileName = "PhonePreset", menuName = "TuioSimulator/New Phone Preset", order = 0)]
    public class PhonePreset : ScriptableObject
    {
        [field:SerializeField] public string Name { get; set; }
        [Tooltip("The size of the phone in mm.")]
        [field:SerializeField] public Vector2 Size { get; set; }
        [field:SerializeField] public Sprite Icon { get; set; }
    }
}