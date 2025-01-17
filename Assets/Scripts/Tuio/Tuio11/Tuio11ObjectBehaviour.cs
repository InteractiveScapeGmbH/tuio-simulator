using TuioNet.Common;
using TuioNet.Server;
using TuioNet.Tuio11;
using TuioSimulator.Input;
using TuioSimulator.Tuio.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

namespace TuioSimulator.Tuio.Tuio11
{
    public class Tuio11ObjectBehaviour : DebugTuio
    {
        [SerializeField] private MouseClicker _clicker;
        [SerializeField] private MouseDrager _drager;
        [SerializeField] private TuioUI _ui;

        public Tuio11Object TuioObject { get; private set; }
        private TuioTime _time;
        private Tuio11Manager _manager;
        
        private RectTransform _rectTransform;
        private Vector2 _lastPosition;
        private float _lastAngle;
        private uint _componentId;
        private float Angle => -_rectTransform.eulerAngles.z * Mathf.Deg2Rad;

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
        
        private uint _symbolId;

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

        public void Init(Tuio11Manager tuioManager, uint symbolId, Vector2 startPosition)
        {
            _parent = transform.parent as RectTransform;
            _manager = tuioManager;
            _symbolId = symbolId;
            _time = TuioTime.GetSystemTime();
            Position = startPosition;
            _lastAngle = Angle;
            TuioObject = new Tuio11Object(_time, _manager.CurrentSessionId, _symbolId, NormalizedPosition.FromUnity(),
                Angle, Vector2.zero.FromUnity(), 0f, 0f, 0f);
            _manager.AddObject(TuioObject);
        }

        private void Update()
        {
            _time = TuioTime.GetSystemTime();
            var velocity = NormalizedPosition - _lastPosition;
            var rotationSpeed = _lastAngle - Angle;
            TuioObject.Update(_time, NormalizedPosition.FromUnity(), Angle, velocity.FromUnity(), rotationSpeed, velocity.magnitude, 0f);
            _lastPosition = NormalizedPosition;
            _lastAngle = Angle;
        }

        private void OnDestroy()
        {
            _manager.RemoveObject(TuioObject);
        }

        public override string DebugText()
        {
            return TuioObject.DebugText;
        }
    }
}