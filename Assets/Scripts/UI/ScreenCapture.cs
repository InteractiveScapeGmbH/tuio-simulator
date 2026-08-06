using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace TuioSimulator.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ScreenCapture : MonoBehaviour
    {
        [SerializeField] private ScreenCaptureSettings _settings;
        private RectTransform _rectTransform;
        private WebCamTexture _camTexture;
        private CanvasGroup _canvasGroup;
        private RawImage _rawImage;
        

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            _rawImage = gameObject.AddComponent<RawImage>();
        }

        public void ToggleVisibility(bool isRunning)
        {
            if (_settings.CurrentScreenCaptureSource == null) return;
            if (!_camTexture)
            {
                _camTexture = new WebCamTexture(_settings.CurrentScreenCaptureSource);
            }
            else
            {
                _camTexture.deviceName = _settings.CurrentScreenCaptureSource;
            }
            _rawImage.texture = _camTexture;
            if (isRunning)
            {
                _camTexture.Play();
                _canvasGroup.alpha = 1;
            }
            else
            {
                _camTexture.Stop();
                _canvasGroup.alpha = 0;
            }
        }
    }
}