using System;
using TuioNet.Common;
using TuioNet.Server;
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
        [SerializeField] private GameObject _selection;
        [SerializeField] private GameObject _lift;
        public Tuio20Token Token { get; private set; }
        private TuioTime _time;
        private Tuio20Manager _manager;
        
        private RectTransform _rectTransform;
        private Vector2 _lastPosition;
        private float _lastAngle;
        private uint _componentId;
        private float Angle => -_rectTransform.eulerAngles.z * Mathf.Deg2Rad;

        private RectTransform _parent;
        
        public Vector2 NormalizedPosition { get; private set; }

        private bool _selected;

        public bool Selected
        {
            get => _selected;
            private set
            {
                _selected = value;
                _selection.SetActive(_selected);
            }
        }

        private bool _grounded = true;

        public bool Grounded
        {
            get => _grounded;
            private set
            {
                _grounded = value;
                _lift.SetActive(!_grounded);
            }
        }

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
                    NormalizedPosition = Rect.PointToNormalized(_parent.rect, localPoint);
                }
            }    
        }
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            _drager.OnMove += Move;
            _clicker.OnLeftClick += ToggleSelection;
            _clicker.OnMiddleClick += DestroyToken;
            _clicker.OnRightClick += ToggleGrounded;
        }

        private void OnDisable()
        {
            _drager.OnMove -= Move;
            _clicker.OnLeftClick -= ToggleSelection;
            _clicker.OnMiddleClick -= DestroyToken;
            _clicker.OnRightClick -= ToggleGrounded;
        }

        private void ToggleGrounded(Vector2 obj)
        {
            Grounded = !Grounded;
        }

        private void ToggleSelection(Vector2 obj)
        {
            Selected = !Selected;
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
            _parent = transform.parent as RectTransform;
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