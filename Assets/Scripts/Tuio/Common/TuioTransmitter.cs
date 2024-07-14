using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TuioNet.Common;
using TuioNet.Server;
using TuioNet.Tuio11;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace TuioSimulator.Tuio.Common
{
    public class TuioTransmitter : MonoBehaviour
    {
        [field:SerializeField] public ServerConfig ServerConfig { get; set; }
        [SerializeField] private bool _openOnStart = false;

        private ITuioServer _server;
        private ITuioManager _manager;
        public ITuioManager Manager => _manager;

        private bool _isInitialized;

        private const float Interval = 1f / 60f;

        private void Init()
        {
            var resolution = new Vector2(Screen.width, Screen.height);
            _server = ServerConfig.ConnectionType switch
            {
                TuioConnectionType.Websocket => new WebsocketServer(Debug.Log),
                TuioConnectionType.UDP => new UdpServer(),
                _ => _server
            };

            _manager = ServerConfig.TuioVersion switch
            {
                TuioVersion.Tuio11 => new Tuio11Manager(ServerConfig.Source),
                TuioVersion.Tuio20 => new Tuio20Manager(ServerConfig.Source, resolution.FromUnity()),
                _ => _manager
            };
        }

        private void Start()
        {
            Init();
            if(_openOnStart)
                Open();
        }

        [ContextMenu("Start Simulator")]
        public void Open()
        {
            if(_isInitialized) return;
            try
            {
                _server.Start(IPAddress.Loopback, ServerConfig.Port);
                StartCoroutine(Send());
                _isInitialized = true;
            }
            catch (Exception exception)
            {
                Debug.LogError($"Could not start server: {exception.Message}");
            }
        }
        
        private IEnumerator Send()
        {
            while (Application.isPlaying)
            {
                _manager.Update();
                _server.Send(_manager.FrameBundle.BinaryData);
                yield return new WaitForSeconds(Interval);
            }
        }

        private void OnDestroy()
        {
            if (!_isInitialized) return;
            _manager.Quit();
            _server.Send(_manager.FrameBundle.BinaryData);
            _server.Stop();
        }
    }
}