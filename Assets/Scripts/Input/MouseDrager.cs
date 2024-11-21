using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TuioSimulator.Input
{
    public class MouseDrager : MonoBehaviour, IDragHandler
    {
        public event Action<PointerEventData> OnMove;
        public void OnDrag(PointerEventData eventData)
        {
            OnMove?.Invoke(eventData);
        }
    }
}