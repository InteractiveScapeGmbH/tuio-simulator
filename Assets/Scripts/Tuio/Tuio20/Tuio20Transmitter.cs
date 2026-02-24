using System;
using System.Collections;
using System.Net.Sockets;
using TuioNet.Server;
using TuioSimulator.Tuio.Common;
using TuioSimulator.Tuio.Sxm;
using UnityEngine;
using Utils;

namespace TuioSimulator.Tuio.Tuio20
{
    public class Tuio20Transmitter : TuioTransmitter
    {
        [SerializeField] private SxmConfig _sxmConfig;
        
        protected override void Init()
        {
            base.Init();
            var resolution = new Vector2(Screen.width, Screen.height);
            _manager = new Tuio20Manager(_sourceName, resolution.FromUnity());
        }
        
        protected override IEnumerator Send()
        {
            while (_isInitialized)
            {
                var sxmDefMessage = _sxmConfig.Message;
                _manager.Update();
                try
                {
                    foreach (var frameBundle in _manager.FrameBundles)     
                    {
                        _server.Send(frameBundle.BinaryData);
                    }
                    _server.Send(sxmDefMessage.BinaryData);
                }
                catch (Exception exception)
                {
                    Debug.LogError($"Could not send data: {exception.Message}");
                }
                yield return new WaitForSeconds(Interval);
            }
        }
    }
}