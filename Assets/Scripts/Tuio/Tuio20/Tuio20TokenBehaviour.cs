using TuioNet.Common;
using TuioNet.Tuio20;
using TuioSimulator.Tuio.Common;
using UnityEngine;
using Utils;

namespace TuioSimulator.Tuio.Tuio20
{
    public class Tuio20TokenBehaviour : MonoBehaviour
    {
        private TuioTransmitter _transmitter;
        private Tuio20Token _token;
        private TuioTime _time;
        private Tuio20Manager _manager;
        private RectTransform _rectTransform;

        private Vector2 _lastPosition;
        private float _lastAngle;
        private uint _componentId;
        private float Angle => -_rectTransform.eulerAngles.z * Mathf.Deg2Rad;
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Init(Tuio20Manager tuioManager, uint componentId)
        {
            _manager = tuioManager;
            _componentId = componentId;
            _time = TuioTime.GetSystemTime();
            var container = new Tuio20Object(_time, _manager.CurrentSessionId);
            _lastAngle = Angle;
            _token = new Tuio20Token(_time, container, 0, componentId, GetNormalizedPosition().FromUnity(), Angle,
                Vector2.zero.FromUnity(), 0f, 0f, 0f);
            _manager.AddEntity(_token);
        }

        private Vector2 GetNormalizedPosition()
        {
            var position = _rectTransform.anchoredPosition;
            position.x /= Screen.width;
            position.y /= Screen.height;
            position.y = 1f - position.y;
            return position;
        }

        private void Update()
        {
            _time = TuioTime.GetSystemTime();

            var position = GetNormalizedPosition();
            var velocity = position - _lastPosition;
            _token.Update(_time, 0, _componentId, position.FromUnity(), Angle, velocity.FromUnity(), _lastAngle - Angle,
                velocity.magnitude, 0f);
            _lastPosition = position;
            _lastAngle = Angle;
        }

        private void OnDestroy()
        {
            _manager.RemoveEntity(_token);
        }
    }
}