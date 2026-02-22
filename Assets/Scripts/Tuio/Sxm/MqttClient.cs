using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Adapter;
using MQTTnet.Client;
using TuioSimulator.Utils;

namespace TuioSimulator.Tuio.Sxm
{
    public class MqttClient
    {
        private readonly MqttFactory _factory = new();
        private IMqttClient _mqttClient;
        private MqttClientOptions _options;
        private MqttClientSubscribeOptions _subscribeOptions;
        private CancellationTokenSource _tokenSource;
        private CancellationToken _token;

        private Func<MqttApplicationMessageReceivedEventArgs, Task> _onMessageCallback;

        public bool IsConnected => _mqttClient.IsConnected;
        
        private string _url;
        private int _port;
        private readonly ILogger _logger;
        
        public MqttClient(string url, int port)
        {
            _logger = new UnityLogger();
            _mqttClient = _factory.CreateMqttClient();
            _url = url;
            _port = port;
            _options = new MqttClientOptionsBuilder()
                .WithWebSocketServer(o => { o.WithUri($"{url}:{port}/mqtt"); })
                .WithTlsOptions(o => o.UseTls())
                .Build();
        }

        public async void Connect(Func<MqttApplicationMessageReceivedEventArgs, Task> onMessage = null)
        {
            if (_mqttClient.IsConnected)
            {
                await Disconnect();
                _mqttClient = _factory.CreateMqttClient();
            }

            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
            _token.ThrowIfCancellationRequested();
            _onMessageCallback = onMessage;
            await Task.Run(async () =>
            {
                while (!_token.IsCancellationRequested)
                {
                    try
                    {
                        if (await _mqttClient.TryPingAsync(_token)) continue;
                        await _mqttClient.ConnectAsync(_options, CancellationToken.None);
                        _logger.LogInformation($"[MqttClient] Connected to MQTT Broker: {_url}:{_port}.");
                        var response = await _mqttClient.SubscribeAsync(_subscribeOptions, CancellationToken.None);
                        var topics = _subscribeOptions.TopicFilters.Select(filter => filter.Topic);
                        _logger.LogInformation($"[MqttClient] Subscribed to topic(s): {string.Join(" ", topics)}");
                        if (onMessage != null)
                        {
                            _mqttClient.ApplicationMessageReceivedAsync += _onMessageCallback;
                        }
                    }
                    catch (OperationCanceledException exception)
                    {
                        _logger.LogError($"[MqttClient] {exception.Message}");
                        break;
                    }
                    catch (ObjectDisposedException exception)
                    {
                        _logger.LogError($"[MqttClient] {exception.Message}");
                        break;
                    }
                    catch (MqttConnectingFailedException exception)
                    {
                        _logger.LogError($"[MqttClient] Could not connect to MQTT Broker: {_url}:{_port} -> {exception.Message}");
                    }
                    finally
                    {
                        await Task.Delay(TimeSpan.FromSeconds(5));
                    }
                }
            }, _token);
        }

        public async Task Disconnect()
        {
            await _mqttClient.DisconnectAsync(
                new MqttClientDisconnectOptionsBuilder()
                    .WithReason(MqttClientDisconnectOptionsReason.NormalDisconnection).Build(), _token);
            _tokenSource.Cancel();
            _tokenSource.Dispose();
            _mqttClient.Dispose();
            _logger.LogInformation($"[MqttClient] Disconnected from MQTT Broker: {_url}:{_port}.");
        }

        public void Subscribe(string topic)
        {
            _subscribeOptions = _factory.CreateSubscribeOptionsBuilder().WithTopicFilter(filter =>
            {
                filter.WithTopic(topic);
            }).Build();
        }
    }
}