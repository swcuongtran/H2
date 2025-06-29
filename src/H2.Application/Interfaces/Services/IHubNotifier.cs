using H2.Domain.Entities;

namespace H2.Application.Interfaces.Services
{
    public interface IHubNotifier
    {
        Task PushSensorDataAsync(SensorData sensorData, CancellationToken cancellationToken);
        Task PushAnomalyImageAsync(Guid imageId, string imageUrl, CancellationToken cancellationToken);
    }
}
