using TuioNet.Common;
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

        private Vector2 _normalizedPosition;

        public Vector2 NormalizedPosition
        {
            get
            {
                _normalizedPosition.x = _position.x / Screen.width;
                _normalizedPosition.y = _position.y / Screen.height;
                return _normalizedPosition;
            }
        }

        private Vector2 _position;
        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                _rectTransform.anchoredPosition = _position;
            }    
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Init(Tuio20Manager tuioManager, Vector2 position)
        {
            _manager = tuioManager;
            _time = TuioTime.GetSystemTime();
            var container = new Tuio20Object(_time, _manager.CurrentSessionId);
            Position = position;
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