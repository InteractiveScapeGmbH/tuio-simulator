using TuioNet.Server;
using TuioNet.Tuio20;
using TuioSimulator.Input;
using TuioSimulator.Tuio.Common;
using TuioSimulator.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

namespace TuioSimulator.Tuio.Tuio20
{
    public class Tuio20TokenBehaviour : TuioTransform
    {
        [SerializeField] private MouseClicker _clicker;
        [SerializeField] private MouseDrager _drager;
        [SerializeField] private TuioUI _ui;
        public Tuio20Token Token { get; private set; }
        private Tuio20Manager _manager;
        private uint _componentId;
        private void OnEnable()
        {
            _drager.OnMove += Move;
            _clicker.OnLeftClick += OnLeftClicked;
            _clicker.OnMiddleClick += OnMiddleClicked;
        }

        private void OnDisable()
        {
            _drager.OnMove -= Move;
            _clicker.OnLeftClick -= OnLeftClicked;
            _clicker.OnMiddleClick -= OnMiddleClicked;
        }

        private void Move(PointerEventData eventData)
        {
            Position = eventData.position;
        }

        private void OnLeftClicked(Vector2 vector)
        {
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
            if (!KeyUtils.IsCommandPressed()) return;
#else
            if (!KeyUtils.IsCtrlPressed()) return;
#endif
            DestroyToken();
        }

        private void OnMiddleClicked(Vector2 vector)
        {
            DestroyToken();
        }

        private void DestroyToken()
        {
            Destroy(gameObject);
        }

        public void Init(Tuio20Manager tuioManager, uint componentId, Vector2 startPosition)
        {
            _manager = tuioManager;
            _componentId = componentId;
            var container = new Tuio20Object(TuioTime, _manager.CurrentSessionId);
            Position = startPosition;
            LastAngle = Angle;
            Token = new Tuio20Token(TuioTime, container, 0, _componentId, NormalizedPosition.FromUnity(), Angle);
            _manager.AddEntity(Token);
        }

        protected override void UpdateTuio(Vector2 velocity, float rotationSpeed)
        {
            Token?.Update(TuioTime, 0, _componentId, NormalizedPosition.FromUnity(), Angle, velocity.FromUnity(), rotationSpeed, velocity.magnitude, 0f);
        }

        private void OnDestroy()
        {
            _manager.RemoveEntity(Token);
        }

        public override string DebugText()
        {
            return  $"s_Id: {Token.SessionId}\n" +
                    $"Angle: {(Token.Angle * 180f / Mathf.PI):f2}\n" +
                    $"Position: {Token.Position:f2}";
        }
    }
}