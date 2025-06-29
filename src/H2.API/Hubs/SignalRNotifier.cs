using H2.API.Hubs;
using H2.Application.Interfaces.Services;
using H2.Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace H2.API.Services
{
    public class SignalRNotifier : IHubNotifier
    {
        private readonly IHubContext<MonitoringHub> _hubContext;
        public SignalRNotifier(IHubContext<MonitoringHub> hubContext)
        {
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        }
        public Task PushAnomalyImageAsync(Guid imageId, string imageUrl, CancellationToken cancellationToken)
        {
            return _hubContext.Clients.All.SendAsync("AnomalyImageRecieved", new
            {
                ImageId = imageId,
                ImageUrl = imageUrl
            }, cancellationToken);
        }

        public Task PushSensorDataAsync(SensorData sensorData, CancellationToken cancellationToken)
        {
            return _hubContext.Clients.All.SendAsync("SensorDataRecieved", new SensorData
            {
                DeviceId = sensorData.DeviceId,
                Timestamp = sensorData.Timestamp,
                Ppm = sensorData.Ppm,
                AlertLevel = sensorData.AlertLevel
            }, cancellationToken);
        }
    }
}
