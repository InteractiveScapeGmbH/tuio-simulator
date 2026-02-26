using System;
using TuioNet.Common;
using UnityEngine;

namespace TuioSimulator.Tuio.Common
{
    public abstract class TuioTransform : DebugTuio
    {
        protected RectTransform RectTransform;
        private float Angle => (-RectTransform.localEulerAngles.z + 360f) * Mathf.Deg2Rad;

        private RectTransform _parent;

        protected TuioTime TuioTime;

        protected readonly TuioTranslation Translation = new();
        protected readonly TuioRotation Rotation = new();

        private Vector2 _position;
        public Vector2 Position
        {
            set
            {
                if(RectTransformUtility.ScreenPointToLocalPointInRectangle(_parent, value, Camera.main, out var localPoint))
                {
                    RectTransform.anchoredPosition = localPoint;
                    var normalizedPosition = Rect.PointToNormalized(_parent.rect, localPoint);
                    normalizedPosition.y = 1.0f - normalizedPosition.y;
                    _position = normalizedPosition;
                }
            }
        }

        protected abstract void UpdateTuio();

        protected void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
            _parent = transform.parent as RectTransform;
            TuioTime = TuioTime.GetSystemTime();
        }

        protected void Update()
        {
            TuioTime = TuioTime.GetSystemTime();
            Translation.Update(_position);
            Rotation.Update(Angle);
            UpdateTuio();
        }
    }
    
    
}