using System;
using System.Collections.Generic;
using TuioNet.Server;
using TuioNet.Tuio20;
using TuioSimulator.Input;
using TuioSimulator.Tuio.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;
using Random = UnityEngine.Random;

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
        private Dictionary<string, Tuio20Mobile> _actives;
        
        public string Data { get; set; } = "Unknown";
        private const string Group = "device_id";

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
        
        private string GenerateShortId(int digits = 6)
        {
            int min = (int)Mathf.Pow(10, digits - 1);
            int max = (int)Mathf.Pow(10, digits) - 1;
            return Random.Range(min, max + 1).ToString();
        }

        public void Init(Tuio20Manager tuioManager, uint componentId, Vector2 startPosition,  ref Dictionary<string, Tuio20Mobile> activeMobiles, string data = null)
        {
            _actives = activeMobiles;
            _manager = tuioManager;
            _componentId = componentId;
            var container = new Tuio20Object(Time, _manager.CurrentSessionId);
            Position = startPosition;
            LastAngle = Angle;
            if (data != null)
            {
                Data = data;
            }
            _symbol = new Tuio20Symbol(Time, container, 0, _componentId, Group, Data);
            _bounds = new Tuio20Bounds(Time, container, NormalizedPosition.FromUnity(), Angle, Size.FromUnity(),
                Area, Vector2.zero.FromUnity(), 0f, 0f, 0f);
            
            _manager.AddEntity(_symbol);
            _manager.AddEntity(_bounds);
            
        }

        protected override void UpdateTuio(Vector2 velocity, float rotationSpeed)
        {
            _symbol.Update(Time, 0, _componentId, Group, Data);
            _bounds.Update(Time, NormalizedPosition.FromUnity(), Angle, Size.FromUnity(), Area, velocity.FromUnity(),
                rotationSpeed, velocity.magnitude, 0);
        }

        private void OnDestroy()
        {
            _manager.RemoveEntity(_symbol);
            _manager.RemoveEntity(_bounds);
            _actives?.Remove(Data);
        }

        public override string DebugText()
        {
            return $"{_symbol.Data}\n" +
                   $"s_Id:{_bounds.SessionId}\n" +
                   $"Angle:{(Angle * 180f / Math.PI):f2}\n" +
                   $"Position:{_bounds.Position:f2}\n" +
                   $"Size:{_bounds.Size:f2}";
        }
    }
}