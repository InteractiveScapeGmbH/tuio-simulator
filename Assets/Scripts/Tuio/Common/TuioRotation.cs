using UnityEngine;

namespace TuioSimulator.Tuio.Common
{
    public class TuioRotation
    {
        public float Angle { get; private set; }
        private float _lastAngle;
        public float Speed { get; private set; }
        private float _lastSpeed;
        public float Acceleration { get; private set; }
        
        public void Update(float angle)
        {
            Angle = angle;
            Speed = (Angle - _lastAngle) / Time.deltaTime;
            Acceleration = (Speed - _lastSpeed) / Time.deltaTime;
        }
    }
}