using System;
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

        private Action<Vector2, string> OnAddMobile;
        private Random _rng = new();

        public void Init(Action<Vector2, string> onAdd)
        {
            OnAddMobile = onAdd;
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
                float x = Mathf.Lerp(400f, 1600, (float)_rng.NextDouble());
                float y = Mathf.Lerp(200f, 900f, (float)_rng.NextDouble());

                OnAddMobile.Invoke(new Vector2(x,y), deviceInfo.DeviceId);
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