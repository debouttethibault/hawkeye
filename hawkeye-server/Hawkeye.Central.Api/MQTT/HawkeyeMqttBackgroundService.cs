using System.Collections.Concurrent;
using Hawkeye.Central.Api.Models;
using Hawkeye.Central.Api.SignalR;
using Hawkeye.Central.Data;
using Hawkeye.Central.Data.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Hawkeye.Central.Api.MQTT
{
    public class HawkeyeMqttBackgroundService(ILogger<HawkeyeMqttBackgroundService> _logger, IServiceProvider _serviceProvider, HawkeyeMqttService _mqttService, IHubContext<HawkeyeHub> _hubContext) : BackgroundService
    {
        private readonly SemaphoreSlim _messageAvailable = new SemaphoreSlim(1);
        private readonly ConcurrentQueue<UavTelemetryMessage> _messageQueue = new ConcurrentQueue<UavTelemetryMessage>();

        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            using var scope = _serviceProvider.CreateScope();
            using var db = scope.ServiceProvider.GetRequiredService<HawkeyeDbContext>();

            _mqttService.TelemetryMessageReceived += MqttTelemetryMessageReceived;
            await _mqttService.ConnectAsync(ct);

            while (!ct.IsCancellationRequested)
            {
                await _messageAvailable.WaitAsync(ct);
                if (!_messageQueue.TryDequeue(out var message))
                {
                    continue;
                }

                await _hubContext.Clients.All.SendAsync("telemetry", message, ct);

                var telemetryEntity = new Telemetry
                {
                    UavId = message.UavId,
                    Key = $"uav/{message.UavId}",
                    ContentType = "application/json",
                    Content = message.Data,
                    CreatedOn = DateTimeOffset.UtcNow
                };

                try
                {
                    db.Add(telemetryEntity);
                    await db.SaveChangesAsync(ct);
                }
                catch (DbUpdateException)
                {
                    _logger.LogCritical("Failed saving telemetry entity");
                }
            }

            _mqttService.TelemetryMessageReceived -= MqttTelemetryMessageReceived;
            await _mqttService.DisconnectAsync(ct);
        }

        private void MqttTelemetryMessageReceived(object? sender, HawkeyeMqttService.UavTelemetryEventArgs args)
        {
            _messageQueue.Enqueue(args.Message);
            _messageAvailable.Release();
        }
    }
}
