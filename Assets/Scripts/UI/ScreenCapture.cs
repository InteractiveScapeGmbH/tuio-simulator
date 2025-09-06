using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace TuioSimulator.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ScreenCapture : MonoBehaviour
    {
        private const string ObsCam = "OBS Virtual Camera";
        private RectTransform _rectTransform;
        private WebCamTexture _camTexture;
        private CanvasGroup _canvasGroup;
        

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            var rawImage = gameObject.AddComponent<RawImage>();
            
            var isObsActive = WebCamTexture.devices.Any(device => device.name == ObsCam);
            if (!isObsActive) return;
            _camTexture = new WebCamTexture(ObsCam);
            rawImage.texture = _camTexture;
        }

        public void ToggleVisibility(bool isRunning)
        {
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