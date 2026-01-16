using UnityEngine;

namespace TuioSimulator.Sxm
{
    [CreateAssetMenu(fileName = "SxmConfig", menuName = "TuioSimulator/New Sxm Config", order = 0)]
    public class SxmConfig : ScriptableObject
    {
        [field:SerializeField] public string WebAppUrl { get; set; }
        [field:SerializeField] public string RoomId { get; set; }
        [field:SerializeField] public string BrokerUrl { get; set; }
        [field:SerializeField] public int BrokerPort { get; set; }

    }
}