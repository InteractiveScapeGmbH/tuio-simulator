using UnityEngine;
using UnityEngine.EventSystems;

namespace TuioSimulation.Input
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
            _rectTransform.Rotate(Vector3.back, scrollDelta);
        }
    }
}