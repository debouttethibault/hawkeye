using Hawkeye.Central.Api.Models;
using Hawkeye.Central.Api.Options;
using Hawkeye.Central.Api.SignalR;
using Hawkeye.Central.Data.Models;
using Hawkeye.Central.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MQTTnet;
using MQTTnet.Formatter;
using MQTTnet.Protocol;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;

namespace Hawkeye.Central.Api.MQTT
{
    public class HawkeyeMqttService
    {
        public class UavTelemetryEventArgs(UavTelemetryMessage message) : EventArgs
        {
            public UavTelemetryMessage Message { get; private set; } = message;
        }
        public event EventHandler<UavTelemetryEventArgs>? TelemetryMessageReceived;

        private readonly ILogger<HawkeyeMqttService> _logger;
        private readonly IMqttClient _mqtt;
        private readonly MqttClientOptions _mqttOptions;

        private readonly ConcurrentDictionary<string, TaskCompletionSource<JsonElement>> _pendingRequestQueue;

        public HawkeyeMqttService(MqttOptions options,
            ILogger<HawkeyeMqttService> logger, ILogger<MqttNetLogger> mqttLogger)
        {
            _logger = logger;

            var mqttClientFactory = new MqttClientFactory(new MqttNetLogger(mqttLogger));

            _mqttOptions = mqttClientFactory.CreateClientOptionsBuilder()
                .WithConnectionUri(options.ConnectionString)
                .WithCredentials(options.Username, options.Password)
                .WithClientId(options.ClientId)
                .WithProtocolVersion(MqttProtocolVersion.V500)
                .WithTlsOptions(opts => opts
                    .UseTls()
                    .WithAllowUntrustedCertificates())
                .Build();

            _mqtt = mqttClientFactory.CreateMqttClient();
            _mqtt.ApplicationMessageReceivedAsync += MessageReceivedAsync;



            _pendingRequestQueue = new ConcurrentDictionary<string, TaskCompletionSource<JsonElement>>();
        }

        private const string TelemetryTopic = "tel";
        private const string ResponseTopic = "res";
        private const string RequestTopic = "req";
        private const string CommandTopic = "cmd";

        private void MessageReceived(MqttApplicationMessageReceivedEventArgs args)
        {
            var topic = args.ApplicationMessage.Topic;
            var topicSplit = topic.Split('/', StringSplitOptions.TrimEntries);
            if (topicSplit.Length < 4
                || !Guid.TryParse(topicSplit[2], out var uavId))
            {
                return;
            }

            JsonElement data;
            try
            {
                var reader = new Utf8JsonReader(args.ApplicationMessage.Payload);
                data = JsonSerializer.Deserialize<JsonElement>(ref reader);
            }
            catch (JsonException)
            {
                _logger.LogWarning("MessageReceivedAsync: Invalid message payload");
                return;
            }


            if (topicSplit[3].Equals(TelemetryTopic, StringComparison.OrdinalIgnoreCase))
            {
                var message = new UavTelemetryMessage { UavId = uavId, Data = data };
                TelemetryMessageReceived?.Invoke(this, new UavTelemetryEventArgs(message));
            }
            else if (topicSplit[3].Equals(ResponseTopic, StringComparison.OrdinalIgnoreCase))
            {
                var correlationData = args.ApplicationMessage.CorrelationData;
                if (correlationData == null || correlationData.Length == 0)
                {
                    _logger.LogWarning("MessageReceivedAsync: Response without correlation data");
                    return;
                }

                var correlationId = Encoding.UTF8.GetString(correlationData);
                if (!_pendingRequestQueue.TryGetValue(correlationId, out var tcs))
                {
                    _logger.LogWarning("MessageReceivedAsync: Response with unknown correlation data");
                    return;
                }

                tcs.SetResult(data);
                return;
            }

            _logger.LogWarning("MessageReceivedAsync: Unhandled message");
        }

        private Task MessageReceivedAsync(MqttApplicationMessageReceivedEventArgs args)
        {
            MessageReceived(args);
            return Task.CompletedTask;
        }

        public async Task ConnectAsync(CancellationToken ct)
        {
            await _mqtt.ConnectAsync(_mqttOptions, cancellationToken: ct);
            await _mqtt.SubscribeAsync("v1/uav/#", cancellationToken: ct);
        }

        public async Task DisconnectAsync(CancellationToken ct)
        {
            await _mqtt.DisconnectAsync(cancellationToken: ct);
        }

        public async Task SendCommandRequestPayloadAsync(string clientId, string? correlationId, JsonElement? data, CancellationToken ct)
        {
            var messageBuilder = new MqttApplicationMessageBuilder()
                .WithContentType("application/json")
                .WithPayload(JsonSerializer.SerializeToUtf8Bytes(data))
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce);

            if (string.IsNullOrEmpty(correlationId))
            {
                messageBuilder = messageBuilder
                    .WithTopic($"v1/uav/{clientId}/{CommandTopic}");
            }
            else
            {
                messageBuilder = messageBuilder
                    .WithTopic($"v1/uav/{clientId}/{RequestTopic}")
                    .WithCorrelationData(Encoding.UTF8.GetBytes(correlationId));
            }

            await _mqtt.PublishAsync(messageBuilder.Build(), ct);
        }

        public async Task<JsonElement?> SendRequestAsync(UavRequestMessage request, CancellationToken ct)
        {
            var correlationId = $"{request.UavId}_{request.Nonce}";

            var tcs = new TaskCompletionSource<JsonElement>();
            _pendingRequestQueue[correlationId] = tcs;

            await SendCommandRequestPayloadAsync(request.UavId.ToString(), correlationId, request.Payload, ct);

            var timeout = Task.Delay(TimeSpan.FromSeconds(10), ct);
            if (await Task.WhenAny(tcs.Task, timeout) == timeout)
            {
                _pendingRequestQueue.TryRemove(correlationId, out _);
                return null;
            }

            var resultMessage = await tcs.Task;
            return resultMessage;
        }

        public async Task SendCommandAsync(UavCommandMessage command, CancellationToken ct)
        {
            await SendCommandRequestPayloadAsync(command.UavId.ToString(), null, command.Payload, ct);
        }
    }
}
