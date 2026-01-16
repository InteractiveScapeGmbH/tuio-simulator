using System;
using TuioNet.Server;
using TuioNet.Tuio20;
using TuioSimulator.Input;
using TuioSimulator.Tuio.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

namespace TuioSimulator.Tuio.Tuio20
{
    public class Tuio20Mobile : TuioTransform
    {
        [SerializeField] private MouseClicker _clicker;
        [SerializeField] private MouseDrager _drager;
        
        private Tuio20Bounds _bounds;
        private Tuio20Symbol _symbol;
        
        private Tuio20Manager _manager;
        
        private uint _componentId;
        private string _data = "Unknown";


        private Vector2 Size
        {
            get
            {
                var size = RectTransform.sizeDelta;
                size.x /= Screen.width;
                size.y /= Screen.height;
                return size;
            }
        }

        private float Area => Size.x * Size.y;
        
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
            _manager = tuioManager;
            _componentId = componentId;
            var container = new Tuio20Object(Time, _manager.CurrentSessionId);
            Position = startPosition;
            LastAngle = Angle;
            _symbol = new Tuio20Symbol(Time, container, 0, _componentId, "sxm", _data);
            _bounds = new Tuio20Bounds(Time, container, NormalizedPosition.FromUnity(), Angle, Size.FromUnity(),
                Area, Vector2.zero.FromUnity(), 0f, 0f, 0f);
            _manager.AddEntity(_symbol);
            _manager.AddEntity(_bounds);
        }

        protected override void UpdateTuio(Vector2 velocity, float rotationSpeed)
        {
            _symbol.Update(Time, 0, _componentId, "sxm", _data);
            _bounds.Update(Time, NormalizedPosition.FromUnity(), Angle, Size.FromUnity(), Area, velocity.FromUnity(),
                rotationSpeed, velocity.magnitude, 0);
        }

        private void OnDestroy()
        {
            _manager.RemoveEntity(_symbol);
            _manager.RemoveEntity(_bounds);
        }

        public override string DebugText()
        {
            return $"{_symbol.Data}\n{_bounds.DebugText}";
        }
    }
}