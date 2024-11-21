using TuioNet.Common;
using TuioNet.Server;
using TuioNet.Tuio11;
using TuioSimulator.Tuio.Common;
using UnityEngine;
using Utils;

namespace TuioSimulator.Tuio.Tuio11
{
    public class Tuio11CursorBehaviour : DebugTuio
    {
        private Tuio11Manager _manager;
        public Tuio11Cursor Cursor { get; private set; }
        private TuioTime _time;
        private Vector2 _lastPosition;
        private RectTransform _rectTransform;
        private RectTransform _parent;
        
        public Vector2 NormalizedPosition { get; private set; }
        private Vector2 _position;

        public Vector2 Position
        {
            get => _position;
            set
            {
                if(RectTransformUtility.ScreenPointToLocalPointInRectangle(_parent, value, Camera.main, out var localPoint))
                {
                    _position = localPoint;
                    _rectTransform.anchoredPosition = localPoint;
                    var normalizedPosition = Rect.PointToNormalized(_parent.rect, localPoint);
                    normalizedPosition.y = 1.0f - normalizedPosition.y;
                    NormalizedPosition = normalizedPosition;
                }
            }    
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Init(Tuio11Manager tuioManager, Vector2 startPosition)
        {
            _parent = transform.parent as RectTransform;
            _manager = tuioManager;
            _time = TuioTime.GetSystemTime();
            Position = startPosition;
            Cursor = new Tuio11Cursor(_time, _manager.CurrentSessionId, 0, NormalizedPosition.FromUnity(), Vector2.zero.FromUnity(), 0f);
            _manager.AddCursor(Cursor);
        }

        private void Update()
        {
            _time = TuioTime.GetSystemTime();
            var velocity = NormalizedPosition - _lastPosition;
            Cursor.Update(_time, NormalizedPosition.FromUnity(), velocity.FromUnity(), velocity.magnitude);
            _lastPosition = NormalizedPosition;
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