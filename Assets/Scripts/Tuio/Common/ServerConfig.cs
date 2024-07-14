using TuioNet.Common;
using UnityEngine;

namespace TuioSimulator.Tuio.Common
{
    [CreateAssetMenu(fileName = "ServerConfig", menuName = "TuioSimulator/New ServerConfig", order = 0)]
    public class ServerConfig : ScriptableObject
    {
        [field: SerializeField] public TuioVersion TuioVersion { get; set; } = TuioVersion.Tuio20;
        [field: SerializeField] public TuioConnectionType ConnectionType { get; set; } = TuioConnectionType.Websocket;
        [field: SerializeField] public int Port { get; set; } = 3333;
        [field: SerializeField] public string Source { get; set; } = "TuioSimulator";
    }
}