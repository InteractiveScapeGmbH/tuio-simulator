using System;
using System.Collections;
using TuioNet.Server;
using TuioSimulator.Tuio.Common;
using UnityEngine;

namespace TuioSimulator.Tuio.Tuio11
{
    public class Tuio11Transmitter: TuioTransmitter
    {
        protected override void Init()
        {
            base.Init();
            _manager = new Tuio11Manager(_sourceName);
        }

        protected override IEnumerator Send()
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
    }
}