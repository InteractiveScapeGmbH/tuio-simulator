using TuioNet.Server;
using TuioNet.Tuio11;
using TuioSimulator.Tuio.Common;
using UnityEngine;
using Utils;

namespace TuioSimulator.Tuio.Tuio11
{
    public class Tuio11CursorBehaviour : TuioTransform
    {
        private Tuio11Manager _manager;
        public Tuio11Cursor Cursor { get; private set; }

        public void Init(Tuio11Manager tuioManager, Vector2 startPosition)
        {
            _manager = tuioManager;
            Position = startPosition;
            Cursor = new Tuio11Cursor(Time, _manager.CurrentSessionId, 0, NormalizedPosition.FromUnity(), Vector2.zero.FromUnity(), 0f);
            _manager.AddCursor(Cursor);
        }

        protected override void UpdateTuio(Vector2 velocity, float rotationSpeed)
        {
            Cursor.Update(Time, NormalizedPosition.FromUnity(), velocity.FromUnity(), velocity.magnitude);
        }

        private void OnDestroy()
        {
            _manager.RemoveCursor(Cursor);
        }

        public override string DebugText()
        {
            return Cursor.DebugText;
        }
    }
}