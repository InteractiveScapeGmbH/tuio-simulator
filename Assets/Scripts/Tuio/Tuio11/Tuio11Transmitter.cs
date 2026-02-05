using TuioNet.Server;
using TuioSimulator.Tuio.Common;

namespace TuioSimulator.Tuio.Tuio11
{
    public class Tuio11Transmitter: TuioTransmitter
    {
        protected override void Init()
        {
            base.Init();
            _manager = new Tuio11Manager(_sourceName);
        }
    }
}