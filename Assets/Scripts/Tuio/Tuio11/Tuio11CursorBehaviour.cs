using TuioNet.Common;
using TuioNet.Tuio11;
using TuioSimulator.Tuio.Common;
using UnityEngine;
using Utils;

namespace TuioSimulator.Tuio.Tuio11
{
    public class Tuio11CursorBehaviour : MonoBehaviour
    {
        private TuioTransmitter _transmitter;
        private Tuio11Cursor _cursor;
        private TuioTime _time;
        private Vector2 _lastPosition;
        private Tuio11Manager _manager;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Init(Tuio11Manager tuioManager)
        {
            _manager = tuioManager;
            _time = TuioTime.GetSystemTime();
            _cursor = new Tuio11Cursor(_time, _manager.CurrentSessionId, 0, Vector2.zero.FromUnity(), Vector2.zero.FromUnity(), 0f);
            _manager.AddCursor(_cursor);
        }

        private void Update()
        {
            _time = TuioTime.GetSystemTime();
            var position = _rectTransform.anchoredPosition;
            position.x /= Screen.width;
            position.y /= Screen.height;
            
            var velocity = position - _lastPosition;
            _cursor.Update(_time, position.FromUnity(), velocity.FromUnity(), velocity.magnitude);
            _lastPosition = position;
        }

        private void OnDestroy()
        {
            _manager.RemoveCursor(_cursor);
        }
    }
}