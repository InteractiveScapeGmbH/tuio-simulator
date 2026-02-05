using TuioNet.Server;
using TuioSimulator.Tuio.Common;
using UnityEngine;
using Utils;

namespace TuioSimulator.Tuio.Tuio20
{
    public class Tuio20Transmitter : TuioTransmitter
    {
        protected override void Init()
        {
            base.Init();
            var resolution = new Vector2(Screen.width, Screen.height);
            _manager = new Tuio20Manager(_sourceName, resolution.FromUnity());
        }
    }
}