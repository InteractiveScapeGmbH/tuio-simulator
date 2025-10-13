using TuioNet.Common;
using UnityEngine;

namespace TuioSimulator.Tuio.Common
{
    [CreateAssetMenu(fileName = "ServerConfig", menuName = "TuioSimulator/New ServerConfig", order = 0)]
    public class ServerConfig : ScriptableObject
    {
        [field: SerializeField] public TuioType TuioVersion { get; set; } = TuioType.Tuio2;
        [field: SerializeField] public TuioConnectionType ConnectionType { get; set; } = TuioConnectionType.Websocket;
        [field: SerializeField] public string IpAddress { get; set; } = "127.0.0.1";
        [field: SerializeField] public int Port { get; set; } = 3333;
        [field: SerializeField] public string Source { get; set; } = "TuioSimulator";
    }
}