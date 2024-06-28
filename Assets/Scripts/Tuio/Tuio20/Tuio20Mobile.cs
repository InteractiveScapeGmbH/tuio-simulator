using TuioNet.Common;
using TuioNet.Tuio20;
using TuioSimulator.Tuio.Common;
using UnityEngine;
using Utils;

namespace TuioSimulator.Tuio.Tuio20
{
    public class Tuio20Mobile : MonoBehaviour
    {
        private TuioTransmitter _transmitter;
        private Tuio20Bounds _bounds;
        private Tuio20Symbol _symbol;
        private Tuio20Manager _manager;
        private RectTransform _rectTransform;

        private Vector2 _lastPosition;
        private float _lastAngle;
        private int _componentId;
        private float Angle => -_rectTransform.eulerAngles.z * Mathf.Deg2Rad;

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

        private float Area => Size.x * Size.y;
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Init(Tuio20Manager tuioManager, int componentId)
        {
            _manager = tuioManager;
            _componentId = componentId;
            var time = TuioTime.GetSystemTime();
            var container = new Tuio20Object(time, _manager.CurrentSessionId);
            _lastAngle = Angle;
            _symbol = new Tuio20Symbol(time, container, 0, componentId, "sxm", "unknown");
            _bounds = new Tuio20Bounds(time, container, GetNormalizedPosition().FromUnity(), Angle, Size.FromUnity(),
                Area, Vector2.zero.FromUnity(), 0f, 0f, 0f);
            _manager.AddEntity(_symbol);
            _manager.AddEntity(_bounds);
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
            var time = TuioTime.GetSystemTime();

            var position = GetNormalizedPosition();
            var velocity = position - _lastPosition;
            _symbol.Update(time, 0, _componentId, "sxm", "unknown");
            _bounds.Update(time, position.FromUnity(), Angle, Size.FromUnity(), Area, velocity.FromUnity(),
                Angle - _lastAngle, velocity.magnitude, 0);
            _lastPosition = position;
            _lastAngle = Angle;
        }

        private void OnDestroy()
        {
            _manager.RemoveEntity(_symbol);
            _manager.RemoveEntity(_bounds);
        }
    }
}