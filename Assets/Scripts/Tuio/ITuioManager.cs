using OSC.NET;
using TuioSimulator.Networking;

namespace TuioSimulator.Tuio
{
    public interface ITuioManager
    {
        public OSCBundle FrameBundle { get; }
        public void Update();
    }
}