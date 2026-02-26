using UnityEngine;

namespace TuioSimulator.Tuio.Common
{
    public class TuioTranslation
    {
        public Vector2 Position { get; private set; }
        private Vector2 _lastPosition;
        
        public Vector2 Velocity { get; private set; }
        public float Speed { get; private set; }
        private float _lastSpeed;
        public float Acceleration { get; private set; }

        public void Update(Vector2 normalizedPosition)
        {
            Position = normalizedPosition;
            Velocity = (Position - _lastPosition) / Time.deltaTime;
            Speed = Velocity.magnitude;
            Acceleration = (Speed - _lastSpeed) / Time.deltaTime;
            _lastPosition = Position;
            _lastSpeed = Speed;
        }
    }
}