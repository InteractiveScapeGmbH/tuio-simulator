using System;
using System.Collections;
using System.Net;
using TuioNet.Common;
using TuioNet.Server;
using TuioSimulator.Utils;
using UnityEngine;

namespace TuioSimulator.Tuio.Common
{
    public abstract class TuioTransmitter : MonoBehaviour
    {
        [SerializeField] protected TuioConnectionType _connectionType = TuioConnectionType.Websocket;
        [SerializeField] protected string _sourceName = "TuioSimulator";

        private ITuioServer _server;
        protected ITuioManager _manager;
        public ITuioManager Manager => _manager;

        private bool _isInitialized;

        private const float Interval = 1f / 60f;
        private readonly UnityLogger _logger = new UnityLogger();
        protected virtual void Init()
        {
            _server = _connectionType switch
            {
                TuioConnectionType.Websocket => new WebsocketServer(_logger),
                TuioConnectionType.UDP => new UdpServer(),
                _ => _server
            };
        }

        public void Open(TuioConnectionType connectionType, IPAddress ipAddress, int port, string sourceName)
        {
            _connectionType = connectionType;
            _sourceName = sourceName;
            if(_isInitialized) return;
            try
            {
                Init();
                _server.Start(ipAddress, port);
                _isInitialized = true;
                StartCoroutine(Send());
                Debug.Log("Tuio Transmitter Initialized");
            }
            catch (Exception exception)
            {
                Debug.LogError($"Could not start server: {exception.Message}");
            }
        }
        
        private IEnumerator Send()
        {
            while (_isInitialized)
            {
                _manager.Update();
                // print(_manager.FrameBundle.ToString());
                try
                {
                    _server.Send(_manager.FrameBundle.BinaryData);
                }
                catch (Exception exception)
                {
                    Debug.LogError($"Could not send data: {exception.Message}");
                }
                yield return new WaitForSeconds(Interval);
            }
        }

        public void Close()
        {
            _isInitialized = false;
            _manager.Quit();
            try
            {
                _server.Send(_manager.FrameBundle.BinaryData);
            }
            catch (Exception exception)
            {
                Debug.LogError($"Could not send data: {exception.Message}");
            }
            _server.Stop();
        }

        private void OnDestroy()
        {
            if (!_isInitialized) return;
            Close();
            
        }
    }
}