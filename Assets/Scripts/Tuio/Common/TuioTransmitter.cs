using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TuioNet.Common;
using TuioNet.Server;
using TuioNet.Tuio11;
using UnityEngine;
using Utils;

namespace TuioSimulator.Tuio.Common
{
    public class TuioTransmitter : MonoBehaviour
    {
        [SerializeField] private TuioType _tuioType = TuioType.Tuio;
        [SerializeField] private TuioConnectionType _connectionType = TuioConnectionType.Websocket;
        [SerializeField] private string _sourceName = "TuioSimulator";

        private IEnumerable<Tuio11Cursor> _cursors;
        private IEnumerable<Tuio11Object> _objects;
        private IEnumerable<Tuio11Blob> _blobs;

        private ITuioServer _server;
        private ITuioManager _manager;
        public ITuioManager Manager => _manager;

        private bool _isInitialized;

        private const float Interval = 1f / 60f;

        private void Init()
        {
            var resolution = new Vector2(Screen.width, Screen.height);
            _server = _connectionType switch
            {
                TuioConnectionType.Websocket => new WebsocketServer(Debug.Log),
                TuioConnectionType.UDP => new UdpServer(),
                _ => _server
            };

            _manager = _tuioType switch
            {
                TuioType.Tuio => new Tuio11Manager(_sourceName),
                TuioType.Tuio2 => new Tuio20Manager(_sourceName, resolution.FromUnity()),
                _ => _manager
            };
        }

        public void Open(TuioType tuioType, TuioConnectionType connectionType, IPAddress ipAddress, int port, string sourceName)
        {
            _tuioType = tuioType;
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
                // print(_manager.FrameBundle.Print());
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