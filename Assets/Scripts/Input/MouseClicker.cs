using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TuioSimulation.Input
{
    public class MouseClicker : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
    {
        public event Action<Vector2> OnLeftClick;
        public event Action<Vector2> OnLeftDoubleClick;
        public event Action<Vector2> OnMiddleClick;
        public event Action<Vector2> OnRightClick;
        
        public event Action<Vector2> OnLeftDown;
        public event Action<Vector2> OnMiddleDown;
        public event Action<Vector2> OnRightDown;
        
        public event Action<Vector2> OnLeftUp;
        public event Action<Vector2> OnMiddleUp;
        public event Action<Vector2> OnRightUp;

        public event Action<Vector2> OnLeftMove;
        public event Action<Vector2> OnMiddleMove;

        public event Action<Vector2> OnRightMove;

        private bool _potentialSingleClick = false;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    if (eventData.clickCount == 1)
                    {
                        _potentialSingleClick = true;
                        StartCoroutine(SingleClick(eventData.position));
                    }else if (eventData.clickCount == 2)
                    {
                        _potentialSingleClick = false;
                        print("double click");
                        OnLeftDoubleClick?.Invoke(eventData.position);
                    }
                    break;
                case PointerEventData.InputButton.Middle:
                    OnMiddleClick?.Invoke(eventData.position);;
                    break;
                case PointerEventData.InputButton.Right:
                    OnRightClick?.Invoke(eventData.position);
                    break;
            }
        }

        IEnumerator SingleClick(Vector2 position)
        {
            float time = 0f;
            while (time < 0.1f)
            {
                time += Time.deltaTime;
                yield return null;
            }

            if (_potentialSingleClick)
            {
                print("single click");
                OnLeftClick?.Invoke(position);
            }
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    OnLeftDown?.Invoke(eventData.position);
                    break;
                case PointerEventData.InputButton.Middle:
                    OnMiddleDown?.Invoke(eventData.position);;
                    break;
                case PointerEventData.InputButton.Right:
                    OnRightDown?.Invoke(eventData.position);
                    break;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    OnLeftUp?.Invoke(eventData.position);
                    break;
                case PointerEventData.InputButton.Middle:
                    OnMiddleUp?.Invoke(eventData.position);;
                    break;
                case PointerEventData.InputButton.Right:
                    OnRightUp?.Invoke(eventData.position);
                    break;
            }
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    OnLeftMove?.Invoke(eventData.position);
                    break;
                case PointerEventData.InputButton.Middle:
                    OnMiddleMove?.Invoke(eventData.position);;
                    break;
                case PointerEventData.InputButton.Right:
                    OnRightMove?.Invoke(eventData.position);
                    break;
            }
        }
    }
}
