using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MQTTnet.Client;
using Newtonsoft.Json;
using UnityEngine;
using Random = System.Random;

namespace TuioSimulator.Tuio.Sxm
{
    public class MqttBehaviour : MonoBehaviour
    {
        [SerializeField] private SxmConfig _sxmConfig;
        private MqttClient _mqttClient;

        private Action<string> OnAddMobile;
        private Action<string> OnRemoveMobile;
        private readonly HashSet<string> _activeMobiles = new();
        private Random _rng = new();
        
        public void Init(Action<string> onAdd, Action<string> onRemove)
        {
            OnAddMobile = onAdd;
            OnRemoveMobile = onRemove;
            _mqttClient = new MqttClient(_sxmConfig.BrokerUrl, _sxmConfig.BrokerPort);
            _mqttClient.Subscribe($"sxm/{_sxmConfig.RoomId}/box");
            _mqttClient.Connect(OnMessage);
        }

        private Task OnMessage(MqttApplicationMessageReceivedEventArgs message)
        {
            var payload = message.ApplicationMessage.PayloadSegment;
            var topic = message.ApplicationMessage.Topic.Replace("sxm/", "");
            if (payload.Array == null) return Task.CompletedTask;
            var decoded = Encoding.ASCII.GetString(payload.Array);
            var deviceInfo = JsonConvert.DeserializeObject<DeviceInfo>(decoded);
            
            if (deviceInfo.DeviceMovement == "stationary" && deviceInfo.DeviceTilt == "horizontal")
            {
                if (_activeMobiles.Contains(deviceInfo.DeviceId)) return Task.CompletedTask;
                _activeMobiles.Add(deviceInfo.DeviceId);
                OnAddMobile.Invoke(deviceInfo.DeviceId);
            }
            else if (deviceInfo.DeviceMovement != "stationary" && deviceInfo.DeviceTilt != "horizontal")
            {
                if (!_activeMobiles.Contains(deviceInfo.DeviceId)) return Task.CompletedTask;
                _activeMobiles.Remove(deviceInfo.DeviceId);
                OnRemoveMobile.Invoke(deviceInfo.DeviceId);
            }
           
            return Task.CompletedTask;
        }

        private async void OnDestroy()
        {
            if (_mqttClient.IsConnected)
            {
                await _mqttClient.Disconnect();
            }
        }
    }
}