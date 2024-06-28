using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using TuioNet.Tuio11;
using TuioSimulator.Networking;
using UnityEngine;

namespace TuioSimulator.Tuio
{
    public class TuioTransmitter : MonoBehaviour
    {
        [SerializeField] private TuioType _tuioType = TuioType.Tuio;
        [SerializeField] private ConnectionType _connectionType = ConnectionType.Websocket;
        [SerializeField] private int _port = 3333;
        [SerializeField] private string _sourceName = "TuioSimulator";

        private IEnumerable<Tuio11Cursor> _cursors;
        private IEnumerable<Tuio11Object> _objects;
        private IEnumerable<Tuio11Blob> _blobs;

        private ITuioServer _server;
        private ITuioManager _manager;
        public ITuioManager Manager => _manager;

        private const float Interval = 1f / 60f;

        private void Awake()
        {
            _server = _connectionType switch
            {
                ConnectionType.Websocket => new TuioWebsocketServer(),
                ConnectionType.Udp => new TuioUdpServer(),
                _ => _server
            };

            _manager = _tuioType switch
            {
                TuioType.Tuio => new Tuio11Manager(_sourceName),
                TuioType.Tuio2 => new Tuio20Manager(),
                _ => _manager
            };
        }

        private void Start()
        {
            _server.Start(IPAddress.Loopback, _port);
            StartCoroutine(Send());
        }
        
        private IEnumerator Send()
        {
            while (Application.isPlaying)
            {
                _manager.Update();
                var sent = _manager.FrameBundle.BinaryData;
                var str = Encoding.ASCII.GetString(sent);
                _server.Send(_manager.FrameBundle.BinaryData);
                yield return new WaitForSeconds(Interval);
            }
        }

        private void OnDestroy()
        {
            _server.Stop();
        }
    }
}