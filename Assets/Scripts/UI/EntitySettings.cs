using System;
using Tweens;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TuioSimulator.UI
{
    public class EntitySettings : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private LayoutElement _layout;
        private const float MinWidth = 50f;
        private const float MaxWidth = 200f;
        private FloatTween _tween;
        private TweenInstance _instance;
        private void Start()
        {
            _layout = GetComponent<LayoutElement>();
            _layout.preferredWidth = MinWidth;

            _tween = new FloatTween
            {
                from = MinWidth,
                to = MaxWidth,
                duration = 0.3f,
                easeType = EaseType.CircInOut,
                usePingPong = false
            };
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            
            _instance?.Cancel();
            _tween.from = MinWidth;
            _tween.to = MaxWidth;
            _instance = gameObject.AddTween(_tween);
            _tween.onUpdate = (instance, value) =>
            {
                _layout.preferredWidth = value;
            };
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _instance?.Cancel();
            _tween.from = MaxWidth;
            _tween.to = MinWidth;
            _instance = gameObject.AddTween(_tween);
            _tween.onUpdate = (instance, value) =>
            {
                _layout.preferredWidth = value;
            };
        }
    }
}
