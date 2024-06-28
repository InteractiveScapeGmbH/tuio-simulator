using System;
using UnityEngine;

namespace TuioSimulator.UI
{
    public class SimulatorMenu : MonoBehaviour
    {
        private RectTransform _rectTransform;
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            gameObject.SetActive(false);
        }

        public void Open(Vector2 position)
        {
            _rectTransform.anchoredPosition = position;
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}