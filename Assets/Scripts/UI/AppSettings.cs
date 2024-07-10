using System;
using System.Collections.Generic;
using TMPro;
using TuioNet.Common;
using TuioSimulator.Tuio.Common;
using UnityEngine;
using UnityEngine.UI;

namespace TuioSimulator.UI
{
    public class AppSettings : MonoBehaviour
    {
        [SerializeField] private TuioTransmitter _tuioTransmitter;
        [SerializeField] private TMP_Dropdown _tuioVersion;
        [SerializeField] private TMP_Dropdown _connectionType;
        [SerializeField] private TMP_InputField _portField;
        [SerializeField] private TMP_InputField _sourceNameField;
        [SerializeField] private Button _playButton;
        
        private void Start()
        {
            SetupDropdown(_tuioVersion, typeof(TuioType));
            SetupDropdown(_connectionType, typeof(TuioConnectionType));
        }

        private void OnEnable()
        {
            _playButton.onClick.AddListener(StartSimulator);
        }

        private void OnDisable()
        {
            _playButton.onClick.RemoveAllListeners();
        }

        private void StartSimulator()
        {
            var isPortValid = int.TryParse(_portField.text, out var port);
            var isTypeValid = Enum.TryParse<TuioType>(_tuioVersion.options[_tuioVersion.value].text, out var tuioType);
            var isConnectionValid =
                Enum.TryParse<TuioConnectionType>(_connectionType.options[_connectionType.value].text, out var connectionType);

            if (isPortValid && isTypeValid && isConnectionValid)
            {
                _tuioTransmitter.Open(tuioType, connectionType, port, _sourceNameField.text);
            }
        }

        private void SetupDropdown(TMP_Dropdown dropdown, Type enumType)
        {
            dropdown.ClearOptions();
            var versions = Enum.GetValues(enumType);
            List<TMP_Dropdown.OptionData> options = new();
            foreach (var version in versions)
            {
                var data = new TMP_Dropdown.OptionData(version.ToString());
                options.Add(data);
            }

            dropdown.AddOptions(options);
        }
    }
}
