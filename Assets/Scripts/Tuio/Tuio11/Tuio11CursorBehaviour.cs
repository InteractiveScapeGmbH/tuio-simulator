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
            Cursor = new Tuio11Cursor(TuioTime, _manager.CurrentSessionId, 0, Translation.Position.FromUnity(), Vector2.zero.FromUnity(), 0f);
            _manager.AddCursor(Cursor);
        }

        protected override void UpdateTuio()
        {
            Cursor.Update(TuioTime, Translation.Position.FromUnity(), Translation.Velocity.FromUnity(), Translation.Speed);
        }

        private void OnDestroy()
        {
            _manager.RemoveCursor(Cursor);
        }

        public override string DebugText()
        {
            return $"Id: {Cursor.SessionId}\n" +
                   $"Position: {Cursor.Position:f2}";
        }
    }
}