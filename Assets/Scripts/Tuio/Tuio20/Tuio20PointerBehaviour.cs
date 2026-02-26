using TuioNet.Server;
using TuioNet.Tuio20;
using TuioSimulator.Tuio.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

namespace TuioSimulator.Tuio.Tuio20
{
    public class Tuio20PointerBehaviour : TuioTransform
    {
        private Tuio20Manager _manager;
        public Tuio20Pointer Pointer { get; private set; }

        public void Init(Tuio20Manager tuioManager, Vector2 startPosition)
        {
            _manager = tuioManager;
            var container = new Tuio20Object(TuioTime, _manager.CurrentSessionId);
            Position = startPosition;
            Pointer = new Tuio20Pointer(TuioTime, container, 0, 0, Translation.Position.FromUnity());
            _manager.AddEntity(Pointer);
        }

        protected override void UpdateTuio()
        {
            Pointer.Update(TuioTime, 0, 0, Translation.Position.FromUnity(), Rotation.Angle, 0f, 0f, 0f, Translation.Velocity.FromUnity(), 0f, Translation.Acceleration, 0f);
        }

        private void OnDestroy()
        {
            _manager.RemoveEntity(Pointer);
        }

        public override string DebugText()
        {
            return $"Id: {Pointer.SessionId}\n" +
                   $"Position: {Pointer.Position:f2}";
        }
    }
}