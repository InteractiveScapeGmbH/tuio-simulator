using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TuioSimulator.UI
{
    
    public class ScreenCaptureSettings : MonoBehaviour
    {
        [SerializeField] private Toggle _toggle;
        [SerializeField] private TMP_Dropdown _sourceSelector;

        public string CurrentScreenCaptureSource { get; private set; } = null;

        private const string NoCamera = "No Camera";

        private void OnEnable()
        {
            _toggle.onValueChanged.AddListener(ToggleDropdown);
            _sourceSelector.onValueChanged.AddListener(SelectSource);
        }

        private void OnDisable()
        {
            _toggle.onValueChanged.RemoveAllListeners();
            _sourceSelector.onValueChanged.RemoveAllListeners();
        }
        
        private void ToggleDropdown(bool screenCaptureEnabled)
        {
            if (screenCaptureEnabled)
            {
                SetupCaptureSourceDropdown(_sourceSelector);
            }
            else
            {
                _sourceSelector.ClearOptions();
            }
            _sourceSelector.interactable = screenCaptureEnabled;
        }
        
        private void SelectSource(int index)
        {
            var source = _sourceSelector.options[index];
            Debug.Log(source.text);
            CurrentScreenCaptureSource = source.text == NoCamera ? null : source.text;
        }
        

        private void SetupCaptureSourceDropdown(TMP_Dropdown dropdown)
        {
            var devices = WebCamTexture.devices;
            dropdown.ClearOptions();
            var options = devices.Select(device => new TMP_Dropdown.OptionData(device.name)).ToList();
            if (options.Count == 0)
            {
                options.Add(new TMP_Dropdown.OptionData(NoCamera));
            }
            dropdown.AddOptions(options);
            SelectSource(0);
        }
    }
}
