using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TuioSimulator.UI
{
    public class MenuEntry : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler, ICanvasRaycastFilter
    {
        private Image _image;
        private RectTransform _rectTransform;
        private readonly Color _active = Color.turquoise;
        private readonly Color _inactive = Color.cornflowerBlue;
        private int _index;
        private Vector2 _center = new Vector2(0.5f, 0.5f);

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _image = GetComponent<Image>();
            _image.color = _inactive;
        }

        public void Init(int index, int count)
        {
            _index = index;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _image.color = _active;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            print(_index);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _image.color = _inactive;
        }

        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            Vector2 localPoint;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _rectTransform, sp, eventCamera, out localPoint))
                return false;

            Rect rect = _rectTransform.rect;

            // Convert to 0–1 normalized space
            Vector2 normalized = new Vector2(
                (localPoint.x - rect.x) / rect.width,
                (localPoint.y - rect.y) / rect.height
            );

            float distance = Vector2.Distance(normalized, _center);

            return distance <= 0.5f;
        }
    }
}