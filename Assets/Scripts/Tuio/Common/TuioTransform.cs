using System;
using TuioNet.Common;
using UnityEngine;

namespace TuioSimulator.Tuio.Common
{
    public abstract class TuioTransform : DebugTuio
    {
        protected RectTransform RectTransform;
        private float _angle => (-RectTransform.localEulerAngles.z + 360f) * Mathf.Deg2Rad;
        // protected Vector2 NormalizedPosition;

        private RectTransform _parent;
        // protected Vector2 _Position;

        protected TuioTime TuioTime;

        protected TuioTranslation _translation;
        protected TuioRotation _rotation;

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
            _translation.Update(_position);
            _rotation.Update(_angle);
            UpdateTuio();
        }
    }
    
    
}