using System.Collections;
using System.Collections.Generic;
using TuioSimulator.Tuio.Common;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TuioSimulator
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Tuio20PointerPreview : DebugTuio
    {
        private RectTransform _rectTransform, _parent;
       private CanvasGroup _canvasGroup;

        public float Angle => -_rectTransform.eulerAngles.z * Mathf.Deg2Rad;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _parent = transform.parent as RectTransform;
        }

        void OnEnable()
        {
            Update();
        }

        void Update()
        {
            Vector2 mousePos;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parent,
                Mouse.current.position.ReadValue(),
                Camera.main,
                out mousePos
            );
            _rectTransform.anchoredPosition = mousePos;
            
            SetVisible(_parent.rect.Contains(mousePos));
        }

        void SetVisible(bool visible)
        {
            _canvasGroup.alpha = visible ? 1 : 0;
            _canvasGroup.blocksRaycasts = visible;
            _canvasGroup.interactable = visible;
        }


        public override string DebugText()
        {
            return $"\nAngle: {Angle * 180.0 / Mathf.PI:F2}";
        }
    }
}