using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TuioSimulator.UI
{
    public class RadialMenu : MonoBehaviour
    {
        [SerializeField] private int _entryCount;
        [SerializeField] private int _radius;
        [SerializeField] private MenuEntry _entryPrefab;
        [SerializeField] private CanvasGroup _canvasGroup;

        private const float evenOffset = 90f;
        private RectTransform _rectTransform;
        private float _segmentAngle;

        private Camera _camera;
        

        private float SegmentAngle()
        {
            var segmentAngle = (360f / _entryCount) % 360f;
            return segmentAngle;
        }

        private void Start()
        {
            _camera = Camera.main;
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

        public void Show()
        {
            _canvasGroup.alpha = 1f;
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0f;
        }

        public void SetPosition(Vector2 position)
        {
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent as RectTransform, position, _camera, out var localPoint))
                return;
            _rectTransform.anchoredPosition = localPoint;
        }
    }
}
