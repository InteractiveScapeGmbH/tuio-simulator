using TuioNet.Common;
using TuioNet.Tuio20;
using TuioSimulator.Tuio.Common;
using UnityEngine;
using Utils;

namespace TuioSimulator.Tuio.Tuio20
{
    public class Tuio20PointerBehaviour : MonoBehaviour
    {
        private TuioTransmitter _transmitter;
        private Tuio20Pointer _pointer;
        private TuioTime _time;
        private Vector2 _lastPosition;
        private Tuio20Manager _manager;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Init(Tuio20Manager tuioManager)
        {
            _manager = tuioManager;
            _time = TuioTime.GetSystemTime();
            var container = new Tuio20Object(_time, _manager.CurrentSessionId);
            _pointer = new Tuio20Pointer(_time, container, 0, 0, Vector2.zero.FromUnity(), 0f, 0f, 0f, 0f, Vector2.zero.FromUnity(), 0f, 0f, 0f);
            _manager.AddEntity(_pointer);
        }

        private void Update()
        {
            _time = TuioTime.GetSystemTime();
            var position = _rectTransform.anchoredPosition;
            position.x /= Screen.width;
            position.y /= Screen.height;
            position.y = 1f - position.y;
            
            var velocity = position - _lastPosition;
            _pointer.Update(_time, 0, 0, position.FromUnity(), 0f, 0f, 0f, 0f, velocity.FromUnity(), 0f, 0f, 0f);
            _lastPosition = position;
        }

        private void OnDestroy()
        {
            _manager.RemoveEntity(_pointer);
        }
    }
}