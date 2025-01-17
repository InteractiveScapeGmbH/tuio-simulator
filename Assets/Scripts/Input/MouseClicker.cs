using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TuioSimulator.Input
{
    public class MouseClicker : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
    {
        public event Action<Vector2> OnLeftClick;
        public event Action<Vector2> OnLeftDoubleClick;
        public event Action<Vector2> OnMiddleClick;
        public event Action<Vector2> OnRightClick;
        public event Action<Vector2> OnRightDoubleClick;
        
        public event Action<PointerEventData> OnLeftDown;
        public event Action<PointerEventData> OnMiddleDown;
        public event Action<PointerEventData> OnRightDown;
        
        public event Action<PointerEventData> OnLeftUp;
        public event Action<PointerEventData> OnMiddleUp;
        public event Action<PointerEventData> OnRightUp;

        public event Action<PointerEventData> OnLeftMove;
        public event Action<PointerEventData> OnMiddleMove;

        public event Action<PointerEventData> OnRightMove;

        private bool _potentialSingleClick = false;

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    if (eventData.clickCount == 1)
                    {
                        _potentialSingleClick = true;
                        StartCoroutine(SingleClick(eventData.position, OnLeftClick));
                    }else if (eventData.clickCount == 2)
                    {
                        _potentialSingleClick = false;
                        OnLeftDoubleClick?.Invoke(eventData.position);
                    }
                    break;
                case PointerEventData.InputButton.Middle:
                    OnMiddleClick?.Invoke(eventData.position);;
                    break;
                case PointerEventData.InputButton.Right:
                    if (eventData.clickCount == 1)
                    {
                        _potentialSingleClick = true;
                        StartCoroutine(SingleClick(eventData.position, OnRightClick));
                    }else if (eventData.clickCount == 2)
                    {
                        _potentialSingleClick = false;
                        OnRightDoubleClick?.Invoke(eventData.position);
                    }
                    break;
            }
        }

        IEnumerator SingleClick(Vector2 position, Action<Vector2> onclick)
        {
            float time = 0f;
            while (time < 0.1f)
            {
                time += Time.deltaTime;
                yield return null;
            }

            if (_potentialSingleClick)
            {
                onclick?.Invoke(position);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    OnLeftDown?.Invoke(eventData);
                    break;
                case PointerEventData.InputButton.Middle:
                    OnMiddleDown?.Invoke(eventData);;
                    break;
                case PointerEventData.InputButton.Right:
                    OnRightDown?.Invoke(eventData);
                    break;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    OnLeftUp?.Invoke(eventData);
                    break;
                case PointerEventData.InputButton.Middle:
                    OnMiddleUp?.Invoke(eventData);;
                    break;
                case PointerEventData.InputButton.Right:
                    OnRightUp?.Invoke(eventData);
                    break;
            }
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    OnLeftMove?.Invoke(eventData);
                    break;
                case PointerEventData.InputButton.Middle:
                    OnMiddleMove?.Invoke(eventData);
                    break;
                case PointerEventData.InputButton.Right:
                    OnRightMove?.Invoke(eventData);
                    break;
            }
        }
    }
}
