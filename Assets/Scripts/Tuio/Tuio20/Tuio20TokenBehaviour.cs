using System;
using TuioNet.Common;
using TuioNet.Tuio20;
using TuioSimulation.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

namespace TuioSimulator.Tuio.Tuio20
{
    public class Tuio20TokenBehaviour : MonoBehaviour
    {
        [SerializeField] private MouseClicker _clicker;
        [SerializeField] private MouseDrager _drager;
        public Tuio20Token Token { get; private set; }
        private TuioTime _time;
        private Tuio20Manager _manager;
        
        private RectTransform _rectTransform;
        private Vector2 _lastPosition;
        private float _lastAngle;
        private uint _componentId;
        private float Angle => -_rectTransform.eulerAngles.z * Mathf.Deg2Rad;
        
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

        public void Init(Tuio20Manager tuioManager, uint componentId, Vector2 position)
        {
            _manager = tuioManager;
            _componentId = componentId;
            _time = TuioTime.GetSystemTime();
            var container = new Tuio20Object(_time, _manager.CurrentSessionId);
            Position = position;
            _lastAngle = Angle;
            Token = new Tuio20Token(_time, container, 0, componentId, NormalizedPosition.FromUnity(), Angle,
                Vector2.zero.FromUnity(), 0f, 0f, 0f);
            _manager.AddEntity(Token);
        }

        private void Update()
        {
            _time = TuioTime.GetSystemTime();
            var velocity = NormalizedPosition - _lastPosition;
            Token.Update(_time, 0, _componentId, NormalizedPosition.FromUnity(), Angle, velocity.FromUnity(), _lastAngle - Angle,
                velocity.magnitude, 0f);
            _lastPosition = NormalizedPosition;
            _lastAngle = Angle;
        }

        private void OnDestroy()
        {
            _manager.RemoveEntity(Token);
        }
    }
}