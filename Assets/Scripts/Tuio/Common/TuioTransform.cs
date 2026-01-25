using System;
using TuioNet.Common;
using UnityEngine;

namespace TuioSimulator.Tuio.Common
{
    public abstract class TuioTransform : DebugTuio
    {
        protected RectTransform RectTransform;
        private Vector2 _lastPosition;
        protected float LastAngle;
        protected float Angle => ((-RectTransform.localEulerAngles.z + 360f) % 360f) * Mathf.Deg2Rad;
        protected Vector2 NormalizedPosition;

        private RectTransform _parent;
        private Vector2 _position;

        protected TuioTime Time;

        public Vector2 Position
        {
            get => _position;
            set
            {
                if(RectTransformUtility.ScreenPointToLocalPointInRectangle(_parent, value, Camera.main, out var localPoint))
                {
                    _position = localPoint;
                    RectTransform.anchoredPosition = localPoint;
                    var normalizedPosition = Rect.PointToNormalized(_parent.rect, localPoint);
                    normalizedPosition.y = 1.0f - normalizedPosition.y;
                    NormalizedPosition = normalizedPosition;
                }
            }
        }

        protected abstract void UpdateTuio(Vector2 velocity, float rotationSpeed);

        protected void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
            _parent = transform.parent as RectTransform;
            Time = TuioTime.GetSystemTime();
        }

        protected void Update()
        {
            Time = TuioTime.GetSystemTime();
            var velocity = NormalizedPosition - _lastPosition;
            var rotationSpeed = Angle - LastAngle;
            UpdateTuio(velocity, rotationSpeed);
            _lastPosition = NormalizedPosition;
            LastAngle = Angle;
        }
    }
    
    
}