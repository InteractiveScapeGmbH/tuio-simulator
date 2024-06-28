using OSC.NET;

namespace TuioSimulator.Tuio.Common
{
    public interface ITuioManager
    {
        public OSCBundle FrameBundle { get; }
        public void Update();
    }
}