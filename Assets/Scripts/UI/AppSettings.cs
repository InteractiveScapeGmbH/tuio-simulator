using System;
using System.Collections.Generic;
using TMPro;
using TuioNet.Common;
using TuioSimulator.App;
using TuioSimulator.Tuio.Common;
using UnityEngine;
using UnityEngine.UI;

namespace TuioSimulator.UI
{
    public class AppSettings : MonoBehaviour
    {
        [SerializeField] private ServerConfig _serverConfig;
        [SerializeField] private SceneLoader _sceneLoader;
        [SerializeField] private TMP_Dropdown _tuioVersion;
        [SerializeField] private TMP_Dropdown _connectionType;
        [SerializeField] private TMP_InputField _portField;
        [SerializeField] private TMP_InputField _sourceNameField;
        [SerializeField] private Button _playButton;
        
        private void Start()
        {
            var tuioVersion = (int)_serverConfig.TuioVersion;
            var connectionType = (int)_serverConfig.ConnectionType;
            SetupDropdown(_tuioVersion, _serverConfig.TuioVersion, tuioVersion);
            SetupDropdown(_connectionType, _serverConfig.ConnectionType, connectionType);
            _portField.text = _serverConfig.Port.ToString();
            _sourceNameField.text = _serverConfig.Source;
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
            var isTypeValid = Enum.TryParse<TuioVersion>(_tuioVersion.options[_tuioVersion.value].text, out var tuioType);
            var isConnectionValid =
                Enum.TryParse<TuioConnectionType>(_connectionType.options[_connectionType.value].text, out var connectionType);

            if (isPortValid && isTypeValid && isConnectionValid)
            {
                _serverConfig.TuioVersion = tuioType;
                _serverConfig.ConnectionType = connectionType;
                _serverConfig.Port = port;
                _serverConfig.Source = _sourceNameField.text;
                _sceneLoader.LoadScene("SimulatorMain");
            }
        }

        private void SetupDropdown(TMP_Dropdown dropdown, Enum configEnum, int defaultValue)
        {
            Type enumType = configEnum.GetType();
            dropdown.ClearOptions();
            var versions = Enum.GetValues(enumType);
            List<TMP_Dropdown.OptionData> options = new();
            foreach (var version in versions)
            {
                var data = new TMP_Dropdown.OptionData(version.ToString());
                options.Add(data);
            }

            dropdown.AddOptions(options);
            dropdown.value = defaultValue;
        }
    }
}
