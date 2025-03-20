using System;
using System.Collections.Generic;
using TMPro;
using TuioNet.Common;
using TuioSimulator.App;
using TuioSimulator.Tuio.Common;
using TuioSimulator.Tuio.Tuio11;
using TuioSimulator.Tuio.Tuio20;
using UnityEngine;
using UnityEngine.UI;

namespace TuioSimulator.UI
{
    public class AppSettings : MonoBehaviour
    {
        [SerializeField] private TuioTransmitter _tuioTransmitter;
        [SerializeField] private ServerConfig _serverConfig;
        // [SerializeField] private SceneLoader _sceneLoader;
        [SerializeField] private TMP_Dropdown _tuioVersion;
        [SerializeField] private TMP_Dropdown _connectionType;
        [SerializeField] private TMP_InputField _portField;
        [SerializeField] private TMP_InputField _sourceNameField;
        [SerializeField] private Button _playButton;
        [SerializeField] private RectTransform _tuioSpawner;
        [SerializeField] private Tuio20Spawner _tuio20Spawner;
        [SerializeField] private Tuio11Spawner _tuio11Spawner;
        [SerializeField] private CanvasGroup _configurations;

        private bool _isRunning;
        
        private Tuio11Spawner _currentTuio11Spawner;
        private Tuio20Spawner _currentTuio20Spawner;
        
        public bool IsRunning
        {
            get => _isRunning;
            private set
            {
                _isRunning = value;
                _configurations.interactable = !_isRunning;
            }
        }
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
            _playButton.onClick.AddListener(ToggleSimulator);
        }

        private void OnDisable()
        {
            _playButton.onClick.RemoveAllListeners();
        }

        private void ToggleSimulator()
        {
            if (!IsRunning)
            {
                StartSimulator();
            }
            else
            {
                StopSimulator();
            }
        }

        private void StopSimulator()
        {
            switch (_serverConfig.TuioVersion)
            {
                case TuioType.Tuio:
                    Destroy(_currentTuio11Spawner.gameObject);
                    break;
                case TuioType.Tuio2:
                    Destroy(_currentTuio20Spawner.gameObject);
                    break;
            }
            _tuioTransmitter.Close();
            IsRunning = false;
        }

        private void StartSimulator()
        {
            var isPortValid = int.TryParse(_portField.text, out var port);
            var isTypeValid = Enum.TryParse<TuioType>(_tuioVersion.options[_tuioVersion.value].text, out var tuioType);
            var isConnectionValid =
                Enum.TryParse<TuioConnectionType>(_connectionType.options[_connectionType.value].text, out var connectionType);

            if (isPortValid && isTypeValid && isConnectionValid)
            {
                _serverConfig.TuioVersion = tuioType;
                _serverConfig.ConnectionType = connectionType;
                _serverConfig.Port = port;
                _serverConfig.Source = _sourceNameField.text;
                // _sceneLoader.LoadScene("SimulatorMain");
            }
            
            _tuioTransmitter.Open(tuioType, connectionType, port, _sourceNameField.text);

            switch (tuioType)
            {
                case TuioType.Tuio:
                    _currentTuio11Spawner = Instantiate(_tuio11Spawner, _tuioSpawner);
                    _currentTuio11Spawner.SetManager(_tuioTransmitter.Manager);
                    break;
                case TuioType.Tuio2:
                    _currentTuio20Spawner = Instantiate(_tuio20Spawner, _tuioSpawner);
                    _currentTuio20Spawner.SetManager(_tuioTransmitter.Manager);
                    break;
            }

            
            IsRunning = true;
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
