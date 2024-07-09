using TuioNet.Common;
using TuioNet.Tuio11;
using TuioSimulator.Tuio.Common;
using UnityEngine;
using Utils;

namespace TuioSimulator.Tuio.Tuio11
{
    public class Tuio11ObjectBehaviour : MonoBehaviour
    {
        private TuioTransmitter _transmitter;
        private Tuio11Object _object;
        private TuioTime _time;
        private Vector2 _lastPosition;
        private float _lastAngle;
        private Tuio11Manager _manager;
        private RectTransform _rectTransform;
        
        
        
        private uint _symbolId;

        private Vector2 NormalizedPosition
        {
            get
            {
                var position = _rectTransform.anchoredPosition;
                position.x /= Screen.width;
                position.y /= Screen.height;
                position.y = 1f - position.y;
                return position;
            }
        }

        private float Angle => -_rectTransform.eulerAngles.z * Mathf.Deg2Rad;
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Init(Tuio11Manager tuioManager, uint symbolId)
        {
            _manager = tuioManager;
            _symbolId = symbolId;
            _time = TuioTime.GetSystemTime();
            _object = new Tuio11Object(_time, _manager.CurrentSessionId, symbolId, NormalizedPosition.FromUnity(),
                Angle, Vector2.zero.FromUnity(), 0f, 0f, 0f);
            _manager.AddObject(_object);
        }

        private void Update()
        {
            _time = TuioTime.GetSystemTime();
            var position = NormalizedPosition;
            var angle = Angle;
            var rotationSpeed = angle - _lastAngle;
            var velocity = position - _lastPosition;
            _object.Update(_time, position.FromUnity(), angle, velocity.FromUnity(), rotationSpeed, velocity.magnitude, 0f);
            _lastPosition = position;
            _lastAngle = angle;
        }

        private void OnDestroy()
        {
            _manager.RemoveObject(_object);
        }
    }
}