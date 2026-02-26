using System;
using TuioNet.Common;
using UnityEngine;

namespace TuioSimulator.Tuio.Common
{
    public abstract class TuioTransform : DebugTuio
    {
        protected RectTransform RectTransform;
        protected Vector2 LastPosition;
        protected float LastAngle;
        protected float Angle => (-RectTransform.localEulerAngles.z + 360f) * Mathf.Deg2Rad;
        protected Vector2 NormalizedPosition;

        protected RectTransform Parent;
        protected Vector2 _Position;

        protected TuioTime TuioTime;
        
        public Vector2 Position
        {
            get => _Position;
            set
            {
                if(RectTransformUtility.ScreenPointToLocalPointInRectangle(Parent, value, Camera.main, out var localPoint))
                {
                    _Position = localPoint;
                    RectTransform.anchoredPosition = localPoint;
                    var normalizedPosition = Rect.PointToNormalized(Parent.rect, localPoint);
                    normalizedPosition.y = 1.0f - normalizedPosition.y;
                    NormalizedPosition = normalizedPosition;
                }
            }
        }

        protected abstract void UpdateTuio(Vector2 velocity, float rotationSpeed);

        protected void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
            Parent = transform.parent as RectTransform;
            TuioTime = TuioTime.GetSystemTime();
        }

        protected void Update()
        {
            TuioTime = TuioTime.GetSystemTime();
            var velocity = (NormalizedPosition - LastPosition) / Time.deltaTime;
            var rotationSpeed = (Angle - LastAngle) / Time.deltaTime;
            UpdateTuio(velocity, rotationSpeed);
            LastPosition = NormalizedPosition;
            LastAngle = Angle;
        }
    }
    
    
}