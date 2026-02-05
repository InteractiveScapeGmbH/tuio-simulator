using System.Text;
using System.Threading.Tasks;
using MQTTnet.Client;
using Newtonsoft.Json;
using TuioSimulator.Tuio.Tuio20;
using UnityEngine;

namespace TuioSimulator.Tuio.Sxm
{
    public class MqttBehaviour : MonoBehaviour
    {
        [SerializeField] private Tuio20Mobile _mobile;
        [SerializeField] private SxmConfig _sxmConfig;
        private MqttClient _mqttClient;

        private void Awake()
        {
            _mqttClient = new MqttClient(_sxmConfig.BrokerUrl, _sxmConfig.BrokerPort);
        }

        private void Start()
        {
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
                _mobile.Data = deviceInfo.DeviceId;
            }
            else
            {
                _mobile.Data = "Unknown";
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