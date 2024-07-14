using System.Collections.Generic;
using UnityEngine;

namespace TuioSimulator.Tuio.Common
{
    [CreateAssetMenu(fileName = "PresetLibrary", menuName = "TuioSimulator/New Preset Library", order = 0)]
    public class PresetLibrary : ScriptableObject
    {
        [field: SerializeField] public List<TokenPreset> TokenPresets;
        [field: SerializeField] public List<PhonePreset> PhonePresets;
    }
}