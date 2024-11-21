using UnityEngine;

namespace TuioSimulator.Tuio.Common
{
    [CreateAssetMenu(fileName = "CurrentId", menuName = "TuioSimulator/New CurrentID", order = 0)]
    public class CurrentIdSO : ScriptableObject
    {
        [field:SerializeField] public uint CurrentId { get; set; }
    }
}