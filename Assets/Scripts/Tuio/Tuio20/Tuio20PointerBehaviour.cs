using TuioNet.Server;
using TuioNet.Tuio20;
using TuioSimulator.Tuio.Common;
using UnityEngine;
using Utils;

namespace TuioSimulator.Tuio.Tuio20
{
    public class Tuio20PointerBehaviour : TuioTransform
    {
        private Tuio20Manager _manager;
        public Tuio20Pointer Pointer { get; private set; }

        public void Init(Tuio20Manager tuioManager, Vector2 startPosition, float angle)
        {
            _manager = tuioManager;
            RectTransform.Rotate(Vector3.back, angle / Mathf.Deg2Rad);
            LastAngle = Angle;
            var container = new Tuio20Object(Time, _manager.CurrentSessionId);
            Position = startPosition;
            Pointer = new Tuio20Pointer(Time, container, 0, 0, NormalizedPosition.FromUnity(),
                PointerOrientationJitter.GetJitteredValue(Angle), 0f, 0f, 0f, Vector2.zero.FromUnity(), 0f, 0f, 0f);
            _manager.AddEntity(Pointer);
        }

        protected override void UpdateTuio(Vector2 velocity, float rotationSpeed)
        {
            Pointer.Update(Time, 0, 0, NormalizedPosition.FromUnity(),
                PointerOrientationJitter.GetJitteredValue(Angle), 0f, 0f, 0f, velocity.FromUnity(), 0f, 0f, 0f);
        }

        private void OnDestroy()
        {
            _manager.RemoveEntity(Pointer);
        }

        public override string DebugText()
        {
            return Pointer.DebugText + $"\nAngle: {Angle * 180.0 / Mathf.PI:F2}";
        }
    }
}
