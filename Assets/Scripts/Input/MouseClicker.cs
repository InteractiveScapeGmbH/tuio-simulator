using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TuioSimulation.Input
{
    public class MouseClicker : MonoBehaviour, IPointerClickHandler
    {
        public event Action<Vector2> OnLeftClick;
        public event Action<Vector2> OnMiddleClick;
        public event Action<Vector2> OnRightClick;
        public void OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    OnLeftClick?.Invoke(eventData.position);
                    break;
                case PointerEventData.InputButton.Middle:
                    OnMiddleClick?.Invoke(eventData.position);;
                    break;
                case PointerEventData.InputButton.Right:
                    OnRightClick?.Invoke(eventData.position);
                    break;
            }
        }


    }
}
