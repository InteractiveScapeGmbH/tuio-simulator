using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace TuioSimulator.Input
{
    public class MouseScroller : MonoBehaviour, IScrollHandler
    {        
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }
        
        public void OnScroll(PointerEventData eventData)
        {
            var scrollDelta = eventData.scrollDelta.y;
            var modifier = Keyboard.current.leftShiftKey.isPressed ? 0.1f : 1f;
            _rectTransform.Rotate(Vector3.forward, scrollDelta * modifier);
        }
    }
}