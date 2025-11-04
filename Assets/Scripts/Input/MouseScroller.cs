using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace TuioSimulator.Input
{
    public class MouseScroller : MonoBehaviour, IScrollHandler
    {        
        private RectTransform _rectTransform;
        private float scrollMultiplier = 10f;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }
        
        public void OnScroll(PointerEventData eventData)
        {
            var scroll = 0f;
            if (Mathf.Abs(eventData.scrollDelta.x) > Mathf.Abs(eventData.scrollDelta.y)) {
                scroll = -Mathf.Sign(eventData.scrollDelta.x);
            } else {
                scroll = Mathf.Sign(eventData.scrollDelta.y);
            }
            var scrollDelta = scroll * scrollMultiplier;
            var modifier = Keyboard.current.leftShiftKey.isPressed ? 0.1f : 1f;
            _rectTransform.Rotate(Vector3.forward, scrollDelta * modifier);
        }
    }
}