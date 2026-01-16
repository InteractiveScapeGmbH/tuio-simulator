using TuioNet.Server;
using TuioNet.Tuio11;
using TuioSimulator.Input;
using TuioSimulator.Tuio.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

namespace TuioSimulator.Tuio.Tuio11
{
    public class Tuio11ObjectBehaviour : TuioTransform
    {
        [SerializeField] private MouseClicker _clicker;
        [SerializeField] private MouseDrager _drager;
        [SerializeField] private TuioUI _ui;

        public Tuio11Object TuioObject { get; private set; }
        private Tuio11Manager _manager;
        
        private uint _componentId;
        private uint _symbolId;

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
            _manager = tuioManager;
            _symbolId = symbolId;
            Position = startPosition;
            LastAngle = Angle;
            TuioObject = new Tuio11Object(Time, _manager.CurrentSessionId, _symbolId, NormalizedPosition.FromUnity(),
                Angle, Vector2.zero.FromUnity(), 0f, 0f, 0f);
            _manager.AddObject(TuioObject);
        }

        protected override void UpdateTuio(Vector2 velocity, float rotationSpeed)
        {
            TuioObject.Update(Time, NormalizedPosition.FromUnity(), Angle, velocity.FromUnity(), rotationSpeed, velocity.magnitude, 0f);
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