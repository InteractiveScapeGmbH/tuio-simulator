using TuioNet.Common;
using TuioNet.Server;
using TuioNet.Tuio20;
using TuioSimulator.Tuio.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

namespace TuioSimulator.Tuio.Tuio20
{
    public class Tuio20PointerBehaviour : DebugTuio
    {
        private Tuio20Manager _manager;
        public Tuio20Pointer Pointer { get; private set; }
        private TuioTime _time;
        
        private Vector2 _lastPosition;
        private RectTransform _rectTransform;

        private RectTransform _parent;

        private PointerEventData _pointerData;
        
        private float Angle => -_rectTransform.eulerAngles.z * Mathf.Deg2Rad;

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

        public void Init(Tuio20Manager tuioManager, PointerEventData pointerData, float angle)
        {
            _pointerData = pointerData;
            _parent = transform.parent as RectTransform;
            _manager = tuioManager;
            _time = TuioTime.GetSystemTime();
            _rectTransform.Rotate(Vector3.back, angle / Mathf.Deg2Rad);
            var container = new Tuio20Object(_time, _manager.CurrentSessionId);
            Position = pointerData.position;
            Pointer = new Tuio20Pointer(_time, container, 0, 0, NormalizedPosition.FromUnity(), angle, 0f, 0f, 0f, Vector2.zero.FromUnity(), 0f, 0f, 0f);
            _manager.AddEntity(Pointer);
        }

        private void Update()
        {
            Position = _pointerData.position;
            _time = TuioTime.GetSystemTime();
            var velocity = NormalizedPosition - _lastPosition;
            Pointer.Update(_time, 0, 0, NormalizedPosition.FromUnity(), Angle, 0f, 0f, 0f, velocity.FromUnity(), 0f, 0f, 0f);
            _lastPosition = NormalizedPosition;
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