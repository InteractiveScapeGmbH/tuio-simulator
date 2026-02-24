using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TuioSimulator.UI
{
    public class RadialMenu : MonoBehaviour, IPointerMoveHandler
    {
        [SerializeField] private int _entryCount;
        [SerializeField] private int _radius;
        [SerializeField] private MenuEntry _entryPrefab;

        private const float evenOffset = 90f;
        private RectTransform _rectTransform;
        private float _segmentAngle;
        

        private float SegmentAngle()
        {
            var segmentAngle = (360f / _entryCount) % 360f;
            return segmentAngle;
        }

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _segmentAngle = SegmentAngle();
            var angle = _entryCount % 2 == 0 ? evenOffset : 0f;
            for (var i = 0; i < _entryCount; i++)
            {
                var entry = Instantiate(_entryPrefab, transform);
                entry.Init(i, _entryCount);
                var entryTransform = entry.transform as RectTransform;
                entryTransform.anchoredPosition = Quaternion.Euler(0, 0, -angle) * (Vector2.down*_radius);
                angle = (angle + _segmentAngle) % 360f;
            }
        }

       

        public void OnPointerMove(PointerEventData eventData)
        {
            var mousePosition = eventData.position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, mousePosition, null, out var point);
            point = point.normalized;
            var angle = Mathf.Atan2(point.x, point.y) * Mathf.Rad2Deg;

            if (_entryCount % 2 == 0)
            {
                angle += 90f;
            }
            
            if (angle < 0)
            {
                angle += 360f;
            }
            var index = Mathf.FloorToInt((angle +(0.5f * _segmentAngle)) % 360f / _segmentAngle);
            print(index);

        }
    }
}
