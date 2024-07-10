using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TuioSimulator.Utils
{
    public class MouseDebug : MonoBehaviour, IDragHandler
    {
        private RectTransform _rect;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
        }


        public void OnDrag(PointerEventData eventData)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_rect, eventData.position, Camera.main,
                    out var point))
            {
                print(point);
                print(Rect.PointToNormalized(_rect.rect, point));
            }
        }
    }
}
