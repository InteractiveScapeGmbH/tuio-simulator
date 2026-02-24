using System;
using TMPro;
using TuioSimulator.Tuio.Sxm;
using UnityEngine;

namespace TuioSimulator.UI
{
    public class SxmSettings : MonoBehaviour
    {
        [SerializeField] private SxmConfig _sxmConfig;
        [SerializeField] private TMP_InputField _brokerUrlInput;
        [SerializeField] private TMP_InputField _brokerPort;
        [SerializeField] private TMP_InputField _roomId;

        private void OnEnable()
        {
            _brokerUrlInput.onValueChanged.AddListener(UpdateBrokerUrl);
            _brokerPort.onValueChanged.AddListener(UpdateBrokerPort);
            _roomId.onValueChanged.AddListener(UpdateRoomId);
        }


        private void OnDisable()
        {
            _brokerUrlInput.onValueChanged.RemoveAllListeners();
            _brokerPort.onValueChanged.RemoveAllListeners();
            _roomId.onValueChanged.RemoveAllListeners();
        }

        private void Start()
        {
            _brokerUrlInput.text = _sxmConfig.BrokerUrl;
            _brokerPort.text = _sxmConfig.BrokerPort.ToString();
            _roomId.text = _sxmConfig.RoomId;
        }

        private void UpdateRoomId(string roomId)
        {
            _sxmConfig.RoomId = roomId;
        }

        private void UpdateBrokerPort(string portText)
        {
            if (int.TryParse(portText, out var port))
            {
                _sxmConfig.BrokerPort = port;
            }
        }

        private void UpdateBrokerUrl(string url)
        {
            _sxmConfig.BrokerUrl = url;
        }
    }
}