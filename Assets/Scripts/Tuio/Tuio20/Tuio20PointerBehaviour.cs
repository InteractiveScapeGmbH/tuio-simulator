using TuioNet.Common;
using TuioNet.Server;
using TuioNet.Tuio20;
using UnityEngine;
using Utils;

namespace TuioSimulator.Tuio.Tuio20
{
    public class Tuio20PointerBehaviour : MonoBehaviour
    {
        private Tuio20Manager _manager;
        public Tuio20Pointer Pointer { get; private set; }
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

        public void Init(Tuio20Manager tuioManager, Vector2 startPosition)
        {
            _parent = transform.parent as RectTransform;
            _manager = tuioManager;
            _time = TuioTime.GetSystemTime();
            var container = new Tuio20Object(_time, _manager.CurrentSessionId);
            Position = startPosition;
            Pointer = new Tuio20Pointer(_time, container, 0, 0, NormalizedPosition.FromUnity(), 0f, 0f, 0f, 0f, Vector2.zero.FromUnity(), 0f, 0f, 0f);
            _manager.AddEntity(Pointer);
        }

        private void Update()
        {
            _time = TuioTime.GetSystemTime();
            var velocity = NormalizedPosition - _lastPosition;
            Pointer.Update(_time, 0, 0, NormalizedPosition.FromUnity(), 0f, 0f, 0f, 0f, velocity.FromUnity(), 0f, 0f, 0f);
            _lastPosition = NormalizedPosition;
        }

        private void OnDestroy()
        {
            _manager.RemoveEntity(Pointer);
        }
    }
}