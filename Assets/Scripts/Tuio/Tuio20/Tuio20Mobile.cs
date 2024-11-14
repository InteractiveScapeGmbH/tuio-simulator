using System;
using TuioNet.Common;
using TuioNet.Server;
using TuioNet.Tuio20;
using TuioSimulation.Input;
using TuioSimulator.Tuio.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

namespace TuioSimulator.Tuio.Tuio20
{
    public class Tuio20Mobile : MonoBehaviour
    {
        [SerializeField] private MouseClicker _clicker;
        [SerializeField] private MouseDrager _drager;
        
        private Tuio20Bounds _bounds;
        private Tuio20Symbol _symbol;
        
        private TuioTime _time;
        private Tuio20Manager _manager;
        
        private RectTransform _rectTransform;
        private Vector2 _lastPosition;
        private float _lastAngle;
        private uint _componentId;
        private float Angle => -_rectTransform.eulerAngles.z * Mathf.Deg2Rad;

        private string _data;
        private RectTransform _parent;


        private Vector2 Size
        {
            get
            {
                var size = _rectTransform.sizeDelta;
                size.x /= Screen.width;
                size.y /= Screen.height;
                return size;
            }
        }
        
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

        private float Area => Size.x * Size.y;
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }
        
        private void OnEnable()
        {
            _drager.OnMove += Move;
            _clicker.OnMiddleClick += DestroyToken;
        }

        private void OnDisable()
        {
            _drager.OnMove -= Move;
            _clicker.OnMiddleClick -= DestroyToken;
        }
        
        private void Move(PointerEventData eventData)
        {
            Position = eventData.position;
        }

        private void DestroyToken(Vector2 position)
        {
            Destroy(gameObject);
        }

        public void Init(Tuio20Manager tuioManager, uint componentId, Vector2 startPosition)
        {
            _parent = transform.parent as RectTransform;
            _manager = tuioManager;
            _componentId = componentId;
            _time = TuioTime.GetSystemTime();
            var container = new Tuio20Object(_time, _manager.CurrentSessionId);
            Position = startPosition;
            _lastAngle = Angle;
            _data = Guid.NewGuid().ToString();
            _symbol = new Tuio20Symbol(_time, container, 0, _componentId, "sxm", _data);
            _bounds = new Tuio20Bounds(_time, container, NormalizedPosition.FromUnity(), Angle, Size.FromUnity(),
                Area, Vector2.zero.FromUnity(), 0f, 0f, 0f);
            _manager.AddEntity(_symbol);
            _manager.AddEntity(_bounds);
        }

        private void Update()
        {
            _time = TuioTime.GetSystemTime();
            var velocity = NormalizedPosition - _lastPosition;
            var rotationSpeed = _lastAngle - Angle;
            _symbol.Update(_time, 0, _componentId, "sxm", _data);
            _bounds.Update(_time, NormalizedPosition.FromUnity(), Angle, Size.FromUnity(), Area, velocity.FromUnity(),
                rotationSpeed, velocity.magnitude, 0);
            _lastPosition = NormalizedPosition;
            _lastAngle = Angle;
        }

        private void OnDestroy()
        {
            _manager.RemoveEntity(_symbol);
            _manager.RemoveEntity(_bounds);
        }
    }
}