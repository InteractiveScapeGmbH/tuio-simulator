using System;
using System.Net;
using UnityEngine;

namespace TuioSimulator.Networking
{
    public class TuioServerBehaviour : MonoBehaviour
    {
        [SerializeField] private ConnectionType _connectionType = ConnectionType.Websocket;
        [SerializeField] private int _port = 9000;

        private ITuioServer _server;

        private void Awake()
        {
            _server = _connectionType switch
            {
                ConnectionType.Websocket => new TuioWebsocketServer(),
                ConnectionType.Udp => new TuioUdpServer(),
                _ => _server
            };
        }

        private void Start()
        {
            _server.Start(IPAddress.Loopback, _port);
        }

        private void OnDestroy()
        {
            _server.Stop();
        }

        [ContextMenu("Send")]
        public void Send()
        {
            _server.Send("Hello from Unity.");
        }
    }
}
